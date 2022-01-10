using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Ticket
    {
        public string CarRegistration { get; set; }
        public ParkingZone ParkingZone { get; set; }

        public Ticket()
        {
            
        }

        public Ticket(string carRegistration, ParkingZone parkingZone)
        {
            CarRegistration = carRegistration;
            ParkingZone = parkingZone;
        }
    }
}
