using DBus;
using GPStoBluetooth.Interfaces;
using System;
using System.Collections.Generic;

namespace GPStoBluetooth
{
    class BluezServices
    {
        private string Service = "org.bluez";
        private Bus systemBus;
        private DBusConnection connection;
        public IList<Advertisement> advertisements { get; } = new List<Advertisement>();
        private Application application;
        private readonly int MAX_ADVERTISEMENTS = 4;
        
        public BluezServices(DBusConnection connection)
        {
            systemBus = connection.System;
            this.connection = connection;

            UnixExitSignal exitClass = new UnixExitSignal();
            exitClass.Exit += OnExit;
        }

        public bool IsLowEnergySupported()
        {
            ObjectManager manager = systemBus.GetObject<ObjectManager>(Service, ObjectPath.Root);
            var managedObjects = manager.GetManagedObjects();

            bool supported = false;

            foreach (var obj in managedObjects.Keys)
            {
                if (managedObjects[obj].ContainsKey(typeof(GattManager1).DBusInterfaceName())
                    && managedObjects[obj].ContainsKey(typeof(LEAdvertisingManager1).DBusInterfaceName()))
                {
                    supported = true;
                    break;
                }
            }

            return supported;
        }

        private ObjectPath GetAdapterPath()
        {
            ObjectManager manager = systemBus.GetObject<ObjectManager>(Service, ObjectPath.Root);
    
            var managedObjects = manager.GetManagedObjects();
            ObjectPath adapterPath = null;
            foreach (var obj in managedObjects.Keys)
            {
                if (managedObjects[obj].ContainsKey(typeof(Adapter1).DBusInterfaceName()))
                {
                    adapterPath = obj;
                    break;
                }
            }

            return adapterPath;
        }

        public void RegisterApplication(Application application)
        {
            GattManager1 applicationManager = systemBus.GetObject<GattManager1>(Service, GetAdapterPath());
            applicationManager.RegisterApplication(application.GetPath(), new Dictionary<string, object>());
            this.application = application;
            Console.WriteLine("Registered Gatt Application");
        }

        public void UnregisterApplication()
        {
            GattManager1 applicationManager = systemBus.GetObject<GattManager1>(Service, GetAdapterPath());
            applicationManager.UnregisterApplication(this.application.GetPath());
            this.application = null;
        }

        public void SetDiscoverable(bool discoverable)
        {
            systemBus.GetObject<org.freedesktop.DBus.Properties>(Service, GetAdapterPath()).Set("org.bluez.Adapter1", "Powered", discoverable);
            string output = discoverable ? "Bluetooth Discoverable" : "Bluetooth Not Discoverable";
            Console.WriteLine(output);
        }

        public void RegisterAdvertisement(MainAdvertisement advertisement)
        {
            if (advertisements.Count < MAX_ADVERTISEMENTS)
            {
                LEAdvertisingManager1 advertisingManager = systemBus.GetObject<LEAdvertisingManager1>(Service, GetAdapterPath());
                advertisingManager.RegisterAdvertisement(advertisement.GetPath(), new Dictionary<string, object>());
                advertisements.Add(advertisement);
                Console.WriteLine("Registered Advertisement");
            }
        }

        public void UnregisterAdvertisement(int index)
        {
            LEAdvertisingManager1 advertisingManager = systemBus.GetObject<LEAdvertisingManager1>(Service, GetAdapterPath());
            advertisingManager.UnregisterAdvertisement(advertisements[index].GetPath());
        }

        public void UnregisterAllAdvertisements()
        {
            LEAdvertisingManager1 advertisingManager = systemBus.GetObject<LEAdvertisingManager1>(Service, GetAdapterPath());

            foreach(Advertisement advertisement in advertisements)
            {
                advertisingManager.UnregisterAdvertisement(advertisement.GetPath());
            }

            advertisements.Clear();  
        }

        public void OnExit(object sender, EventArgs e)
        {
            UnregisterApplication();
            UnregisterAllAdvertisements();
            systemBus.Close();
            connection.Dispose();
            Environment.Exit(1);
        }
    }
}
