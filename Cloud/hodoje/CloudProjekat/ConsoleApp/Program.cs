using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string containerDirectoryPath = args[0];

            int port;
            if (!Int32.TryParse(args[1], out port))
            {
                Console.WriteLine("Invalid argument for 'port'. Needs to be an integer.");
                return;
            }

            int id;
            if (!Int32.TryParse(args[2], out id))
            {
                Console.WriteLine("Invalid argument for 'id'. Needs to be an integer.");
                return;
            }

            Container container = new Container(containerDirectoryPath, id, port);

            Console.WriteLine($"Id: {container.Id}");
            Console.WriteLine($"Folder path: {container.ContainerDirectoryPath}");
            Console.WriteLine($"Port: {container.Port}");

            ContainerServer containerServer = new ContainerServer(container);
            containerServer.Start(port);

            Console.Read();
        }
    }
}
