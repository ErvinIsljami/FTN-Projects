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
using Microsoft.WindowsAzure.Storage.Queue;
using MessageData;
using System.ServiceModel;
using Common;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        BratskaKonekcijaServer bratskaKonekcijaServer = new BratskaKonekcijaServer();

        public void InitBlobs()
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobStorage.GetContainerReference("blob");
                container.CreateIfNotExists();
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }
            catch (WebException)
            {
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();
            if (int.Parse(RoleEnvironment.CurrentRoleInstance.Id.Split('_')[2]) == 1)
            {
                bratskaKonekcijaServer.Open();
                InitBlobs();
            }

            Trace.TraceInformation("WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();
            if (int.Parse(RoleEnvironment.CurrentRoleInstance.Id.Split('_')[2]) == 1)
            {
                bratskaKonekcijaServer.Close();
            }

            Trace.TraceInformation("WorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            RobaRepository robaRepository = new RobaRepository();
            CloudQueue queue = QueueHelper.GetQueueReference("queue1");

            while (!cancellationToken.IsCancellationRequested)
            {
                if (int.Parse(RoleEnvironment.CurrentRoleInstance.Id.Split('_')[2]) == 0)
                {
                    CloudQueueMessage retrievedMessage = queue.GetMessage();
                    List<RoleInstance> lista = RoleEnvironment.Roles[RoleEnvironment.CurrentRoleInstance.Role.Name].Instances.ToList();
                    RoleInstance next = null;

                    foreach (RoleInstance ri in lista)
                    {
                        if (int.Parse(ri.Id.Split('_')[2]) == 1)
                        {
                            next = ri;
                            break;
                        }
                    }
                    string endpointName = "Internal";
                    ChannelFactory<IBratskaKonekcija> cf = new ChannelFactory<IBratskaKonekcija>(new NetTcpBinding(), new EndpointAddress(String.Format("net.tcp://{0}/{1}", next.InstanceEndpoints[endpointName].IPEndpoint.ToString(), endpointName)));
                    IBratskaKonekcija proxy = cf.CreateChannel();

                    if (retrievedMessage == null)
                    {
                        Trace.TraceInformation("Nema porudzbina");
                    }
                    else
                    {
                        Trace.TraceInformation(retrievedMessage.AsString);
                        string[] delovi = retrievedMessage.AsString.Split(' '); //0 - vrsta; 1 - kolicina
                        int kolicina = int.Parse(delovi[1]);
                        Roba r = robaRepository.PreuzmiKonkretnuRobu(delovi[0]);

                        if (r != null && r.Stanje >= kolicina)
                        {
                            r.Stanje -= kolicina;
                            robaRepository.AzurirajRobu(r);
                            
                            proxy.Obavestenje(delovi[0], kolicina, true);
                        } else
                        {
                            proxy.Obavestenje(delovi[0], kolicina, false);
                        }
                        queue.DeleteMessage(retrievedMessage);
                    }
                }
                
                await Task.Delay(1000);
            }
        }
    }
}
