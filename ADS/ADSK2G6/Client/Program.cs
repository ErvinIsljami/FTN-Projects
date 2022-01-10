using Common;
using System;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IApoteka> factory = new ChannelFactory<IApoteka>("Apoteka"); //ovo ime mora biti isto kao i namepoint name u app.config-u
            ChannelFactory<IBezbednosniMehanizmi> factory2 = new ChannelFactory<IBezbednosniMehanizmi>("Bezbednost"); //ovo ime mora biti isto kao i namepoint name u app.config-u

            IApoteka proxy = factory.CreateChannel();
            IBezbednosniMehanizmi proxy2 = factory2.CreateChannel();

            Lek l1 = new Lek("Bromazepan", "Hemofarm", 450);
            Lek l2 = new Lek("Andol", "Galenika", 280);
            Lek l3 = new Lek("Brufen", "Hemofarm", 370);
            Lek l4 = new Lek("Bensedin", "Hemofarm", 480);
            string token = "";
            try
            {
                token = proxy2.Autentifikacija("admin", "admin");
                Console.WriteLine("Moj token: " + token);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }



            try
            {
                proxy.DodajLek(l1, token);
                proxy.DodajLek(l2, token);
                proxy.DodajLek(l3, token);
                proxy.DodajLek(l4, token);
                proxy.DodajLek(l4, token);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }

            proxy.ObrisiLek(l4.Id, token);
            Console.WriteLine(proxy.VratiNajskupljiOdProizvodjaca("Hemofarm", token));




            Console.ReadLine();
        }
    }
}
