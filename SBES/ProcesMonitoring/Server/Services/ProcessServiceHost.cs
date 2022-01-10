using Common;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ProcessServiceHost
    {
        private ServiceHost svc;

        public ProcessServiceHost()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:10100/ProcessService";

            svc = new ServiceHost(typeof(ProcessService));
            svc.AddServiceEndpoint(typeof(IProcessService), binding, address);

            
            svc.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            svc.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            svc.Authorization.ServiceAuthorizationManager = new CustomServiceAuthorizationManager();
            svc.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            svc.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

        }

        public void StartService()
        {
            Console.WriteLine("Starting process service...");
            svc.Open();
            ProxyLog.Proxy.LogEvent("Process service opened.", ECriticalLvl.INFORMATION, DateTime.Now);
        }

        public void StopService()
        {
            svc.Close();
            ProxyLog.Proxy.LogEvent("Process service closed.", ECriticalLvl.INFORMATION, DateTime.Now);
        }
    }
}
