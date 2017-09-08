using DBus;

namespace GPStoBluetooth
{
    class MainAdvertisement : Advertisement
    {

        public MainAdvertisement(Bus bus, int index) : base(bus, index, "peripheral")
        {
            AddServiceUUID("00001821-0000-1000-8000-00805f9b34fb");
            IncludeTxPower = false;
        }
    }
}
