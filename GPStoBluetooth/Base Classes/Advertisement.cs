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
    class Advertisement : DBusObject, LEAdvertisement1, Properties
    {       
        private string Type { get; set; } = "";
        private IList<string> ServiceUUIDs { get; set; } = new List<string>();
        private Dictionary<string, object> ManufacturerData { get; set; } = new Dictionary<string, object>();
        private IList<string> SolicitUUIDs { get; set; } = new List<string>();
        private Dictionary<string, object> ServiceData { get; set; } = new Dictionary<string, object>();
        public bool IncludeTxPower { private get; set; } = false;

        private IDictionary<string, IDictionary<string, object>> GetProperties()
        {
            Dictionary<string, IDictionary<string, object>> response = new Dictionary<string, IDictionary<string, object>>();
            Dictionary<string, object> inner = new Dictionary<string, object>();

            inner["Type"] = Type;
            if (ServiceUUIDs.Any()) { inner["ServiceUUIDs"] = ServiceUUIDs.ToArray(); }
            if (SolicitUUIDs.Any()) { inner["SolicitUUIDs"] = SolicitUUIDs.ToArray(); }
            if (ManufacturerData.Any()) { inner["ManufacturerData"] = ManufacturerData;}
            if (ServiceData.Any()) { inner["ServiceData"] = ServiceData;}
            inner["IncludeTxPower"] = IncludeTxPower;

            response[typeof(LEAdvertisement1).DBusInterfaceName()] = inner;

            return response;
        }

        public void Release() { }

        protected void AddServiceUUID(string uuid)
        {
            ServiceUUIDs.Add(uuid);
        }

        protected void AddSolicitUUID(string uuid)
        {
            SolicitUUIDs.Add(uuid);
        }

        protected void AddManufacturerData(int manufacturerCode, object data)
        {
            ManufacturerData.Add(manufacturerCode.ToString(), data);
        }

        protected void AddServiceData(string uuid, object data)
        {
            ServiceData.Add(uuid, data);
        }

        [return: Argument("value")]
        public object Get(string @interface, string propname)
        {
            throw new NotImplementedException();
        }

        public void Set(string @interface, string propname, object value)
        {
            throw new NotImplementedException();
        }

        [return: Argument("props")]
        public IDictionary<string, object> GetAll(string @interface)
        {
            if (@interface != typeof(LEAdvertisement1).DBusInterfaceName())
            {
                throw new NotImplementedException();
            }

            return GetProperties()[typeof(LEAdvertisement1).DBusInterfaceName()];
        }

        public Advertisement(Bus bus, int index, string advertisingType)
        {
            this.basePath = "/ad" + index;
            Type = advertisingType;
            bus.Register(GetPath(), this);
        }
    }
}
