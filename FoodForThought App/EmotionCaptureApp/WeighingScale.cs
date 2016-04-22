using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionCaptureApp
{
    public class WeighingScale
    {

        private string deviceId;
        private DeviceClient clientConnection;
        private float remainingQuantity;
        private float startQuantity;
        private float temperature;
        private string weightUom;
        private string temperatureUom;
        private float percentageConsumption;
        private int binSrNo=0;
        private string itemName;

        public string DeviceId
        {
            get
            {
                return deviceId;
            }

            set
            {
                deviceId = value;
            }
        }

        public DeviceClient ClientConnection
        {
            get
            {
                return clientConnection;
            }

            set
            {
                clientConnection = value;
            }
        }

        public float RemainingQuantity
        {
            get
            {
                return remainingQuantity;
            }

            set
            {
                remainingQuantity = value;
            }
        }

        public float StartQuantity
        {
            get
            {
                return startQuantity;
            }

            set
            {
                startQuantity = value;
            }
        }

        public float Temperature
        {
            get
            {
                return temperature;
            }

            set
            {
                temperature = value;
            }
        }

        public string WeightUom
        {
            get
            {
                return weightUom;
            }

            set
            {
                weightUom = value;
            }
        }

        public string TemperatureUom
        {
            get
            {
                return temperatureUom;
            }

            set
            {
                temperatureUom = value;
            }
        }

        public float PercentageConsumption
        {
            get
            {
                return percentageConsumption;
            }

            set
            {
                percentageConsumption = value;
            }
        }

        public int BinSrNo
        {
            get
            {
                return binSrNo;
            }

            set
            {
                binSrNo = value;
            }
        }

        public string ItemName
        {
            get
            {
                return itemName;
            }

            set
            {
                itemName = value;
            }
        }
    }
}
