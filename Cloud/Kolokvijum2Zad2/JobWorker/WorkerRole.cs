using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace JobWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private ServiceHost serviceHost;
        private ServiceHost serviceHost2;
        private IBrotherConnection Proxy;
        private bool otvoren = false;

        public override void Run()
        {
            Trace.TraceInformation("JobWorker is running");

            while (true)
            {
                Thread.Sleep(5000);
                if (RoleEnvironment.Roles["JobWorker"].Instances[0].Id == RoleEnvironment.CurrentRoleInstance.Id)
                {
                    CloudQueue queue = QueueHelper.GetQueueReference("vezba");
                    CloudQueueMessage message = queue.GetMessage();
                    
                    if (message == null)
                    {
                        continue;
                    }
                    else if (message.AsString == "otvori")
                    {
                        if(!otvoren)
                        {
                            Trace.TraceInformation("Otvaramo WCF.");
                            HostFirst();
                            serviceHost.Open();
                            otvoren = true;
                            BlobHelper.InsertItem("otvoren", "komande", "blob");
                        }
                    }
                    else if(message.AsString == "zatvori")
                    {
                        if(otvoren)
                        {
                            Trace.TraceInformation("Zatvaramo WCF.");
                            serviceHost.Close();
                            otvoren = false;
                            BlobHelper.InsertItem("zatvoren", "komande", "blob");
                        }
                    }
                    if(message != null)
                        queue.DeleteMessage(message);   

                }

            }
        }
        

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            if(RoleEnvironment.Roles["JobWorker"].Instances[0].Id == RoleEnvironment.CurrentRoleInstance.Id)
            {
             
                HostFirst();
                BlobHelper.InitBlobs();
            }
            //hostovanje instance 1
            RoleInstanceEndpoint inputEndPoint = RoleEnvironment.Roles["JobWorker"].Instances[1].InstanceEndpoints["InternalEndpoint2"];

            string endpoint = String.Format("net.tcp://{0}/InternalEndpoint2", inputEndPoint.IPEndpoint.ToString());
                serviceHost2 = new ServiceHost(typeof(CallBrother));
                NetTcpBinding binding = new NetTcpBinding();
                serviceHost2.AddServiceEndpoint(typeof(ICallBrother), binding, endpoint);
                serviceHost2.Open();

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("JobWorker has been started");

            return result;
        }

        public void HostFirst()
        {
            //hostovanje instance 0
            RoleInstanceEndpoint inputEndPoint = RoleEnvironment.Roles["JobWorker"].Instances[0].InstanceEndpoints["InternalEndpoint"];
            string endpoint = String.Format("net.tcp://{0}/InternalEndpoint", inputEndPoint.IPEndpoint);
            serviceHost = new ServiceHost(typeof(BrotherConnection));
            NetTcpBinding binding = new NetTcpBinding();
            serviceHost.AddServiceEndpoint(typeof(IBrotherConnection), binding, endpoint);
        }

        public override void OnStop()
        {
            Trace.TraceInformation("JobWorker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("JobWorker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
