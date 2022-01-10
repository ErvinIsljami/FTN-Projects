using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.ServiceModel;
using Contracts;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        JobServer js = new JobServer();
        public override void Run()
        {
            Trace.TraceInformation("WorkerRole1 is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();
            js.Open();
            Trace.TraceInformation("WorkerRole1 has been started");
            
            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();
            
            base.OnStop();
            js.Close();
            Trace.TraceInformation("WorkerRole1 has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            String idInstance = RoleEnvironment.CurrentRoleInstance.Id.Split('_')[2];
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                if (idInstance == "0")
                {
                    QueueHelper queueHelper = new QueueHelper("red1");
                    TableHelper tableHelper = new TableHelper("tabela1");

                    string poruka = queueHelper.queue.GetMessage().AsString;
                    var splitovanaPoruka = poruka.Split('_');
                    var entitet = tableHelper.GetEntity(splitovanaPoruka[0]);
                    
                    if (entitet != null)
                    {
                        if (Int32.Parse(entitet.Kolicina) >= Int32.Parse(splitovanaPoruka[1]))
                        {
                            Trace.WriteLine("Moguce je poruciti!");
                            string novaKolicina = (Int32.Parse(entitet.Kolicina) - Int32.Parse(splitovanaPoruka[1])).ToString();
                            tableHelper.UpdateEntitet(new Entitet(splitovanaPoruka[0], novaKolicina));

                            
                            NetTcpBinding binding = new NetTcpBinding();


                            List<EndpointAddress> internalEndpoints =
                                RoleEnvironment.Roles[RoleEnvironment.CurrentRoleInstance.Role.Name].Instances.Where(
                                instance => instance.Id != RoleEnvironment.CurrentRoleInstance.Id).Select(process => new
                                EndpointAddress(String.Format("net.tcp://{0}/{1}",
                                process.InstanceEndpoints["Internal"].IPEndpoint.ToString(),
                                "Internal"))).ToList();

                            IBrother proxy;
                            proxy = new ChannelFactory<IBrother>(binding, internalEndpoints[0]).CreateChannel();
                             proxy.Posalji(entitet.RowKey, novaKolicina, true);
                            Trace.WriteLine("Podaci su poslati bratskoj instanci");
                            

                        }
                      }
                        else
                    {
                        NetTcpBinding binding = new NetTcpBinding();


                        List<EndpointAddress> internalEndpoints =
                            RoleEnvironment.Roles[RoleEnvironment.CurrentRoleInstance.Role.Name].Instances.Where(
                            instance => instance.Id != RoleEnvironment.CurrentRoleInstance.Id).Select(process => new
                            EndpointAddress(String.Format("net.tcp://{0}/{1}",
                            process.InstanceEndpoints["Internal"].IPEndpoint.ToString(),
                            "Internal"))).ToList();

                        IBrother proxy;
                        proxy = new ChannelFactory<IBrother>(binding, internalEndpoints[0]).CreateChannel();
                        proxy.Posalji("", "", false);
                        
                        Trace.WriteLine("Podaci su poslati bratskoj instanci");                 
                    }
                    await Task.Delay(5000);
                }
                else if (idInstance == "1")
                {

                    await Task.Delay(5000);

                }
               

                Trace.TraceInformation("Working");
                
            }
        }
    }
}
