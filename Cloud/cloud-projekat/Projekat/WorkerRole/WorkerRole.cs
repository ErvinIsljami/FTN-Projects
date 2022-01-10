using Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class WorkerRole : IWorkerRole
    {
        public void Start(string containerId)
        {

            ChannelFactory<IRoleEnvironment> factory = new ChannelFactory<IRoleEnvironment>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:502/Compute"));
            IRoleEnvironment proxy = factory.CreateChannel();
            

            string address = "";
            string[] brothers = null;
            int port = (Int32.Parse(containerId));

            Console.WriteLine("My new containerid is " + containerId);
            //Process.Start(@"C:\Users\ervin\Documents\Cloud\Projekat\Client\bin\Debug\Client.exe");
            
            try
            {
                 address =  proxy.GetAddress("Assembly.dll", port.ToString());
            }catch(TargetInvocationException e)
            {
                Console.Write(e.InnerException.Message);
            }
            Console.WriteLine(address);
            


            Console.WriteLine("My brothers: ");
            try
            {
                brothers = proxy.BrotherInstances("Assembly.dll", address);
            }catch(TargetInvocationException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            foreach (string s in brothers)
                Console.WriteLine(s);
            
        }

        public void Stop()
        {
            Console.WriteLine("STOP");
        }
    }
}