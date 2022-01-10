using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemLoger.LogService;

namespace SystemLoger
{
    class Program
    {
        static LogServiceHost logService = new LogServiceHost();
        static void Main(string[] args)
        {
            Console.WriteLine("Starting loging service...");
            logService.StartService();
            using (StreamWriter sw = new StreamWriter("proccessLoger.log", true))
            {
                sw.WriteLine("Loging service started.");
            }

            Console.ReadLine();
            logService.StopService();
            using (StreamWriter sw = new StreamWriter("proccessLoger.log", true))
            {
                sw.WriteLine("Loging service stoped.");
            }
        }
    }
}
