using Common;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class ObavestiBrata : IObavesti
    {
        static int counter = 0;
        public void ObavestiSvogBrata(Roba r, string uspesno)
        {
            QueueHelper queueHelper = new QueueHelper();
            BlobHelper blobHelper = new BlobHelper();

           

            if(uspesno == "uspesno")
            {
                Trace.WriteLine("USPESNO OBAVESTEN BRAT");
                CloudBlockBlob blob = blobHelper.GetBlobRef(counter);
                blobHelper.UploadToStream(blob, r.ToString());

                counter++;
                CloudQueue queue = queueHelper.GetQueueReference("redpovratak");
                queue.AddMessage(new CloudQueueMessage(r.ToString()));

            }
            else
            {
                Trace.WriteLine("NEUSPESNO OBAVESTEN BRAT");
                CloudQueue queue = queueHelper.GetQueueReference("redpovratak");
                queue.AddMessage(new CloudQueueMessage(r.ToString()));
            }
        }
    }
}
