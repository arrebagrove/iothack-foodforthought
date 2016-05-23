using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace Device_Registration_Sample
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "HostName=foodforthoughtiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=DQn6/nYBV4WBt808p3smx6qGpAEz8iTD+s5Z+qrWrk8=";

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddDeviceAsync().Wait();
            Console.ReadLine();
        }


        private async static Task AddDeviceAsync()
        {
            string deviceId = "FacerecognizerBangaloreSensor1";
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }
    }
}
