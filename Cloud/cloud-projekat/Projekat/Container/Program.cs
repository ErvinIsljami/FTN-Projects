using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Container
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ContainerService service = new ContainerService(args[0]);
            Console.WriteLine("Kontejner pokrenut na {0} portu", args[0]);
            Console.ReadKey();
            service.Stop();

        }

    }
}
