using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ParkingService : IParkingService
    {
        public void AddParkingZone(ParkingZone parkingZone)
        {
            if(!ServerDatabase.ParkingZones.Add(parkingZone))
            {
                throw new Exception("Parking zone with given color exsits.");
            }
        }

        public void AddTicketToUser(string registration, string parkingZone)
        {

            ParkingZone zone = ServerDatabase.ParkingZones.FirstOrDefault(x => x.ZoneColor == parkingZone);
            if (zone != null)
            {
                ServerDatabase.Tickets[registration] = new Ticket(registration, zone);
            }
            else
            {
                throw new Exception("Given parkingZone doesnt exsits.");
            }
        }

        public void DeleteTicket(string registration)
        {
            if(ServerDatabase.Tickets.ContainsKey(registration))
            {
                ServerDatabase.Tickets.Remove(registration);
            }
            else
            {
                throw new Exception("Ticket for given registration doesnt exists.");
            }
        }

        public bool IsParkingPayed(string registration)
        {
            return ServerDatabase.Tickets.ContainsKey(registration);
        }

        public void PayParking(string zoneColor, string registration)
        {
            ParkingZone zone = ServerDatabase.ParkingZones.FirstOrDefault(x => x.ZoneColor == zoneColor);
            if(zone == null)
            {
                throw new Exception("Given parkingZone doesnt exsits.");
            }

            ServerDatabase.Tickets[registration] = new Ticket(registration, zone);
        }
    }
}
