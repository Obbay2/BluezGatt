using DBus;

namespace GPStoBluetooth
{
    public class LocationService : Service
    {
        public static string uuid = "00001821-0000-1000-8000-00805f9b34fb";
        public LocationService(Bus bus, int index) : base(bus, index, uuid, true)
        {
            AddCharacteristic(new LocationConfigurationCharacteristic(bus, 0, this));
            AddCharacteristic(new LatitudeCharacteristic(bus, 1, this));
            AddCharacteristic(new LongitudeCharacteristic(bus, 2, this));
        }
    }
}
