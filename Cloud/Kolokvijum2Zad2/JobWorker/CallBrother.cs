using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace JobWorker
{

    public class CallBrother : ICallBrother
    {
        IBrotherConnection proxy;
        public string IsBrotherAlive()
        {
            //klijent instanca 1 za instancu 0
            RoleInstanceEndpoint inputEndPoint = RoleEnvironment.Roles["JobWorker"].Instances[0].InstanceEndpoints["InternalEndpoint"];
            var binding = new NetTcpBinding();
            ChannelFactory<IBrotherConnection> factory = new ChannelFactory<IBrotherConnection>(binding, new EndpointAddress(string.Format("net.tcp://{0}/InternalEndpoint", inputEndPoint.IPEndpoint.ToString())));
            proxy = factory.CreateChannel();

            try
            {
                proxy.AreYouAlive();
                return "uspeh";
            }
            catch(Exception e)
            {
                return e.StackTrace;
            }
        }
    }
}
