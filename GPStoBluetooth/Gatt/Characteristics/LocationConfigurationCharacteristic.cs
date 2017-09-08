using DBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPStoBluetooth
{
    public class LocationConfigurationCharacteristic : Characteristic
    {    
        private static string UUID = "00002AAD-0000-1000-8000-00805f9b34fb";
        public LocationConfigurationCharacteristic(Bus bus, int index, Service service) : base(bus, index, UUID, new[] { "read" }, service)
        {
            byte[] configFlags = { 0b00000000 };
            value = configFlags;
        }

        public override byte[] ReadValue(IDictionary<string, object> options)
        {
            return value;
        }
    }
}
