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
            ChannelFactory<IServiceZvucnika> factory = new ChannelFactory<IServiceZvucnika>("AjGaJebi123");
            IServiceZvucnika proxy = factory.CreateChannel();

            Zvucnik z = new Zvucnik("Intex", "55apfoek", 40, 120, 4100, 1);

            try
            {
                proxy.DodajZvucnik(z);
            }
            catch(FaultException<DbException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }


            Console.ReadLine();
        }
    }
}
