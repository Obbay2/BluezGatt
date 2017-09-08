using DBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPStoBluetooth
{
    [Interface("org.bluez.GattDescriptor1")]
    public interface GattDescriptor1
    {
        byte[] ReadValue();
        void WriteValue(byte[] value);
    }
}
