using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace JobWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private BrotherConnectionService service = new BrotherConnectionService();

        public override void Run()
        {
            //Trace.TraceInformation("JobWorker is running");

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
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            //Trace.TraceInformation("JobWorker has been started");

            return result;
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
                //Trace.TraceInformation("Working");

                if (RoleEnvironment.CurrentRoleInstance.Id.Contains("IN_0"))    //provera da li je prva
                {
                    QueueHelper helper = new QueueHelper("queue");
                    var message = helper.DequeueMessage();

                    if (message != null)
                    {
                        Trace.TraceInformation("Stigla poruka: poruka = " + message.AsString);

                        if (message.AsString == "otvori")
                        {
                            if (service.IsOpened() == false)
                            {
                                service.Open();
                            }
                            else
                            {
                                Trace.TraceInformation("vec je upaljen");
                            }
                        }
                        else
                        {
                            if (service.IsOpened())
                            {
                                service.Close();
                            }
                            else
                            {
                                Trace.TraceInformation("vec je ugasen");
                            }
                        }

                        helper.DeleteMessage(message);
                    }

                }

                else if (RoleEnvironment.CurrentRoleInstance.Id.Contains("IN_1"))
                {
                    try
                    {
                        ChannelFactory<IBrotherConnection> factory = new ChannelFactory<IBrotherConnection>();
                        NetTcpBinding binding = new NetTcpBinding();
                        string addr="";
                        int port=0;
                        foreach (RoleInstance instanca in RoleEnvironment.Roles["JobWorker"].Instances)
                        {
                            if(instanca.Id.Contains("IN_0"))
                            {
                                addr = instanca.InstanceEndpoints["InternalEndpoint"].IPEndpoint.Address.ToString();
                                port = instanca.InstanceEndpoints["InternalEndpoint"].IPEndpoint.Port;
                            }
                        }
                        EndpointAddress address = new EndpointAddress($"net.tcp://{addr}:{port}/Brother");
                        factory = new ChannelFactory<IBrotherConnection>(binding, address);
                        IBrotherConnection proxy = factory.CreateChannel();

                        proxy.AreYouAlive();
                        Trace.TraceInformation("Ziv je burazer, umro nije");
                    }
                    catch
                    {
                        Trace.TraceInformation("Nije ziv. Plaky");
                    }

                }
                await Task.Delay(1000);
            }
        }
    }
}
