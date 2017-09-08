using System;

namespace GPStoBluetooth
{
    class ConversionUtility
    {
        public static byte[] ToByteArray(string StringToConvert)
        {
            char[] CharArray = StringToConvert.ToCharArray();
            byte[] ByteArray = new byte[CharArray.Length];

            for (int i = 0; i < CharArray.Length; i++)
            {
                ByteArray[i] = Convert.ToByte(CharArray[i]);
            }

            return ByteArray;
        }
    }
}
