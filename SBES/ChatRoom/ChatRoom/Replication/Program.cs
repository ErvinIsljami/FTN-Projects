using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Replication
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                ChannelFactory<IReplication> cfIzvor = new ChannelFactory<IReplication>("Izvor");

                ChannelFactory<IReplication> cfOdrediste = new ChannelFactory<IReplication>("Odrediste");

                IReplication kIzvor = cfIzvor.CreateChannel();
                IReplication kOdrediste = cfOdrediste.CreateChannel();

                List<string> ret = kIzvor.PreuzmiBazu();
                kOdrediste.PosaljiBazu(ret);
                Thread.Sleep(3000);
            }
        }
    }
}
