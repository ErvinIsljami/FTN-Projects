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
using Data;
using Microsoft.WindowsAzure.Storage.Queue;
using System.ServiceModel;

namespace WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        JobServer server = new JobServer();
        TableHelper tableHelper;
        QueueHelper queueHelper;
        CloudQueue queue;

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole is running");
            queueHelper = new QueueHelper();
            tableHelper = new TableHelper();

            queue = queueHelper.GetQueueReference("zahtevi");
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
            server.Open();

            Trace.TraceInformation("WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();
            server.Close();

            Trace.TraceInformation("WorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                if (Int32.Parse(RoleEnvironment.CurrentRoleInstance.Id.Split('_').Last()) == 0)
                {
                    CloudQueueMessage poruka = queue.GetMessage();

                    if (poruka == null)
                    {
                        //Trace.WriteLine("Nema poruka u redu.");
                    }
                    else
                    {
                        string[] split = poruka.AsString.Split('#');
                        Trace.WriteLine("Porudzbina: " + split[0] + " - kolicina: " + split[1]);

                        List<Hrana> lista = tableHelper.RetrieveAll().ToList();

                        bool uspesno = false;
                        Hrana trazena = null;

                        foreach (Hrana h in lista)
                        {
                            if (h.RowKey.Equals(split[0]))
                            {
                                trazena = h;
                                if (h.Kolicina >= Int32.Parse(split[1]))
                                {
                                    // moguce je obaviti poruzdbinu
                                    h.Kolicina -= Int32.Parse(split[1]);
                                    tableHelper.Update(h);
                                    Trace.WriteLine("Poruzdbina uspesna");
                                    uspesno = true;
                                }
                            }
                        }
                        if (!uspesno)
                        {
                            Trace.WriteLine("Porudzbina neuspesna");
                        }

                        if (trazena != null)
                        {
                            trazena.Kolicina = Int32.Parse(split[1]);
                        }
                        else
                        {
                            trazena = new Hrana(split[0]);
                            trazena.Kolicina = Int32.Parse(split[1]);
                        }

                        INotify proxy = Connect();
                        proxy.Notify(trazena, uspesno);

                        //Kada je poruka procitana brise se iz reda
                        queue.DeleteMessage(poruka);
                    }
                }
                await Task.Delay(1000);
            }
        }

        private INotify Connect()
        {
            NetTcpBinding binding = new NetTcpBinding();
            RoleInstanceEndpoint endpoint = RoleEnvironment.Roles["WorkerRole"].Instances.Where(i => i.Id.Split('_').Last().Equals("1")).First().InstanceEndpoints["InternalRequest"];
            string remoteAddress = $"net.tcp://{endpoint.IPEndpoint}/InternalRequest";
            ChannelFactory<INotify> factory = new ChannelFactory<INotify>(binding, remoteAddress);
            return factory.CreateChannel();
        }

    }
}
