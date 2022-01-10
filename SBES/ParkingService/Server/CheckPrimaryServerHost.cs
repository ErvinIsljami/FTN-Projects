using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class CheckPrimaryServerHost
    {
        private ServiceHost svc;

        public CheckPrimaryServerHost()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:10200/CheckPrimaryServer";

            svc = new ServiceHost(typeof(ParkingService));
            svc.AddServiceEndpoint(typeof(ICheckPrimaryServer), binding, address);

            //svc.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            //svc.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            //svc.Authorization.ServiceAuthorizationManager = new CustomServiceAuthorizationManager();
            //svc.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            //List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            //policies.Add(new CustomAuthorizationPolicy());
            //svc.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
        }

        public void StartService()
        {
            svc.Open();
        }

        public void StopService()
        {
            svc.Close();
        }
    }
}
