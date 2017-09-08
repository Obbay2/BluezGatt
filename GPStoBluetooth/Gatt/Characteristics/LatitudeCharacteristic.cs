using DBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPStoBluetooth
{
    public class LatitudeCharacteristic : Characteristic
    {    
        private static string UUID = "00002AAE-0000-1000-8000-00805f9b34fb";
        public LatitudeCharacteristic(Bus bus, int index, Service service) : base(bus, index, UUID, new[] { "read" }, service) { }

        public override byte[] ReadValue(IDictionary<string, object> options)
        {
            return value;
        }
    }
}
