using DBus;
using GPStoBluetooth.Base_Classes;
using GPStoBluetooth.Interfaces;
using System.Collections.Generic;

namespace GPStoBluetooth
{
    public class Application : DBusObject, ObjectManager
    {
        public List<Service> services { get; } = new List<Service>();

        public Application(Bus bus, string basePath)
        {
            this.basePath = basePath;
            bus.Register(GetPath(), this);
        }

        public void AddService(Service service)
        {
            services.Add(service);
        }

        [return: Argument("objects")]
        public IDictionary<ObjectPath, IDictionary<string, IDictionary<string, object>>> GetManagedObjects()
        {
            Dictionary<ObjectPath, IDictionary<string, IDictionary<string, object>>> response = new Dictionary<ObjectPath, IDictionary<string, IDictionary<string, object>>>();

            foreach (Service service in services)
            {
                response[service.GetPath()] = service.GetProperties();
                foreach (Characteristic chrc in service.characteristics)
                {
                    response[chrc.GetPath()] = chrc.GetProperties();
                }
            }

            return response;
        }
    }
}
