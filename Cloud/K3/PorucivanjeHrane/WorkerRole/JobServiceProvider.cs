using Data;
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
    public class JobServiceProvider : INotify
    {
        BlobHelper blobHelper = new BlobHelper();
        QueueHelper queueHelper = new QueueHelper();
        
        public static int id = 0;

        public void Notify(Hrana hrana, bool uspesno)
        {
            if (uspesno)
            {
                id++;
                CloudBlockBlob _blob = blobHelper.GetBlobRef(id);
                string poruka = hrana.RowKey + " " + hrana.Kolicina.ToString();
                blobHelper.UploadToStream(_blob, poruka);
            }

            CloudQueue red2 = queueHelper.GetQueueReference("drugired");
            string porukaRed2 = hrana.RowKey + "#" + hrana.Kolicina.ToString();
            red2.AddMessage(new CloudQueueMessage(porukaRed2));

            string pom = "";
            if(uspesno)
            {
                pom = "uspesnp";
            }
            else
            {
                pom = "nauspesno";
            }
            Trace.WriteLine(hrana.RowKey + "#" + hrana.Kolicina.ToString() + " prosla " + pom);
        }
    }
}
