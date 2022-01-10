using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalController
{
    public static class ControllerOperations
    {
        public static string LcName { get; set; }

        public static string ChooseName()
        {
            Console.Clear();
            Console.WriteLine("Izaberite ime za lokalni kontroler: ");
            LcName = Console.ReadLine();
            return LcName;
        }
     
        public static void Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Lokalni kontroler radi... Unesite 'exit' ako zelite da izadjete.");
                if (Console.ReadLine().Equals("exit"))
                {
                    Console.WriteLine("Aplikacija se gasi...");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Greska.. molimo pokusajte ponovo");
                }
            }
        }
    }
}
