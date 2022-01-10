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
        private static IProcessService _proxy;
        static void Main(string[] args)
        {
            
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:10100/ProcessService";
            var channel = new ChannelFactory<IProcessService>(binding, address);
            int running = 1;
            _proxy = channel.CreateChannel();

            while (running == 1)
            {
                int option = Menu();

                Console.Clear();
                if(option == -1)
                {
                    Console.WriteLine("Wrong choise. Please try again");
                    continue;
                }
                string processName;
                int processId;

                switch (option)
                {
                    case 0:
                        Console.WriteLine("Press any key to exit");
                        Console.ReadKey();
                        running = 0;
                        break;

                    case 1:
                        StartProcess();
                        break;

                    case 2:
                        StopProcess();
                        break;

                    case 3:
                        try
                        {
                            Console.WriteLine(_proxy.ShowActiveProcess());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;

                    case 4:
                        try
                        {
                            _proxy.StopAllProcess();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        static int Menu()
        {
            Console.WriteLine("Choose: ");
            Console.WriteLine("1. Start process.");
            Console.WriteLine("2. Stop process.");
            Console.WriteLine("3. Show all active processes.");
            Console.WriteLine("4. Stop all active processes.");
            Console.WriteLine("0. Exit."); 

            int option = -1;

            if(Int32.TryParse(Console.ReadLine(), out option))
            {
                return option;
            }

            return -1;
        }

        static void StartProcess()
        {
            string processName;
            long processId;
            Console.WriteLine("Please enter process name: ");
            processName = Console.ReadLine();
            Console.WriteLine("Please enter process id: ");
            while (true)
            {
                if (!long.TryParse(Console.ReadLine(), out processId))
                {
                    Console.WriteLine("Wrong id input. Please input number.");
                    continue;
                }
                break;
            }
            ProcessModel process = new ProcessModel(processId, processName);
            try
            {
                _proxy.StartProcess(process);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static void StopProcess()
        {
            long processId;
            Console.WriteLine("Please enter process id: ");
            while (true)
            {
                if (!long.TryParse(Console.ReadLine(), out processId))
                {
                    Console.WriteLine("Wrong id input. Please input number.");
                    continue;
                }
                break;
            }
            try
            {
                _proxy.StopProcess(processId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
