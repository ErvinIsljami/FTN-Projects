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
            ChannelFactory<IBiblioteka> factory1 = new ChannelFactory<IBiblioteka>("BibliotekaP");
            IBiblioteka proxy1 = factory1.CreateChannel();

            ChannelFactory<IBiblioteka> factory2 = new ChannelFactory<IBiblioteka>("BibliotekaS");
            IBiblioteka proxy2 = factory2.CreateChannel();

         

            while(true)
            {
                var baza = proxy1.PreuzmiBazu();
                proxy2.PosaljiBazu(baza);
                Console.WriteLine("Replikacija uspesna");

                Thread.Sleep(5000);
            }
            
        }
    }
}
