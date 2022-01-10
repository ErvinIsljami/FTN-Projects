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
using AlIExpress_Data;

namespace AliExpressWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private JobServer _jobServer;
        private Int32 _instanceId;
        //private BlobHelper _blobHelper;
        private CloudQueue _queue;
        private CloudQueueMessage _recivedMessage;
        private TableHelper _table;
        public override void Run()
        {
            Trace.TraceInformation("AliExpressWorkerRole is running");

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

            _instanceId = Int32.Parse(RoleEnvironment.CurrentRoleInstance.Id.Split('.').Last().Split('_').Last());
            _queue = QueueHelper.GetQueueReference("poruci");

            _jobServer = new JobServer();

            if(_instanceId == 0)
            {
                _jobServer.AddInternalServiceEP();
         
            }

            Trace.TraceInformation("AliExpressWorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("AliExpressWorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("AliExpressWorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);


                if(_instanceId == 0)
                {
                    
                        _recivedMessage = _queue.GetMessage();
                                
                }

            }
        }

      
    }
}
