using DBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPStoBluetooth
{
    [Interface("org.bluez.GattCharacteristic1")]
    public interface GattCharacteristic1
    {
        byte[] ReadValue(IDictionary<string, object> options);
        void WriteValue(byte[] value, IDictionary<string, object> options);
        void StartNotify();
        void StopNotify();
    }
}
