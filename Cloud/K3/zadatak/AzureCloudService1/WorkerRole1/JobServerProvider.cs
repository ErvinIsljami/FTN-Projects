using Contracts;
using Microsoft.WindowsAzure.Storage.Queue;
using Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{
    public class JobServerProvider : IBrother
    {
        public void Posalji(string vrstaHrane, string kolicina, bool uspjesno)
        {
            Trace.WriteLine("Stigli podaci od bratske instance");
            BlobHelper bh = new BlobHelper("blob1", "hrana");
            if (uspjesno)
            {
                bh.UploadToBlob(vrstaHrane + "_" + kolicina);
                Trace.WriteLine("Dodato u blob");
            }
            QueueHelper qh = new QueueHelper("red2");
            CloudQueueMessage message = new CloudQueueMessage(vrstaHrane + "_" + kolicina);

            qh.queue.AddMessage(message);
            Trace.WriteLine("Dodato u red2");
           
        }
    }
}
