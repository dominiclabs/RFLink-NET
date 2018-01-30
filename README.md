# RFLink-NET

C# Library and GUI tool for interacting with the: [RFLink Gateway](http://www.rflink.nl/blog2/)

## RFLink-NET Library
The library provides a simple interface to send and receive messages from the RFLink Gateway. 

To create a client there are three options:

    // Default serial port (COM1)
    RFLinkClient rfLinkClient = new RFLinkClient();
    // Define your own serial port
    RFLinkClient rfLinkClient = new RFLinkClient("COM3");
	// Pass in your own serial port object
	RFLinkClient rfLinkClient = new RFLinkClient(mySerialPort);

Next you'll need to add event handlers to receive debug output text and RF Link data

    rfLinkClient.EventLogOut += ReceivedStdOut;
    rfLinkClient.EventRFOut += ReceivedRFOut;

Then call connect which will return a boolean success status. The connect call will fetch and parse the RFLink Gateway's status and version information

	bool result = rfLinkClient.Connect()

If successful you will find the RFLinkClient has populated itself with the gateway's settings and version accessible through:

    RFLinkSettings settings = rfLinkClient.Settings;
Which contains the following read only properties:

    bool SetRF433
    bool SetNodoNRF
    bool SetMiLight
    bool SetLivingColors 
    bool SetAnsluta
    bool SetGPIO
    bool SetBLE 
    bool SetMySensors
    string Version 
    string Rev
    string Build

Any time the library receives RF data it will return a RFData object through your EventRFOut event handler.

    public class RFData
    {
        // Time the data was processed
        public DateTime DateTime
        // RFLink Gateway counter ID
        public string Counter
        // Unique key for the data ignoring state fields 
        public string HashKey 
        // Protocol used by the device
        public string Protocol
        // All fields returned by the gateway
        public Dictionary<string, string> Fields 
    }
### What are 'state fields'
When the RFData object is returned by the library it contains a HashKey. This key uniquely identifies an RF device based on all the data returned. 

As requirements will vary the state fields needs to be defined by the user. State fields are the fields that vary for a single device (e.g. TEMP, CMD, BAT...). For example you may want to ignore the ON/OFF field changing when calculating the hash by calling:

    rfLinkClient.StateFields.Add("CMD");

Which would result in these two commands being given the same HashKey:

    20;12;NewKaku;ID=000002;SWITCH=2;CMD=OFF;
    20;13;NewKaku;ID=000002;SWITCH=2;CMD=ON;


