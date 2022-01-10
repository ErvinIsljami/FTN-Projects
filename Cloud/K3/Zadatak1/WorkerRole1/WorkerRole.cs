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
using Helpers;
using Common;
using Microsoft.WindowsAzure.Storage.Table;
using System.ServiceModel;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private Server server = new Server(typeof(Uspesnost), typeof(IUspesnost));

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

            int iD = int.Parse(RoleEnvironment.CurrentRoleInstance.Id.Split('_').Last());
            if (iD == 1)
            {
                server.Open();
            }


            Trace.TraceInformation("WorkerRole1 has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            int iD = int.Parse(RoleEnvironment.CurrentRoleInstance.Id.Split('_').Last());
            if (iD == 1)
            {
                server.Close();
            }

            Trace.TraceInformation("WorkerRole1 has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            var queue = QueueHelper.GetQueueReference("red");
            var queue2 = QueueHelper.GetQueueReference("red2");
            var table = TableHelper.GetTableReference("tabela");

            queue.Clear();

            // bilo za dodavanje elemenata u tabelu
            //int id = int.Parse(RoleEnvironment.CurrentRoleInstance.Id.Split('_').Last());
            //if (id == 0)
            //{
            //    for (int i = 0; i < 5; i++)
            //    {
            //        TableOperation o = TableOperation.Insert(new Porudzbina($"AAA{i * 10}", (i + 1) * 100));
            //        table.Execute(o);
            //    }
            //}

            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                int iD = int.Parse(RoleEnvironment.CurrentRoleInstance.Id.Split('_').Last());
                if (iD == 0)
                {
                    var message = queue.GetMessage();
                    if (message == null)
                    {
                        continue;
                    }

                    Trace.WriteLine(message.AsString);

                    Porudzbina p = new Porudzbina(message.AsString.Split('~').First(), int.Parse(message.AsString.Split('~').Last()));
                    TableOperation operation = TableOperation.Retrieve(p.PartitionKey, p.RowKey);
                    TableResult result = table.Execute(operation);
                    
                    if (result.Result == null)
                    {
                        Trace.WriteLine("result je null");
                        queue.DeleteMessage(message);
                        continue;
                    }

                    bool uspelo = true;
                    Porudzbina pp = null;
                    var value = (result.Result as DynamicTableEntity).Properties["Amount"].Int32Value;
                    if (value >= p.Amount)
                    {
                        int newVal = value.Value - p.Amount;
                        pp = new Porudzbina(message.AsString.Split('~').First(), newVal);
                        TableOperation o = TableOperation.InsertOrReplace(pp);
                        table.Execute(o);
                        Trace.WriteLine(p.Amount);
                    }
                    else
                    {
                        uspelo = false;
                    }

                    ChannelFactory<IUspesnost> factory = new ChannelFactory<IUspesnost>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{RoleEnvironment.Roles["WorkerRole1"].Instances[1].InstanceEndpoints["Internal"].IPEndpoint}/Internal"));
                    IUspesnost proxy = factory.CreateChannel();

                    proxy.Obavestenje(pp, uspelo);

                    queue2.AddMessage(new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage($"{p.Type}{p.Amount}~{uspelo}"));

                    queue.DeleteMessage(message);
                }

                await Task.Delay(10);
            }
        }
    }
}
