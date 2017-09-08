using System;
using WiringPi;
using System.Threading;
using System.Text;
using GPStoBluetooth.Interfaces;

namespace GPStoBluetooth
{
    class Program
    {

        static void Main(string[] args)
        {
            Run();
        }

        static void Run()
        {
           
            Init.WiringPiSetup();
            int result = Init.WiringPiSetupSys();
            int device = Serial.serialOpen("/dev/ttyS0", 9600);
            StringBuilder dataHolder = new StringBuilder();

            DBusConnection connection = new DBusConnection("org.GPSService");
            Application app = new Application(connection.System, "/org/bluez");
            app.AddService(new LocationService(connection.System, 0));

            MainAdvertisement ad = new MainAdvertisement(connection.System, 0);

            BluezServices bluezServices = new BluezServices(connection);

            if (bluezServices.IsLowEnergySupported())
            {
                bluezServices.SetDiscoverable(true);
                bluezServices.RegisterApplication(app);
                bluezServices.RegisterAdvertisement(ad);
            }

            while (true)
            {
                
                Tuple<string, string> LatLng = SerialReader.ReadLatLng(dataHolder, device);

                if (LatLng.Item1 != null && LatLng.Item2 != null)
                {
                    Characteristic latchrc = app.services[0].characteristics[1];
                    Characteristic lngchrc = app.services[0].characteristics[2];
                    latchrc.Set(typeof(GattCharacteristic1).DBusInterfaceName(), "Value", LatLng.Item1);
                    lngchrc.Set(typeof(GattCharacteristic1).DBusInterfaceName(), "Value", LatLng.Item2);
                    Console.WriteLine("Lat: " + LatLng.Item1 + " Long: " + LatLng.Item2);
                }

                Thread.Sleep(5);
            }
        }
    }
}
