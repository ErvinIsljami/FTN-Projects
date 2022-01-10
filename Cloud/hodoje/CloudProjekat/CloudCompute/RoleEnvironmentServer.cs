using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Contract;

namespace CloudCompute
{
    public class RoleEnvironmentServer
    {
        private ServiceHost _serviceHost;
        private RoleEnvironment _roleEnvironment;

        public RoleEnvironmentServer(RoleEnvironment roleEnvironment)
        {
            _roleEnvironment = roleEnvironment;
        }

        public void Start()
        {
            _serviceHost = new ServiceHost(_roleEnvironment);
            var serviceBehvaiour = _serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            serviceBehvaiour.InstanceContextMode = InstanceContextMode.Single;

            var binding = new NetTcpBinding();
            var endpoint = $"net.tcp://localhost:50000/RoleEnvironment";

            _serviceHost.AddServiceEndpoint(typeof(IRoleEnvironment), binding, endpoint);
            _serviceHost.Open();
        }

        public void Stop()
        {
            _serviceHost.Close();
        }
    }
}
