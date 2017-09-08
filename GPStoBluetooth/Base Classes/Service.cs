using DBus;
using GPStoBluetooth.Base_Classes;
using GPStoBluetooth.Interfaces;
using org.freedesktop.DBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPStoBluetooth
{
    public class Service : DBusObject, GattService1, Properties
    {
        public IList<Characteristic> characteristics { get; } = new List<Characteristic>();
        private string UUID;
        private bool primary;

        public IDictionary<string, IDictionary<string, object>> GetProperties()
        {
            Dictionary<string, IDictionary<string, object>> response = new Dictionary<string, IDictionary<string, object>>();
            Dictionary<string, object> inner = new Dictionary<string, object>();

            inner["UUID"] = UUID;
            inner["Primary"] = primary;

            response[typeof(GattService1).DBusInterfaceName()] = inner;

            return response;
        }

        public ObjectPath[] GetCharacteristicPaths()
        {
            return GetChildPaths((IList<DBusObject>) characteristics);
        }

        public void AddCharacteristic(Characteristic characteristic)
        {
            characteristics.Add(characteristic);
        }

        [return: Argument("value")]
        public object Get(string @interface, string propname)
        {
            if (propname != "UUID" && propname != "Primary")
            {
                throw new NotSupportedException("Need to pass in 'UUID' or 'Primary' for second argument.");
            }

            return GetAll(@interface)[propname];
        }

        public void Set(string @interface, string propname, object value)
        {
            throw new NotImplementedException();
        }

        [return: Argument("props")]
        public IDictionary<string, object> GetAll(string @interface)
        {
            if (@interface != typeof(GattService1).DBusInterfaceName())
            {
                throw new NotSupportedException("Need to pass in 'GattService1' for first argument.");
            }

            return GetProperties()[typeof(GattService1).DBusInterfaceName()];
        }

        public Service(Bus bus, int index, string uuid, bool primary)
        {
            this.basePath = "/org/bluez/service" + index;
            this.UUID = uuid;
            this.primary = primary;
            bus.Register(GetPath(), this);
        }
    }
}
