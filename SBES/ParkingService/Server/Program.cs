using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        public static ParkingServiceHost ParkingSvc;
        static void Main(string[] args)
        {
            ParkingSvc = new ParkingServiceHost();
            CheckPrimaryServerHost checkSvc = new CheckPrimaryServerHost();

            if(CheckIfPrimary())
            {
                ParkingSvc.StartService();
                checkSvc.StartService();
            }
            ParkingSvc.StartService();


            Console.ReadLine();
        }

        static bool CheckIfPrimary()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:10100/ParkingService";
            var channel = new ChannelFactory<ICheckPrimaryServer>(binding, address);
            ICheckPrimaryServer proxy = channel.CreateChannel();
            try
            {
                return !proxy.CheckIfAlive();
            }
            catch
            {
                return true;
            }
        }

        static async void SendToPrimary()
        {

        }
    }
}
