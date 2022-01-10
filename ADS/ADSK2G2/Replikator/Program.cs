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
            //ako treba i autentifikacija onda se prvo povezati na servis za autentifikaciju(za oba servisa)
            //zapamtiti oba tokena i metode pozivati sa tokenima
            /*
            ChannelFactory<ISecurityService> factory12 = new ChannelFactory<ISecurityService>("SecurityServicePrimarni");
            ISecurityService proxy12 = factory12.CreateChannel();
            ChannelFactory<ISecurityService> factory22 = new ChannelFactory<ISecurityService>("SecurityServiceSekundarni");
            ISecurityService proxy22 = factory22.CreateChannel();

            string token1 = proxy12.AuthenticateUser("admin", "admin");
            string token2 = proxy22.AuthenticateUser("admin", "admin");
            */
            ChannelFactory<IProdavnicaAutomobila> factory1 = new ChannelFactory<IProdavnicaAutomobila>("Primarni");
            ChannelFactory<IProdavnicaAutomobila> factory2 = new ChannelFactory<IProdavnicaAutomobila>("Sekundarni");
            IProdavnicaAutomobila proxy1 = factory1.CreateChannel();
            IProdavnicaAutomobila proxy2 = factory2.CreateChannel();


            while (true)
            {
                try
                {

                    var lista = proxy1.UzmiSve();
                    proxy2.UpisiSve(lista);
                    Console.WriteLine("Uspesno replicirani podaci...");
                }
                catch(Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }

                Thread.Sleep(10000);
            }



        }
    }
}
