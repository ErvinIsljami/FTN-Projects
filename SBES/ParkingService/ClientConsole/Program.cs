using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:10100/ParkingService";
            var channel  = new ChannelFactory<IParkingService>(binding, address);
            IParkingService proxy = channel.CreateChannel();

            proxy.AddTicketToUser("NS-123-PR","RED");

            Console.ReadLine();
        }
    }
}
