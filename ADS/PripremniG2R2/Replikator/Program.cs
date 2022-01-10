using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Replikator
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IReplikatorService> factoryP = new ChannelFactory<IReplikatorService>("Primarni");
            IReplikatorService proxyP = factoryP.CreateChannel();

            ChannelFactory<IReplikatorService> factoryS = new ChannelFactory<IReplikatorService>("Sekundarni");
            IReplikatorService proxyS = factoryS.CreateChannel();


            while(true)
            {
                try
                {
                    var baza = proxyP.PreuzmiBazu();
                    proxyS.PosaljiBazu(baza);
                    Console.WriteLine("Replikacija uradjena");
                    Thread.Sleep(5000);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }





        }
    }
}
