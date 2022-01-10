using Common;
using System;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IProdavnicaTelefona> factory = new ChannelFactory<IProdavnicaTelefona>("ProdavnicaTelefona"); //endpoint name iz app.config-a
            ChannelFactory<IBezbednosniMehanizmi> factory2 = new ChannelFactory<IBezbednosniMehanizmi>("Bezbednost"); //endpoint name iz app.config-a

            IProdavnicaTelefona proxy = factory.CreateChannel();
            IBezbednosniMehanizmi proxy2 = factory2.CreateChannel();
            string token = null;

            try
            {
                token = proxy2.Autentifikacija("pera", "pera");
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }

            Console.WriteLine("Token koji sam dobio je: " + token);

            Telefon t1 = new Telefon("Sony Ericson", "K510i", 200, 16);

            

            try
            {
                proxy.DodajTelefon(t1, token);
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
