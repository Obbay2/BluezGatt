using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiringPi;

namespace GPStoBluetooth
{
    class SerialReader
    {
        public static Tuple<string, string> ReadLatLng(StringBuilder dataHolder, int device)
        {

            string lat = null;
            string lng = null;

            if (Serial.serialDataAvail(device) != -1)
            {

                char data = (char)Serial.serialGetchar(device);
                dataHolder.Append(data);

                if (data == '\n')
                {
                    if (dataHolder.ToString().Contains("$GPGGA"))
                    {
                        int? latIndex = null;
                        int? lngIndex = null;
                        String[] splitData = dataHolder.ToString().Split(',');

                        for (int i = 0; i < splitData.Length; i++)
                        {
                            switch (splitData[i])
                            {
                                case "N": latIndex = i - 1; break;
                                case "S": latIndex = -(i - 1); break;
                                case "W": lngIndex = -(i - 1); break;
                                case "E": lngIndex = i - 1; break;
                            }
                        }

                        //Console.WriteLine(latIndex + "  " + lngIndex + "  " + latIndex.HasValue + " " + lngIndex.HasValue);
                        if (latIndex.HasValue && lngIndex.HasValue)
                        {
                            //Console.WriteLine("Indices at: " + latIndex + " " + lngIndex + " Data at: " + splitData[Math.Abs(latIndex.Value)] + " " + splitData[Math.Abs(lngIndex.Value)]);

                            lat = (Math.Sign(latIndex.Value) * double.Parse(splitData[Math.Abs(latIndex.Value)]) * 10000).ToString();
                            lng = (Math.Sign(lngIndex.Value) * double.Parse(splitData[Math.Abs(lngIndex.Value)]) * 10000).ToString();
                        }


                    }
                    dataHolder.Clear();
                }
            }

            return new Tuple<string, string>(lat, lng);
        }
    }
}
