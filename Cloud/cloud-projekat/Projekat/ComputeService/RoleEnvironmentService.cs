using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using System.ServiceModel;

namespace ComputeService
{
    public class RoleEnvironmentService
    {
        public RoleEnvironmentService()
        {
            ServiceHost svc = new ServiceHost(typeof(RoleEnviroment));
            svc.AddServiceEndpoint(typeof(IRoleEnvironment),
            new NetTcpBinding(),
            new Uri("net.tcp://localhost:502/Compute"));
            svc.Open();
        }
    }
}
