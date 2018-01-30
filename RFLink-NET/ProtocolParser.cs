using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RFLinkNet
{
    public enum Direction
    {
        None = 0,
        MasterToRF = 10,
        RFToMaster = 20,
        MasterToMaster = 11
    }

    public static class Commands
    {
        public static readonly string GetStatus = "status;";
        public static readonly string GetVersion = "version;";

        public static string ConstructPacket(string request)
        {
            return $"{(int)Direction.MasterToRF};{request}";
        }
    }

    /// <summary>
    /// Describes a RF Data packet with helper functions
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class RFData : EventArgs
    {
        // Time the data was processed
        public DateTime DateTime { get => dateTime; set => dateTime = value; }
        // RFLink Gateway counter ID
        public string Counter { get => counter; set => counter = value; }
        // Unique key for the data ignoring state fields 
        public string HashKey { get => hashkey; set => hashkey = value; }
        // Protocol used by the device
        public string Protocol { get => protocol; set => protocol = value; }
        // All fields returned by the gateway
        public Dictionary<string, string> Fields { get => fields; }

        private DateTime dateTime;
        private string counter;
        private string hashkey;
        private string protocol; // Also available in fields
        private Dictionary<string, string> fields = new Dictionary<string, string>();
        private List<string> defaultFields = new List<string>();

        public RFData()
        {
            defaultFields.Add("Time");
            Fields.Add("Time", DateTime.Now.ToString("yyyy.mm.dd HH:mm:ss"));
        }


        public override string ToString()
        {
            string output = $"{DateTime}::";

            foreach (KeyValuePair<string, string> kvp in Fields)
            {
                output += string.Format("{0}:{1}:", kvp.Key, kvp.Value);
            }

            return output;
        }

        /// <summary>
        /// Calculate the unique hash of the RF data using
        /// all data fields (An RF device with CMD = ON will
        /// have a different hash to the same RF device with 
        /// CMD = OFF
        /// </summary>
        /// <returns>Hash string</returns>
        public string CalculateHash()
        {
            // Create an empty list
            List<string> fieldsToIgnore = new List<string>();
            return CalculateHash(fieldsToIgnore);
        }

        public string CalculateHash(List<string> fieldsToIgnore)
        {
            if (fieldsToIgnore == null)
            {
                throw new ArgumentNullException("fieldsToIgnore", "CalculateHash was passed a null value");
            }

            string hashingString = string.Empty;

            // Add the default set of fields to ignore
            fieldsToIgnore.AddRange(defaultFields);

            foreach (KeyValuePair<string, string> kvp in Fields)
            {
                if (fieldsToIgnore.Contains(kvp.Key) == false)
                {
                    hashingString += string.Format("{0}:{1}:", kvp.Key, kvp.Value);
                }
            }

            HashKey = String.Format("{0:X}", hashingString.GetHashCode());
            return HashKey;
        }


    }

    public static class ProtocolParser
    {
        private static readonly char DataDelim = ';';
        private static readonly char FieldDELIM = '=';

        internal static RFData ProcessData(string indata)
        {
            RFData rf = new RFData();

            string[] splitProtocol = indata.Split(DataDelim);
            rf.Counter = splitProtocol[1];

            // Set protocol in object and fields
            rf.Protocol = splitProtocol[2];
            rf.Fields.Add("Protocol", splitProtocol[2]);

            int labelcount = splitProtocol.Length - 1;

            ProcessesFields(splitProtocol, labelcount, ref rf);

            return rf;
        }

        internal static void ProcessesFields(string[] fields, int labelcount, ref RFData rf)
        {
            // Skip pass "20;xx;
            for (int i = 3; i < labelcount; i++)
            {
                try
                {
                    string[] labelSplit = fields[i].Split(FieldDELIM);
                    rf.Fields.Add(labelSplit[0], labelSplit[1]);

                }
                catch { }//todo: remove?
            }
        }

        internal static bool ToBoolean(string stringbool)
        {
            if (stringbool == "ON")
            {
                return true;
            }
            else if (stringbool == "OFF")
            {
                return false;
            }
            else
            {
                throw new ArgumentException("Could not convert RF string to bool");
            }
        }
    }


}
