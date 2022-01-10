using Common;
using MessageData;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class BratskaKonekcijaProvider : IBratskaKonekcija
    {
        public void Obavestenje(string vrsta, int kolicina, bool uspesno)
        {
            CloudQueue queue = QueueHelper.GetQueueReference("queue2");
            string uniqueBlobName = string.Format(DateTime.Now.ToString());
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobStorage.GetContainerReference("blob");
            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);

            if (uspesno)
            {
                using (var stream = new MemoryStream(Encoding.Default.GetBytes(vrsta + " " + kolicina)))
                {
                    blob.UploadFromStream(stream);
                }
                Trace.WriteLine(String.Format("Uspesna porudzbina: {0} {1}", vrsta, kolicina));
                queue.AddMessage(new CloudQueueMessage(String.Format("Uspesna porudzbina: {0} {1}", vrsta, kolicina)));
            } else
            {
                Trace.WriteLine(String.Format("Neuspesna porudzbina: {0} {1}", vrsta, kolicina));
                queue.AddMessage(new CloudQueueMessage(String.Format("Neuspesna porudzbina: {0} {1}", vrsta, kolicina)));
            }
        }
    }
}
