using DBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPStoBluetooth.Interfaces
{
    [Interface("org.bluez.LEAdvertisingManager1")]
    interface LEAdvertisingManager1
    {
        void RegisterAdvertisement(ObjectPath advertisement, IDictionary<string, object> options);
        void UnregisterAdvertisement(ObjectPath service);
    }
}
