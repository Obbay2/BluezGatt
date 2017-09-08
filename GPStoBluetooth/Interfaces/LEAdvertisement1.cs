using DBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPStoBluetooth.Interfaces
{
    [Interface("org.bluez.LEAdvertisement1")]
    public interface LEAdvertisement1
    {
        void Release();
    }
}
