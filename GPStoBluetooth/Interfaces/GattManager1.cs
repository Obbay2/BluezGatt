using DBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPStoBluetooth.Interfaces
{
    [Interface("org.bluez.GattManager1")]
    interface GattManager1
    {
        void RegisterApplication(ObjectPath application, IDictionary<string, object> options);
        void UnregisterApplication(ObjectPath application);
    }
}
