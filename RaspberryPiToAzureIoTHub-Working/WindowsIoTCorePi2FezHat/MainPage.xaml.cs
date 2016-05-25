// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WindowsIoTCorePi2FezHat
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using GHIElectronics.UWP.Shields;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        FEZHAT hat;
        DispatcherTimer telemetryTimer;
        DispatcherTimer commandsTimer;

        ConnectTheDotsHelper ctdHelper;

        /// <summary>
        /// Main page constructor
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            var deviceInfo = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();

            // Hard coding guid for sensors. Not an issue for this particular application which is meant for testing and demos
            List<ConnectTheDotsSensor> sensors = new List<ConnectTheDotsSensor> {
                new ConnectTheDotsSensor(0, "Kg", 0, "Celsius")
            };
            
            ctdHelper = new ConnectTheDotsHelper(iotDeviceConnectionString: "HostName=foodforthoughtiothub.azure-devices.net;DeviceId=RaspberryScale2;SharedAccessKey=rbfciKTp1rIrgjC8wiYayIEcf6Ql8pLtgG4ymprtlkc=",
                sensorList: sensors);
        }

        private async Task SetupHatAsync()
        {
            this.hat = await FEZHAT.CreateAsync();
            
            this.telemetryTimer = new DispatcherTimer();
            this.telemetryTimer.Interval = TimeSpan.FromMilliseconds(2000);
            this.telemetryTimer.Tick += this.TelemetryTimer_Tick;
            this.telemetryTimer.Start();
        }

        private void TelemetryTimer_Tick(object sender, object e)
        {
            // Light Sensor
            ConnectTheDotsSensor lSensor = ctdHelper.sensors.Find(item => item.UOM1 == "Kg");
            lSensor.LightLevel = this.hat.GetLightLevel();  //Using Light as weight
            lSensor.Temperature = this.hat.GetTemperature();    //Temperature reading

            this.ctdHelper.SendSensorData(lSensor);

            this.LightTextBox.Text = lSensor.LightLevel.ToString("P2", CultureInfo.InvariantCulture);
            this.LightProgress.Value = lSensor.LightLevel;

            // Temperature Sensor
            //var tSensor = ctdHelper.sensors.Find(item => item.measurename == "Temperature");
            //tSensor.value = this.hat.GetTemperature();
            //this.ctdHelper.SendSensorData(tSensor);

            this.TempTextBox.Text =  lSensor.Temperature.ToString("N2", CultureInfo.InvariantCulture);

            System.Diagnostics.Debug.WriteLine("Temperature: {0} °C, Light {1}", lSensor.Temperature.ToString("N2", CultureInfo.InvariantCulture), lSensor.LightLevel.ToString("P2", CultureInfo.InvariantCulture));
        }

        private async void CommandsTimer_Tick(object sender, object e)
        {
            string message = await ctdHelper.ReceiveMessage();

            if (message != string.Empty)
            {
                System.Diagnostics.Debug.WriteLine("Command Received: {0}", message);
                switch (message.ToUpperInvariant())
                {
                    case "RED":
                        hat.D2.Color = new FEZHAT.Color(255, 0, 0);
                        break;
                    case "GREEN":
                        hat.D2.Color = new FEZHAT.Color(0, 255, 0);
                        break;
                    case "BLUE":
                        hat.D2.Color = new FEZHAT.Color(0, 0, 255);
                        break;
                    case "OFF":
                        hat.D2.TurnOff();
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Unrecognized command: {0}", message);
                        break;
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize FEZ HAT shield
            await SetupHatAsync();
        }
    }
}
