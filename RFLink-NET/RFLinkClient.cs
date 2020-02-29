using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RFLinkNet
{
    public class RFEventArgs : EventArgs
    {
        public string Data { get; set; }
    }

    public enum LibraryStatus
    {
        Ready,
        WaitingForStatus,
        WaitingForVersion,
        NotReady
    }

    public class RFLinkClient : IDisposable
    {
        // Callback functions
        public event EventHandler EventLogOut = null;
        public event EventHandler EventRFOut = null;

        // StateFields define fields that vary such as CMD, TEMP that 
        // should be ignored when calculating a unique device hash.
        public List<string> StateFields { get => stateFields; }
        private List<string> stateFields = new List<string>();

        // RFLink Settings are returned from the device on load
        public RFLinkSettings Settings { get => settings; }
        private RFLinkSettings settings = new RFLinkSettings();

        private SerialPort serialPort = new SerialPort("COM1", 57600);
        private object receiveLock = new object();
        private Dictionary<string, RFData> knownDevices = new Dictionary<string, RFData>();
        private bool disposed = false;

        // The library's internal state
        private LibraryStatus libraryStatus = LibraryStatus.NotReady;
        private ManualResetEvent statusReceived = new ManualResetEvent(false);
        private ManualResetEvent versionReceived = new ManualResetEvent(false);
        private ManualResetEvent pingReceived = new ManualResetEvent(false);


        protected void ReturnStdOut(string text)
        {
            var data = new RFEventArgs { Data = text };

            if (EventLogOut != null)
            {
                var eventListeners = EventLogOut.GetInvocationList();

                for (int index = 0; index < eventListeners.Count(); index++)
                {
                    var methodToInvoke = (EventHandler)eventListeners[index];
                    methodToInvoke.BeginInvoke(this, data, EndAsyncEvent, null);
                }
            }
        }

        protected void ReturnRFOutput(RFData data)
        {
            if (EventRFOut != null)
            {
                var eventListeners = EventRFOut.GetInvocationList();

                for (int index = 0; index < eventListeners.Count(); index++)
                {
                    var methodToInvoke = (EventHandler)eventListeners[index];
                    methodToInvoke.BeginInvoke(this, data, EndAsyncEvent, null);
                }
            }
        }

        private void EndAsyncEvent(IAsyncResult iar)
        {
            var ar = (System.Runtime.Remoting.Messaging.AsyncResult)iar;
            var invokedMethod = (EventHandler)ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(iar);
            }
            catch (Exception e)
            {
                Console.WriteLine("An event listener threw an unexpected exception: {0}", e.Message);
            }
        }

        /// <summary>
        /// Construct a RFLink client with default port ("COM1")
        /// </summary>
        public RFLinkClient()
        {
            Setup("COM1");
        }

        /// <summary>
        /// Construct a RFLink client with option
        /// to specify port name
        /// </summary>
        /// <param name="port">Port name e.g. COM1 (default)</param>
        public RFLinkClient(string port)
        {
            if (String.IsNullOrEmpty(port))
            {
                throw new ArgumentNullException("port", "RFLinkClient contstructed with null port value");
            }

            Setup(port);
        }

        private void Setup(string port)
        {
            serialPort.PortName = port;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Parity = Parity.None;
            serialPort.WriteTimeout = 500;
            serialPort.ReadTimeout = 500;
            serialPort.RtsEnable = true;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);            
        }

        /// <summary>
        /// Construct RFLink client with existing serial port object
        /// </summary>
        /// <param name="serialPort"></param>
        public RFLinkClient(SerialPort serialPort)
        {
            this.serialPort = serialPort ?? throw new ArgumentNullException();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
        }

        ~RFLinkClient()
        {
            try
            {
                Close();
                Dispose(false);

            }
            catch
            {
                // We tried
            }
        }

        public bool Ping()
        {
            bool pingSuccess = false; 

            if (SendRawData(Commands.ConstructPacket("PING")))
            {
                if (pingReceived.WaitOne(TimeSpan.FromSeconds(3)))
                {
                    pingSuccess = true;
                }
            }

            pingReceived.Reset();
            return pingSuccess;
        }

        /// <summary>
        /// Send data to the RFLink serial port
        /// No data will be manipulated 
        /// </summary>
        /// <param name="data">Raw data to send</param>
        /// <param name="repeat">How many times to send data</param>
        public bool SendRawData(string data, int repeat = 1)
        {
            bool success = true;

            foreach (var i in Enumerable.Range(1, repeat))
            {
                try
                {
                    serialPort.WriteLine(data);

                    // Sleep if we're repeating
                    if (repeat > 1)
                    {
                        Thread.Sleep(250 + i);
                    }
                }
                catch (Exception)
                {
                    success = false;
                }
            }

            return success;
        }

        public void Close()
        {
            serialPort?.Close();
            libraryStatus = LibraryStatus.NotReady;
            versionReceived.Reset();
            statusReceived.Reset();
        }

        public bool Connect()
        {
            bool connected = false;

            try
            {
                serialPort.Open();

                libraryStatus = LibraryStatus.WaitingForStatus;
                RequestDeviceState(Commands.GetStatus, ref statusReceived, 3);

                libraryStatus = LibraryStatus.WaitingForVersion;
                RequestDeviceState(Commands.GetVersion, ref versionReceived, 3);

                libraryStatus = LibraryStatus.Ready;
                connected = true;
            }
            catch (Exception e)
            {
                ReturnStdOut(e.Message);
                Close();
                throw;
            }

            return connected;
        }

        private void RequestDeviceState(string request, ref ManualResetEvent waitevent, int attempts)
        {
            for (int i = 1; i < attempts; i++)
            {
                SendRawData(Commands.ConstructPacket(request));

                if (waitevent.WaitOne(TimeSpan.FromSeconds(3)))
                {
                    return;
                }
            }

            throw new TimeoutException($"RF did not return after {attempts} attempts: {libraryStatus.ToString()}");
        }

        /// <summary>
        /// Data to be processed when received by the 
        /// RFlink serial port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lock (receiveLock)
            {
                var indata = string.Empty;

                try
                {
                    SerialPort sp = (SerialPort)sender;
                    indata = sp.ReadLine();

                    Task.Run(() => ProcessIncoming(indata));
                }
                catch (FormatException)
                {
                    // Failed to process settings/version, ignore
                }
                catch (Exception ex)
                {
                    ReturnStdOut($"Invalid data {indata}. Exception {ex.Message.ToString()}");
                }
            }

        }

        private void ProcessIncoming(string indata)
        {
            try
            {
                RFData rf = ProtocolParser.ProcessData(indata);

                if (rf.Protocol == "STATUS")
                {
                    Settings.ProcessStatusResponse(rf);
                    statusReceived.Set();
                }
                else if (rf.Protocol.StartsWith("VER"))
                {
                    Settings.ProcessVerResponse(rf);
                    versionReceived.Set();
                }
                else if (rf.Protocol == "PONG")
                {
                    pingReceived.Set();
                }
                else if (libraryStatus == LibraryStatus.Ready)
                {
                    string hashkey = rf.CalculateHash(stateFields);

                    if (!knownDevices.ContainsKey(hashkey))
                    {
                        knownDevices.Add(hashkey, rf);
                    }

                    ReturnRFOutput(rf);
                }
            }
            catch(Exception)
            {

            }
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                serialPort?.Dispose();
                serialPort = null;
                statusReceived?.Dispose();
                versionReceived?.Dispose();
            }

            disposed = true;
        }
    }
}
