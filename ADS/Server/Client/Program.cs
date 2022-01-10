using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IBanka> factory = new ChannelFactory<IBanka>("BankaService");
            ChannelFactory<IBezbednosniMehanizmi> factory2 = new ChannelFactory<IBezbednosniMehanizmi>("AuthService");
            IBezbednosniMehanizmi proxy2 = factory2.CreateChannel();
            IBanka proxy = factory.CreateChannel();


            string token = proxy2.Autentifikacija("admin", "admin");
            Console.WriteLine(token);


            Racun r1 = new Racun("123487621387", 58000, false);
            Racun r2 = new Racun("634623452345", 4500, true);
            Racun r3 = new Racun("758323641673", 100, false);
            Racun r4 = new Racun("748432345576", 14000, true);
            Racun r5 = new Racun("634745626345", 145000, false);
            Racun r6 = new Racun("112464747855", 4150, true);

            try
            {
                proxy.DodajRacun(r1, token);
                proxy.DodajRacun(r2, token);
                proxy.DodajRacun(r3, token);
                proxy.DodajRacun(r4, token);
                proxy.DodajRacun(r5, token);
                proxy.DodajRacun(r6, token);
                proxy.DodajRacun(r4, token);
            }
            catch(FaultException<SecurityException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }
            catch(FaultException<DatabaseException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }





            Console.ReadLine();
        }
    }
}
