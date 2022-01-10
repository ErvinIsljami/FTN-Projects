using Common;
using System;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IProdavnicaAutomobila> factory = new ChannelFactory<IProdavnicaAutomobila>("ProdavnicaAutomobila");
            ChannelFactory<ISecurityService> factory2 = new ChannelFactory<ISecurityService>("SecurityService");
            IProdavnicaAutomobila proxy = factory.CreateChannel();
            ISecurityService proxy2 = factory2.CreateChannel();


            Automobil a1 = new Automobil("Audi", "A1", 800, 2001);
            string token = null;
            try
            {
                token = proxy2.AuthenticateUser("admin", "admin");
                Console.WriteLine("Ulogovao sam se.");
            }
            catch(FaultException<SecurityException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }

            proxy.DodajAuto(a1, token);

            try
            {
                proxy.VratiNajskupljiIzMarke("Audi", token);
                proxy.DodajAuto(a1, token);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }

            








            Console.ReadLine();

        }
    }
}
