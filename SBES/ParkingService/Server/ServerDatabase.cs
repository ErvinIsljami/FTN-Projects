using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class ServerDatabase
    {
        public static HashSet<ParkingZone> ParkingZones { get; }
        public static Dictionary<string, Ticket> Tickets { get; }
        
        static ServerDatabase()
        {
            ParkingZones = new HashSet<ParkingZone>();
            Tickets = new Dictionary<string, Ticket>();
        }

    }
}
