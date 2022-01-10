using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [DataContract]
    public class ParkingZone
    {
        [DataMember]
        public double PricePerHour { get; set; }
        [DataMember]
        public string ZoneColor { get; set; }
        
        public ParkingZone()
        {
            
        }

        public ParkingZone(double pricePerHour, string color)
        {
            PricePerHour = pricePerHour;
            ZoneColor = color;
        }

        public override bool Equals(object obj)
        {
            return ZoneColor.Equals(((ParkingZone)obj).ZoneColor);
        }
    }
}
