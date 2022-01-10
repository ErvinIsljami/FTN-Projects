using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using System.Runtime;
using System.Runtime.Remoting.Channels;

namespace CloudCompute
{
    class Program
    {
        static void Main(string[] args)
        {
            Compute compute = new Compute();
        
            Console.WriteLine("Compute Service started...");
            Console.WriteLine("Creating workers...");

            for (var i = 0; i < compute.NumOfContainers; i++)
            {
                int port = compute.ContainersStartingPort + i * 10;
                var proc = new Process();
                proc.StartInfo.FileName = compute.ContainerExe;
                // If we were to execute the ConsoleApp.exe in CMD with a string argument, we would need to do it like this:
                // ConsoleApp.exe "C:\Users\Nikola Karaklic\Documents\Visual Studio 2017\Projects\CloudProjekat\ConsoleApp\Folder"
                // An argument that is a string neeeds to be in double-quotes when passed, that's why we have these double-quotes
                proc.StartInfo.Arguments = $"\"{compute.ContainersPartialDirectory}{i}\" {port} {i}";                
                proc.Start();

                var containerData = new ContainerData(i, port, $"{compute.ContainersPartialDirectory}{i}", null, null);

                // Save container data
                compute.RoleEnvironment.RoleInstances.Add(i, containerData);
                
                // Connect to each container proxy
                compute.Connect(containerData.Port);
            }            

            // Start the container state watcher
            compute.ContainerStateWatcher();

            RoleEnvironmentServer roleEnvironmentServer = new RoleEnvironmentServer(compute.RoleEnvironment);
            roleEnvironmentServer.Start();

            while (true)
            {
                //Console.WriteLine("Checking designated location...");
                Thread.Sleep(3000);
            }
        }
    }
}
