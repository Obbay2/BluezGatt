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
            StringBuilder sb = new StringBuilder();

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
                int avail = Serial.serialDataAvail(device);
                if (avail != -1)
                {
                    
                    char data = (char) Serial.serialGetchar(device);

                    sb.Append(data);

                    if(data == '\n')
                    {
                        if(sb.ToString().Contains("$GPGGA"))
                        {
                            String[] splitted = sb.ToString().Split(',');
                            if (!splitted[2].Equals("") && !splitted[4].Equals(""))
                            {
                                Console.WriteLine("FIRST TIME");
                                for (int i = 0; i < splitted.Length; i++ )
                                {
                                    Console.WriteLine(splitted[i]);
                                }

                                splitted[2] = (double.Parse(splitted[2]) / 100).ToString();
                                splitted[4] = (double.Parse(splitted[4]) / 100).ToString();

                                Console.WriteLine("SECOND TIME");
                                for (int i = 0; i < splitted.Length; i++)
                                {
                                    Console.WriteLine(splitted[i]);
                                }

                                if (splitted[3].Contains("S"))
                                {
                                    splitted[2] = (-double.Parse(splitted[2])).ToString();
                                }
                                if (splitted[5].Contains("W"))
                                {
                                    splitted[4] = (-double.Parse(splitted[4])).ToString();
                                }

                                Characteristic latchrc = app.services[0].characteristics[1];
                                Characteristic lngchrc = app.services[0].characteristics[2];
                                latchrc.Set(typeof(GattCharacteristic1).DBusInterfaceName(), "Value", splitted[2]);
                                lngchrc.Set(typeof(GattCharacteristic1).DBusInterfaceName(), "Value", splitted[4]);
                                Console.WriteLine("Lat: " + splitted[2] + " Long: " + splitted[4]);
                            }
                        }
                        
                        sb.Clear();
                    }
                }
                else
                {
                    Console.WriteLine(avail);
                }
                Thread.Sleep(20);
            }
        }
    }
}
