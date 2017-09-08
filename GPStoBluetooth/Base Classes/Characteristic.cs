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
    public class Characteristic : DBusObject, Properties, GattCharacteristic1
    {

        private Service parentService;
        private int index;
        public List<Descriptor> descriptors { get; } = new List<Descriptor>();
        private string UUID;
        private string[] flags;

        public byte[] value = new byte[0];

        public IDictionary<string, IDictionary<string, object>> GetProperties()
        {
            Dictionary<string, IDictionary<string, object>> response = new Dictionary<string, IDictionary<string, object>>();
            Dictionary<string, object> inner = new Dictionary<string, object>();

            inner["UUID"] = UUID;
            inner["Flags"] = flags;
            inner["Service"] = parentService.GetPath();
            inner["Value"] = value;

            response[typeof(GattCharacteristic1).DBusInterfaceName()] = inner;

            return response;
        }

        public ObjectPath[] GetDescriptorPaths()
        {
            return GetChildPaths((IList<DBusObject>) descriptors);
        }

        public void AddCharacteristic(Descriptor descriptor)
        {
            descriptors.Add(descriptor);
        }

        [return: Argument("value")]
        public object Get(string @interface, string propname)
        {
            SecondArgCheck(propname);
            return GetAll(@interface)[propname];
        }

        public void Set(string @interface, string propname, object value)
        {
            FirstArgCheck(@interface);
            SecondArgCheck(propname);

            switch (propname)
            {
                case "UUID": UUID = (string) value; break;
                case "Flags": flags = (string[]) value; break;
                case "Service": parentService = (Service) value; break;
                case "Value": this.value = ConversionUtility.ToByteArray((string) value); break;
            }
        }

        [return: Argument("props")]
        public IDictionary<string, object> GetAll(string @interface)
        {
            FirstArgCheck(@interface);
            return GetProperties()[typeof(GattCharacteristic1).DBusInterfaceName()];
        }

        private void FirstArgCheck(string @interface)
        {
            if (@interface != typeof(GattCharacteristic1).DBusInterfaceName())
            {
                throw new NotSupportedException("Need to pass in 'GattCharacteristic1' for first argument.");
            }
        }

        private void SecondArgCheck(string propname)
        {
            if (propname != "UUID" && propname != "Flags" && propname != "Service" && propname != "Value")
            {
                throw new NotSupportedException("Need to pass in 'UUID', 'Flags', 'Service', or 'Value' for second argument.");
            }
        }

        public virtual byte[] ReadValue(IDictionary<string, object> options)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteValue(byte[] value, IDictionary<string, object> options)
        {
            throw new NotImplementedException();
        }

        public virtual void StartNotify()
        {
            throw new NotImplementedException();
        }

        public virtual void StopNotify()
        {
            throw new NotImplementedException();
        }

        public Characteristic(Bus bus, int index, string uuid, string[] flags, Service service)
        {
            parentService = service;
            this.basePath = parentService.GetPath().ToString() + "/char" + index;
            UUID = uuid;
            this.flags = flags;
            bus.Register(GetPath(), this);
        }
    }
}
