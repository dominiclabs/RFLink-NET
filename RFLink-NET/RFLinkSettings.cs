using System;

namespace RFLinkNet
{
    public class RFLinkSettings
    {
        private bool setMySensors;
        private bool setRF433;
        private bool setNodoNRF;
        private bool setMiLight;
        private bool setLivingColors;
        private bool setAnsluta;
        private bool setGPIO;
        private bool setBLE;
        private string build;
        private string version;
        private string rev;

        public bool SetRF433 { get => setRF433; set => setRF433 = value; }
        public bool SetNodoNRF { get => setNodoNRF; set => setNodoNRF = value; }
        public bool SetMiLight { get => setMiLight; set => setMiLight = value; }
        public bool SetLivingColors { get => setLivingColors; set => setLivingColors = value; }
        public bool SetAnsluta { get => setAnsluta; set => setAnsluta = value; }
        public bool SetGPIO { get => setGPIO; set => setGPIO = value; }
        public bool SetBLE { get => setBLE; set => setBLE = value; }
        public bool SetMySensors { get => setMySensors; set => setMySensors = value; }
        public string Version { get => version; set => version = value; }
        public string Rev { get => rev; set => rev = value; }
        public string Build { get => build; set => build = value; }

        public void ProcessStatusResponse(RFData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Fields["Protocol"] != "STATUS" ||
                   !data.Fields.ContainsKey("setRF433") ||
                   !data.Fields.ContainsKey("setNodoNRF") ||
                   !data.Fields.ContainsKey("setMilight") ||
                   !data.Fields.ContainsKey("setLivingColors") ||
                   !data.Fields.ContainsKey("setAnsluta") ||
                   !data.Fields.ContainsKey("setGPIO") ||
                   !data.Fields.ContainsKey("setMysensors") ||
                   !data.Fields.ContainsKey("setBLE"))
            {
                throw new FormatException("RF Data did not contain correct status data");
            }
            
            SetRF433 = ProtocolParser.ToBoolean(data.Fields["setRF433"]);
            SetNodoNRF = ProtocolParser.ToBoolean(data.Fields["setNodoNRF"]);
            SetMiLight = ProtocolParser.ToBoolean(data.Fields["setMilight"]);
            SetLivingColors = ProtocolParser.ToBoolean(data.Fields["setLivingColors"]);
            SetAnsluta = ProtocolParser.ToBoolean(data.Fields["setAnsluta"]);
            SetGPIO = ProtocolParser.ToBoolean(data.Fields["setGPIO"]);
            SetMySensors = ProtocolParser.ToBoolean(data.Fields["setMysensors"]);
            SetBLE = ProtocolParser.ToBoolean(data.Fields["setBLE"]);
        }

        public void ProcessVerResponse(RFData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (!data.Fields["Protocol"].StartsWith("VER") || !data.Fields.ContainsKey("REV") || !data.Fields.ContainsKey("BUILD"))
            {
                throw new FormatException("RF Data did not contain correct version data");
            }

            Version = data.Fields["Protocol"];
            Rev = data.Fields["REV"];
            Build = data.Fields["BUILD"];
        }
    }
}
