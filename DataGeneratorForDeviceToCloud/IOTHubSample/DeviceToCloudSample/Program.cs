using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Threading;

namespace DeviceToCloudSample
{
    class Program
    {
        public static string DeviceId = "GurgaonFootfallSensor1";
        public static string DeviceId2 = "BangaloreFootfallSensor1";


        static DeviceClient deviceClient;
        static DeviceClient deviceClient2;
    


        static string DeviceConnectionString = "<Enter your key>";
        static string DeviceConnectionString2 = "<Enter your key>";

        static void Main(string[] args)
        {

            Console.WriteLine("Simulated device\n");

            //deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(DeviceId, deviceKey));


            //This is for MQTT protocol
            deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);
            deviceClient.OpenAsync().Wait();
            deviceClient2 = DeviceClient.CreateFromConnectionString(DeviceConnectionString2, TransportType.Mqtt);
            deviceClient2.OpenAsync().Wait();
            SendDeviceToCloudMessagesAsync();
            SendDeviceToCloudMessagesAsync2();



            // This is for http as the protocol
            //deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Http1);
            //deviceClient.OpenAsync().Wait();
            //deviceClient2 = DeviceClient.CreateFromConnectionString(DeviceConnectionString2, TransportType.Http1);
            //deviceClient2.OpenAsync().Wait();
            //SendDeviceToCloudMessagesHttpAsync();
            //SendDeviceToCloudMessagesHttpAsync2();



            Console.ReadLine();
        }


        private static async void SendDeviceToCloudMessagesAsync()
        {
            //double avgWindSpeed = 10; // m/s
            //Random rand = new Random();
            int l_counter = 0;

            Random footfall = new Random();
            Thread.Sleep(1000);
            while (true)
            {
                //double currentWindSpeed = avgWindSpeed + rand.NextDouble() * 4 - 2;

                var telemetryDataPoint = new
                {
                    CafeteriaID = "Bangalore Cafeteria",
                    SwipeInTime = DateTime.Now,
                    Persons = footfall.Next(0, 4)                    
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
                l_counter++;
                Thread.Sleep(2000);
            }
        }




        private static async void SendDeviceToCloudMessagesAsync2()
        {
            /*double avgWindSpeed = 10; // m/s
            Random rand = new Random();*/

            Random footfall = new Random();
            Thread.Sleep(1000);

            int l_counter = 0;
            while (true)
            {
                //double currentWindSpeed = avgWindSpeed + rand.NextDouble() * 4 - 2;

                var telemetryDataPoint = new
                {
                    CafeteriaID = "Gurgaon Cafeteria",
                    SwipeInTime = DateTime.Now,
                    Persons = footfall.Next(0, 4)
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient2.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
                l_counter++;
                Thread.Sleep(2000);
            }
        }


        private static async void SendDeviceToCloudMessagesHttpAsync()
        {
            double avgWindSpeed = 10; // m/s
            Random rand = new Random();

            while (true)
            {
                double currentWindSpeed = avgWindSpeed + rand.NextDouble() * 4 - 2;

                var telemetryDataPoint = new
                {
                    deviceId = DeviceId,
                    windSpeed = currentWindSpeed
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                //var message = new Message(Encoding.UTF8.GetBytes(messageString));
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                Thread.Sleep(1000);
            }
        }

        private static async void SendDeviceToCloudMessagesHttpAsync2()
        {
            double avgWindSpeed = 10; // m/s
            Random rand = new Random();

            while (true)
            {
                double currentWindSpeed = avgWindSpeed + rand.NextDouble() * 4 - 2;

                var telemetryDataPoint = new
                {
                    deviceId = DeviceId2,
                    windSpeed = currentWindSpeed
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient2.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                Thread.Sleep(1000);
            }
        }

    }
}
