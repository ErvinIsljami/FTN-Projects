using AlIExpress_Data;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AliExpressWorkerRole
{
    class JobServer
    {
        private ServiceHost _externalServiceHost;
        private String _externalEPName = "SpoljasnjiEP";

        private ServiceHost _internalServiceHost;
        private String _internalEPName = "UnutrasnjiEP";

        public void AddExternalServiceEP()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };

            //EXTERNAL
            _externalServiceHost = new ServiceHost(typeof(ExternalJobProvider));
            RoleInstanceEndpoint externalEP = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[_externalEPName];
            String externalAddress = $"net.tcp://{externalEP.IPEndpoint}/{_externalEPName}";
            _externalServiceHost.AddServiceEndpoint(typeof(ISecondBrother), binding, externalAddress);
            Trace.TraceInformation("External service host setup.");
        }

        public void OpenExternal()
        {
            try
            {
                _externalServiceHost.Open();
                Trace.TraceInformation($"External service opened at {DateTime.Now}");
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
            }
        }

        public void CloseExternal()
        {
            try
            {
                _externalServiceHost.Close();
                Trace.TraceInformation($"External service closed at {DateTime.Now}");
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
            }
        }

        public void AddInternalServiceEP()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };

            //INTERNAL
            _internalServiceHost = new ServiceHost(typeof(InternalJobProvider));
            RoleInstanceEndpoint internalEP = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[_internalEPName];
            String internalAddress = $"net.tcp://{internalEP.IPEndpoint}/{_internalEPName}";
            _internalServiceHost.AddServiceEndpoint(typeof(IBrotherConnection), binding, internalAddress);
            Trace.TraceInformation("Internal service host setup.");
        }

        public void OpenInternal()
        {
            try
            {              
                if (_internalServiceHost.State == CommunicationState.Closed || _internalServiceHost.State == CommunicationState.Closing)
                {
                    AddInternalServiceEP();
                }

                _internalServiceHost.Open();
                Trace.TraceInformation($"Internal service opened at {DateTime.Now}");
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
            }
        }

        public void CloseInternal()
        {
            try
            {
                _internalServiceHost.Close();
                Trace.TraceInformation($"Internal service closed at {DateTime.Now}");
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
            }
        }
    }


    
}
