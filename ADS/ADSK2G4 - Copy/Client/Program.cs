using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IBezbednosniMehanizmi> factorySec = new ChannelFactory<IBezbednosniMehanizmi>("BezbednosniMehanizmi");
            IBezbednosniMehanizmi proxySec = factorySec.CreateChannel();

            ChannelFactory<ILaptopService> factory = new ChannelFactory<ILaptopService>("LaptopService");
            ILaptopService proxy = factory.CreateChannel();

            Laptop l1 = new Laptop("zenbook", "asus", 900);
            Laptop l2 = new Laptop("pro", "mcbook", 1200);
            Laptop l3 = new Laptop("k850l", "hp", 500);
            Laptop l4 = new Laptop("vivobook s", "asus", 800);
            Laptop l5 = new Laptop("vivobook pro", "asus", 700);
            Laptop l6 = new Laptop("vivobook max", "asus", 1400);

            string token = proxySec.Autentifikacija("admin", "admin");



            proxy.DodajLaptop(l1, token);




            Console.ReadLine();
        }
    }
}
