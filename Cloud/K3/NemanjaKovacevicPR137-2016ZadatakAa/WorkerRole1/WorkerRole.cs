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
using HelpersAndTableStorage;
using System.ServiceModel;
using Contracts;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        FirstService fs = new FirstService();
        DataRepository repo;
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
            fs.Open();
            repo = new DataRepository("tabelaHrana");

            /*repo.Add(new Hrana("v1", 1));
            repo.Add(new Hrana("v2", 2));
            repo.Add(new Hrana("v3", 3));*/
            Trace.TraceInformation("WorkerRole1 has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            var queue = QueueHelper.InitQueue("DataConnectionString6", "hranaqueue");

            while (!cancellationToken.IsCancellationRequested)
            {
                string id = RoleEnvironment.CurrentRoleInstance.Id;
                if (id[id.Length - 1] == '0')
                {
                    var msg = queue.GetMessage();

                    if (msg != null)
                    {
                        Trace.WriteLine("Poruka : " + msg.AsString);

                        string[] parts = msg.AsString.Split(' ');
                        Hrana roba = new Hrana(parts[0], int.Parse(parts[1]));
                        Hrana izTabele = ZeljenaRoba(roba.Vrsta);

                        ChannelFactory<IFirstService> channel = new ChannelFactory<IFirstService>(new NetTcpBinding(), new EndpointAddress(
                        String.Format("net.tcp://{0}/Internal", RoleEnvironment.Roles["WorkerRole1"].Instances[1].InstanceEndpoints["Internal"].IPEndpoint.ToString())));
                        IFirstService proxy = channel.CreateChannel();

                        if (izTabele == null)
                        {
                            Trace.WriteLine("Zeljeni proizvod ne postoji.");
                            proxy.ProslediBratskojInstanci(roba.Vrsta, roba.Kolicina, false);

                        }
                        else
                        {
                            if (izTabele.Kolicina - roba.Kolicina >= 0)
                            {
                                izTabele.Kolicina -= roba.Kolicina;
                                repo.Modify(izTabele);
                                proxy.ProslediBratskojInstanci(roba.Vrsta, roba.Kolicina, true);
                            }
                            else
                            {
                                proxy.ProslediBratskojInstanci(roba.Vrsta, roba.Kolicina, false);
                            }
                        }

                        queue.DeleteMessage(msg);
                    }
                }
                Trace.TraceInformation("Working");
                await Task.Delay(10000);
            }
        }

        private Hrana ZeljenaRoba(string vrsta)
        {

            var roba = repo.RetrieveAllStudents();
            Hrana ret = null;
            foreach (var item in roba)
            {
                Trace.WriteLine(item.Vrsta + " " + item.Kolicina);
                if (item.Vrsta.ToLower() == vrsta.ToLower())
                {
                    ret = item;
                    break;
                }

            }

            return ret;
        }
    }
}
