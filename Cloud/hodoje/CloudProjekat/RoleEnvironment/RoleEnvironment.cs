using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RoleEnvironment
{
    public class RoleEnvironment
    {
        public string ReturnAddress(string myAssemblyName, string containerId)
        {
            var binding = new NetTcpBinding();
            var endpoint = new EndpointAddress(new Uri($"net.tcp://localhost:50000/RoleEnvironment"));
            var factory = new ChannelFactory<IRoleEnvironment>(binding);
            var proxy = factory.CreateChannel(endpoint);
            return proxy.GetAddress(myAssemblyName, containerId);
        }

        public string[] ReturnBrotherInstancesAddresses(string myAssemblyName, string myAddress)
        {
            var binding = new NetTcpBinding();
            var endpoint = new EndpointAddress(new Uri($"net.tcp://localhost:50000/RoleEnvironment"));
            var factory = new ChannelFactory<IRoleEnvironment>(binding);
            var proxy = factory.CreateChannel(endpoint);
            return proxy.BrotherInstances(myAssemblyName, myAddress);
        }

        public string GetServiceAddress(string serviceName)
        {
            var binding = new NetTcpBinding();
            var endpoint = new EndpointAddress(new Uri($"net.tcp://localhost:50000/RoleEnvironment"));
            var factory = new ChannelFactory<IRoleEnvironment>(binding);
            var proxy = factory.CreateChannel(endpoint);
            return proxy.GetServiceAddress(serviceName);
        }
    }
}
