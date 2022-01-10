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
using Common;
using Microsoft.WindowsAzure.Storage.Queue;
using System.ServiceModel;

namespace WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        TableHelper tableHelper = new TableHelper();
        QueueHelper queueHelper = new QueueHelper();
        ObavestiBrataHost bratHost = new ObavestiBrataHost();

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole is running");

            while (true)
            {
                if(RoleEnvironment.CurrentRoleInstance.Id.Split('_')[2] == "0")
                {
                    CloudQueue queue = queueHelper.GetQueueReference("redporuka");
                    CloudQueueMessage message = queue.GetMessage();

                    if(message != null)
                    {
                        //preuzimanje poruke iz reda
                        string vrsta = message.AsString.Split(' ')[0];
                        string kolicina = message.AsString.Split(' ')[1];
                        int pomKol = Int32.Parse(kolicina);
                        Roba robaPom = new Roba(vrsta) { Vrsta = vrsta, Kolicina = pomKol };

                        //preuzimanje robe iz tabele
                        Roba roba = tableHelper.PreuzmiRobu(vrsta);

                        if(roba.Vrsta == " ")
                        {
                            Trace.WriteLine("Ne postoji uneta roba u tabeli.");
                            Connect();
                            proxy.ObavestiSvogBrata(robaPom, "neuspesno");

                        }
                        else
                        {
                            if(roba.Kolicina > pomKol)
                            {
                                //umanjujemo vrednost i pamtim u tabeli
                                int novaVrednost = roba.Kolicina - pomKol;
                                Roba r = new Roba(vrsta) { Vrsta = vrsta, Kolicina = novaVrednost };
                                tableHelper.DodajRobu(r);
                                Trace.WriteLine("Nova vrednost je azurirana.");
                                Connect();
                                proxy.ObavestiSvogBrata(r, "uspesno");

                            }
                            else
                            {
                                Roba r = new Roba(vrsta) { Vrsta = vrsta, Kolicina = roba.Kolicina };
                                Trace.WriteLine("Nema dovoljno vrednosti na lageru.");
                                Connect();
                                proxy.ObavestiSvogBrata(r, "neuspesno");
                            }
                        }
                        queue.DeleteMessage(message);

                    }
                  else
                  {
                        Trace.WriteLine("TRENUTNO NEMA PORUKE U REDU!");
                  }


                }

                Thread.Sleep(5000);
            }
            
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();
            InicijalizacijaTabele();
            bratHost.Open();
            Trace.TraceInformation("WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            bratHost.Close();
            base.OnStop();

            Trace.TraceInformation("WorkerRole has stopped");
        }

        public void InicijalizacijaTabele()
        {
            Roba r1 = new Roba("margarin") { Vrsta = "margarin", Kolicina = 5 };
            Roba r2 = new Roba("hleb") { Vrsta = "hleb", Kolicina = 3 };
            Roba r3 = new Roba("paprika") { Vrsta = "paprika", Kolicina = 4 };
            tableHelper.DodajRobu(r1);
            tableHelper.DodajRobu(r2);
            tableHelper.DodajRobu(r3);
        }

        public static IObavesti proxy;
        public void Connect()
        {
            int brojInstanci = RoleEnvironment.Roles["WorkerRole"].Instances.Count();
            for (int i = 0; i < brojInstanci; i++)
            {
                if (RoleEnvironment.Roles["WorkerRole"].Instances[i].Id.Split('_')[2] == "0")
                {
                    NetTcpBinding binding = new NetTcpBinding();
                    RoleInstanceEndpoint internalEndpoint = RoleEnvironment.Roles["WorkerRole"].Instances[i].InstanceEndpoints["InternalRequest"];
                    string endpoint = $"net.tcp://{internalEndpoint.IPEndpoint}/InternalRequest";
                    ChannelFactory<IObavesti> factory = new ChannelFactory<IObavesti>(binding, new EndpointAddress(endpoint));
                    proxy = factory.CreateChannel();
                }
            }
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
