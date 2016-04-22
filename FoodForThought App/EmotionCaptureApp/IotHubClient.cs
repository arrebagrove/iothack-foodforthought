using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Threading;



namespace EmotionCaptureApp
{
    public class IotHubClient
    {
        private static DeviceClient ClientRaspberryPiScale1;
        private static DeviceClient ClientRaspberryPiScale2;
        private static DeviceClient ClientRaspberryPiScale3;
        private static DeviceClient ClientRaspberryPiScale4;
        private static DeviceClient ClientRaspberryPiScale5;

        private static List<WeighingScale> allWeighingScales = new List<WeighingScale>();

        private static DeviceClient FootfallSensor1;
        private static DeviceClient FootfallSensor2;

        private static DeviceClient FaceRecognizerSensorGurgaon1;
        private static DeviceClient FaceRecognizerSensorBangalore1;

        private static bool sendWeightDataSignal = true;
        private static bool sendFootfallDataSignal = true;
        private static bool sendEmotionDataSignal = true;

        private static bool deviceClientsInitialized = false;

        private static string errorString = string.Empty;

        public static bool SendEmotionDataSignal
        {
            get
            {
                return sendEmotionDataSignal;
            }

            set
            {
                sendEmotionDataSignal = value;
            }
        }

        public static bool SendFootfallDataSignal
        {
            get
            {
                return sendFootfallDataSignal;
            }

            set
            {
                sendFootfallDataSignal = value;
            }
        }

        public static bool SendWeightDataSignal
        {
            get
            {
                return sendWeightDataSignal;
            }

            set
            {
                sendWeightDataSignal = value;
            }
        }

        public static string ErrorString
        {
            get
            {
                return errorString;
            }

            set
            {
                errorString = value;
            }
        }

        public async static Task InitializeConnections()
        {
            if (deviceClientsInitialized)
                return;
            //initialize all Client device connections to IotHub
            ClientRaspberryPiScale1 = DeviceClient.CreateFromConnectionString(ApplicationConstants.RaspberryPiScale1Key, TransportType.Http1);
            await ClientRaspberryPiScale1.OpenAsync();
            ClientRaspberryPiScale2 = DeviceClient.CreateFromConnectionString(ApplicationConstants.RaspberryPiScale2Key, TransportType.Http1);
            await ClientRaspberryPiScale2.OpenAsync();
            ClientRaspberryPiScale3 = DeviceClient.CreateFromConnectionString(ApplicationConstants.RaspberryPiScale3Key, TransportType.Http1);
            await ClientRaspberryPiScale3.OpenAsync();
            ClientRaspberryPiScale4 = DeviceClient.CreateFromConnectionString(ApplicationConstants.RaspberryPiScale4Key, TransportType.Http1);
            await ClientRaspberryPiScale4.OpenAsync();
            ClientRaspberryPiScale5 = DeviceClient.CreateFromConnectionString(ApplicationConstants.RaspberryPiScale5Key, TransportType.Http1);
            await ClientRaspberryPiScale5.OpenAsync();

            WeighingScale wscale1 = new WeighingScale();
            wscale1.DeviceId = ApplicationConstants.RaspberryPiScale1;
            wscale1.ItemName = "Bread";
            wscale1.ClientConnection = ClientRaspberryPiScale1;
            wscale1.BinSrNo = 1;
            wscale1.PercentageConsumption = 98;
            wscale1.RemainingQuantity = 150;
            wscale1.StartQuantity = 150;
            wscale1.Temperature = 35;
            wscale1.TemperatureUom = "celcius";
            wscale1.WeightUom = "kg";

            WeighingScale wscale2 = new WeighingScale();
            wscale2.DeviceId = ApplicationConstants.RaspberryPiScale2;
            wscale2.ItemName = "Pizza";
            wscale2.ClientConnection = ClientRaspberryPiScale2;
            wscale2.BinSrNo = 1;
            wscale2.PercentageConsumption = 97;
            wscale2.RemainingQuantity = 75;
            wscale2.StartQuantity = 75;
            wscale2.Temperature = 45;
            wscale2.TemperatureUom = "celcius";
            wscale2.WeightUom = "kg";

            WeighingScale wscale3 = new WeighingScale();
            wscale3.DeviceId = ApplicationConstants.RaspberryPiScale3;
            wscale3.ItemName = "Lasagna";
            wscale3.ClientConnection = ClientRaspberryPiScale3;
            wscale3.BinSrNo = 1;
            wscale3.PercentageConsumption = 97;
            wscale3.RemainingQuantity = 50;
            wscale3.StartQuantity = 50;
            wscale3.Temperature = 43;
            wscale3.TemperatureUom = "celcius";
            wscale3.WeightUom = "kg";

            WeighingScale wscale4 = new WeighingScale();
            wscale4.DeviceId = ApplicationConstants.RaspberryPiScale4;
            wscale4.ItemName = "Sandwich";
            wscale4.ClientConnection = ClientRaspberryPiScale4;
            wscale4.BinSrNo = 1;
            wscale4.PercentageConsumption = 80;
            wscale4.RemainingQuantity = 55;
            wscale4.StartQuantity = 55;
            wscale4.Temperature = 40;
            wscale4.TemperatureUom = "celcius";
            wscale4.WeightUom = "kg";


            WeighingScale wscale5 = new WeighingScale();
            wscale5.DeviceId = ApplicationConstants.RaspberryPiScale5;
            wscale5.ItemName = "Burger";
            wscale5.ClientConnection = ClientRaspberryPiScale5;
            wscale5.BinSrNo = 1;
            wscale5.PercentageConsumption = 80;
            wscale5.RemainingQuantity = 55;
            wscale5.StartQuantity = 55;
            wscale5.Temperature = 40;
            wscale5.TemperatureUom = "celcius";
            wscale5.WeightUom = "kg";


            allWeighingScales.Add(wscale1);
            allWeighingScales.Add(wscale2);
            allWeighingScales.Add(wscale3);
            allWeighingScales.Add(wscale4);
            allWeighingScales.Add(wscale5);

            FootfallSensor1 = DeviceClient.CreateFromConnectionString(ApplicationConstants.GurgaonFootfallSensor1Key, TransportType.Http1);
            await FootfallSensor1.OpenAsync();
            //FootfallSensor2 = DeviceClient.CreateFromConnectionString(ApplicationConstants.BangaloreFootfallSensor1Key, TransportType.Amqp);
            //await FootfallSensor2.OpenAsync();

            FaceRecognizerSensorGurgaon1 = DeviceClient.CreateFromConnectionString(ApplicationConstants.FacerecognizerGurgaonSensor1Key, TransportType.Http1);
            await FaceRecognizerSensorGurgaon1.OpenAsync();
            //FaceRecognizerSensorBangalore1 = DeviceClient.CreateFromConnectionString(ApplicationConstants.FacerecognizerBangaloreSensor1Key, TransportType.Amqp);
            //await FaceRecognizerSensorBangalore1.OpenAsync();

            deviceClientsInitialized = true;
        }


        public static async void SendWeightDataToIotHub()
        {
            SendWeightDataSignal = true;
            while (SendWeightDataSignal)
            {
                await SendWeightData();
                await Task.Delay(1000);
            }
        }

        public static void StopWeightDataToIotHub()
        {
            SendWeightDataSignal = false;
        }


        private static async Task SendWeightData()
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    if (allWeighingScales[i].RemainingQuantity <= 5)
                    {
                        allWeighingScales[i].RemainingQuantity = allWeighingScales[i].StartQuantity;
                        allWeighingScales[i].BinSrNo = allWeighingScales[i].BinSrNo +1;
                    }
                    var telemetryDataPoint = new
                    {
                        deviceId = allWeighingScales[i].DeviceId,
                        MenuItem = allWeighingScales[i].ItemName,
                        BinSrNo = allWeighingScales[i].BinSrNo,
                        RemainingWeight = allWeighingScales[i].RemainingQuantity,
                        UOM1 = allWeighingScales[i].WeightUom,
                        UOM2 = allWeighingScales[i].TemperatureUom,
                        Temperature = allWeighingScales[i].Temperature,
                        TimeFlag = DateTime.UtcNow
                    };
                    allWeighingScales[i].RemainingQuantity = allWeighingScales[i].RemainingQuantity * allWeighingScales[i].PercentageConsumption/100;
                    var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                    var message = new Message(Encoding.ASCII.GetBytes(messageString));
                    await allWeighingScales[i].ClientConnection.SendEventAsync(message);
                    System.Diagnostics.Debug.WriteLine("Pushed Message to Iot Hub :" + messageString);
                }
            }
            catch(Exception ex)
            {
                string exdata = ex.Message;
                ErrorString = "Error sending Weight Data" + ex.Message;
            }
    }





        public static async Task SendFootfallAndEmotionData(string emotion)
        {
            //Send the Emotion data to Iot Hub (positive/negative feedback on the menu
            try
            {
                var telemetryDataPoint = new
                {
                    Expression=emotion,
                    edatetime = DateTime.UtcNow
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                await FaceRecognizerSensorGurgaon1.SendEventAsync(message);
                System.Diagnostics.Debug.WriteLine("Pushed Emotion data to Iot Hub :" + messageString);
                
            }
            catch (Exception ex)
            {
                string exdata = ex.Message;
                ErrorString = "Error sending Emotion Data" + ex.Message;
                return;
            }


            //Send the footfall data to iotHub. Every time an emotion is captured, a footfall is considered and passed to IotHub
            try
            {
                Random rand = new Random();
                
                int val = rand.Next(1,5);

                var telemetryDataPoint = new
                {
                    CafeteriaID = "Bangalore Cafetaria",
                    SwipeInTime = DateTime.UtcNow,
                    Persons = val
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                await FaceRecognizerSensorGurgaon1.SendEventAsync(message);
                System.Diagnostics.Debug.WriteLine("Pushed Footfall data to Iot Hub :" + messageString);

            }
            catch (Exception ex)
            {
                string exdata = ex.Message;
                ErrorString = "Error sending footfall Data" + ex.Message;
            }
        }
    }
}
