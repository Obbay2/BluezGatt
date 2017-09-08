using DBus;
using System;
using System.Linq;

namespace GPStoBluetooth.Interfaces
{
    static class InterfaceUtility
    {
        public static string DBusInterfaceName(this Type type)
        {
            var attribute = type.GetCustomAttributes(false).SingleOrDefault(x => x.GetType() == typeof(InterfaceAttribute));
            if (attribute != null)
            {
                return (attribute as InterfaceAttribute).Name;
            }
            else
            {
                throw new InvalidOperationException("The specified type is not decorated with the Interface attribute");
            }
        }
    }
}
