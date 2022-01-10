using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;
using System.Net;
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
            Trace.TraceInformation("JobWorker is running");

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

            Trace.TraceInformation("JobWorker has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("JobWorker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            if (service.IsOpen())
            {
                service.Close();
            }

            Trace.TraceInformation("JobWorker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");



                
                //if (RoleEnvironment.CurrentRoleInstance.Id.Contains("0"))
                //{
                //    QueueHelper queueHelper = new QueueHelper("queue");
                //    var message = queueHelper.DequeueMessage();

                //    if (message != null)
                //    {

                //        if (message.DequeueCount == 10)
                //        {
                //            queueHelper.DeleteMessage(message);
                //        } //proverite dequeueCount koliko je puta skinuta da bi je mozda brisali(ako je previse puta skidana sa reda a niko je nije obrisao)


                //        if (message.AsString == "otvori")
                //        {
                //            if (service.IsOpen())
                //            {
                //                Trace.TraceInformation("Vec je otvoren");
                //                queueHelper.DeleteMessage(message);
                //            }
                //            else
                //            {
                //                service.Open();
                //                Trace.TraceInformation("Service otvoren...");
                //                queueHelper.DeleteMessage(message);
                //            }
                //        }
                //        else if (message.AsString == "zatvori")
                //        {
                //            if (service.IsOpen())
                //            {
                //                service.Close();
                //                Trace.TraceInformation("Service zatvoren...");
                //                queueHelper.DeleteMessage(message);
                //            }
                //            else
                //            {
                //                Trace.TraceInformation("Service vec zatvoren...");
                //                queueHelper.DeleteMessage(message);
                //            }
                //        }
                //    }

                //}






                await Task.Delay(100);
            }
        }
    }
}
