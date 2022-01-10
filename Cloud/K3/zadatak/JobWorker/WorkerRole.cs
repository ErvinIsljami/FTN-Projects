using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        JobServer jobServer = new JobServer();

        QueueHelper queueHelper = new QueueHelper();
        BlobHelper blobHelper = new BlobHelper();
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
            // svaki put kad se pokrene workerRole, stanje servisa ce biti zatvoreno


            Trace.TraceInformation("JobWorker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                CloudQueue queue = QueueHelper.GetQueueReference("Porudzbinaqueue");
                CloudQueueMessage retrievedMessage = queue.GetMessage();
                if (retrievedMessage == null)
                {
                    Trace.TraceInformation("No message");
                }
                else
                {
                    Trace.TraceInformation(retrievedMessage.AsString);
                    queue.DeleteMessage(retrievedMessage);
                }
                await Task.Delay(5000);
            }
        }
    }
}
