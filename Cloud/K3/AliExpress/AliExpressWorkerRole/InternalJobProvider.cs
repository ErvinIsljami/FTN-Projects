using AlIExpress_Data;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AliExpressWorkerRole
{
    class InternalJobProvider : IBrotherConnection
    {
        public void ReadMessage(string message)
        {
            
        }

        private IBrotherConnection Connect()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            RoleInstanceEndpoint remoteInstanceEP = RoleEnvironment.Roles["AliExpress_Data"].Instances.Where(i => i.Id.Split('.').Last().Split('_').Last().Equals("0")).First().InstanceEndpoints["UnutrasnjiEP"];
            String remoteAddress = $"net.tcp://{remoteInstanceEP.IPEndpoint}/UnutrasnjiEP";
            return new ChannelFactory<IBrotherConnection>(binding, remoteAddress).CreateChannel();
        }
    }
}
