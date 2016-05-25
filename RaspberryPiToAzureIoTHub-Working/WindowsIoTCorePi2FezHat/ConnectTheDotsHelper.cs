using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Web.Http;
using Microsoft.Azure.Devices.Client;

namespace WindowsIoTCorePi2FezHat
{
    public class ConnectTheDotsHelper
    {
        int i = 0;
        // App Settings variables
        public AppSettings localSettings = new AppSettings();

        public List<ConnectTheDotsSensor> sensors;

        // Http connection string, SAS tokem and client
        DeviceClient deviceClient;
        bool HubConnectionInitialized = false;

        public ConnectTheDotsHelper(string iotDeviceConnectionString = "",
            string organization = "",
            string location = "",
            List<ConnectTheDotsSensor> sensorList = null)
        {
            localSettings.IoTDeviceConnectionString = iotDeviceConnectionString;
            sensors = sensorList;

            SaveSettings();
        }

        /// <summary>
        /// Validate the settings 
        /// </summary>
        /// <returns></returns>
        bool ValidateSettings()
        {
            if ((localSettings.IoTDeviceConnectionString == ""))
            {
                this.localSettings.SettingsSet = false;
                return false;
            }

            this.localSettings.SettingsSet = true;
            return true;

        }

        /// <summary>
        /// Apply new settings to sensors collection
        /// </summary>
        public bool SaveSettings()
        {
            if (ValidateSettings())
            {
                ApplySettingsToSensors();
                this.InitHubConnection();
                return true;
            }
            else {
                return false;
            }
        }


        /// <summary>
        ///  Apply settings to sensors collection
        /// </summary>
        public void ApplySettingsToSensors()
        {
           
        }

        private void SendAllSensorData()
        {
            foreach (ConnectTheDotsSensor sensor in sensors)
            {
                sensor.TimeFlag = DateTime.UtcNow.ToString("o");
                sendMessage(sensor.ToJson());
            }
        }

        public void SendSensorData(ConnectTheDotsSensor sensor)
        {
            sensor.TimeFlag = DateTime.UtcNow.ToString("o");

            if (i == 0)
            {
                sensor.DeviceId = "RaspberryScale1";
                sensor.MenuItem = "Bread";
                i++;
            }
            else if(i==1)
            {
                sensor.DeviceId = "RaspberryScale2";
                sensor.MenuItem = "Pasta";
                i++;
            }
            else if (i == 2)
            {
                sensor.DeviceId = "RaspberryScale3";
                sensor.MenuItem = "Lasagna";
                i++;
            }
            else if (i == 3)
            {
                sensor.DeviceId = "RaspberryScale4";
                sensor.MenuItem = "Burger";
                i++;
            }
            else if (i == 4)
            {
                sensor.DeviceId = "RaspberryScale5";
                sensor.MenuItem = "Sandwich";
                i++;
            }

            if (i == 5)
                i = 0;


            sendMessage(sensor.ToJson());
        }

        /// <summary>
        /// Send message to an IoT Hub using IoT Hub SDK
        /// </summary>
        /// <param name="message"></param>
        public async void sendMessage(string message)
        {
            if (this.HubConnectionInitialized)
            {
                try
                {
                    var content = new Message(Encoding.UTF8.GetBytes(message));
                    await deviceClient.SendEventAsync(content);

                    Debug.WriteLine("Message Sent: {0}", message, null);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception when sending message:" + e.Message);
                }
            }
        }

        public async Task<string> ReceiveMessage()
        {
            if (this.HubConnectionInitialized)
            {
                try
                {
                    var receivedMessage = await this.deviceClient.ReceiveAsync();

                    if (receivedMessage != null)
                    {
                        var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                        this.deviceClient.CompleteAsync(receivedMessage);
                        return messageData;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception when receiving message:" + e.Message);
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Initialize Hub connection
        /// </summary>
        public bool InitHubConnection()
        {
            try
            {
                this.deviceClient = DeviceClient.CreateFromConnectionString(localSettings.IoTDeviceConnectionString);
                this.HubConnectionInitialized = true;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
