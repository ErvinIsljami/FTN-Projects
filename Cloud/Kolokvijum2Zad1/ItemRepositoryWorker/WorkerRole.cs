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

namespace ItemRepositoryWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private TableHelper th = new TableHelper();
        ServiceHost svc = new ServiceHost(typeof(NotifyOthers));
        public override void Run()
        {
            Trace.TraceInformation("ItemRepositoryWorker is running");
            CloudQueue queue = QueueHelper.GetQueueReference("vezba");
            
            Trace.TraceInformation("ImageConverter_WorkerRole is running");

            while (true)
            {
                CloudQueueMessage message = queue.GetMessage();
                if (message == null)
                {
                    Trace.TraceInformation("Trenutno ne postoji poruka u redu.", "Information");
                }
                else
                {
                    Trace.TraceInformation(String.Format("Poruka glasi: {0}", message.AsString), "Information");
                    string name = th.GetId(message.AsString);
                    foreach (RoleInstance r in RoleEnvironment.Roles["ItemRepositoryWorker"].Instances)
                    {
                        if (r != RoleEnvironment.CurrentRoleInstance)
                        {
                            string adresa = r.InstanceEndpoints["InternalEndpoint"].IPEndpoint.ToString();
                            INotifyOthers proxy;
                            ChannelFactory<INotifyOthers> chf = new ChannelFactory<INotifyOthers>(new NetTcpBinding(), string.Format(@"net.tcp://{0}/Notify", adresa));
                            proxy = chf.CreateChannel();

                            if(name == null)
                            {
                                proxy.Notify("Neispravan id");
                            }
                            else
                            {
                                proxy.Notify(name);
                            }


                        }
                    }
   
                    queue.DeleteMessage(message);
                    Trace.TraceInformation(String.Format("Poruka procesuirana: {0}", message.AsString), "Information");
                }
                Thread.Sleep(5000);
                Trace.TraceInformation("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            string port = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["InternalEndpoint"].IPEndpoint.ToString();
            svc.AddServiceEndpoint(typeof(INotifyOthers), new NetTcpBinding(), string.Format(@"net.tcp://{0}/Notify", port));
            svc.Open();

            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;
            Artikal a1 = new Artikal("123");
            a1.Name = "Jagode";
            Artikal a2 = new Artikal("456");
            a2.Name = "Hleb";
            Artikal a3 = new Artikal("789");
            a3.Name = "Mleko";

            
            th.AddElement(a1);
            th.AddElement(a2);
            th.AddElement(a3);
            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("ItemRepositoryWorker has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("ItemRepositoryWorker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();
            svc.Close();
            Trace.TraceInformation("ItemRepositoryWorker has stopped");
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
