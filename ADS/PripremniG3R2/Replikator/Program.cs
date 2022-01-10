using Common;
using System;
using System.ServiceModel;
using System.Threading;

namespace Replikator
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IReplikatorService> factoryP = new ChannelFactory<IReplikatorService>("ReplikatorP");
            IReplikatorService proxyP = factoryP.CreateChannel();

            ChannelFactory<IReplikatorService> factoryS = new ChannelFactory<IReplikatorService>("ReplikatorS");
            IReplikatorService proxyS = factoryS.CreateChannel();

            while (true)
            {
                try
                {

                    var baza = proxyP.PreuzmiBazu();
                    proxyS.PosaljiBazu(baza);
                    Console.WriteLine("Replikacija odradjena");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Thread.Sleep(5000);
            }


        }
    }
}
