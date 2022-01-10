
using Contracts;
using HelpersAndTableStorage;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{
    public class FirstServiceProvider : IFirstService
    {
        private static int uniqueBlobName;

        static FirstServiceProvider()
        {
            uniqueBlobName = 0;
        }

        public void ProslediBratskojInstanci(string vrstaRobe, int kolicina, bool uspesnost)
        {
            Trace.WriteLine(vrstaRobe + "  " + kolicina + "  " + uspesnost);

            if(uspesnost)
            {
                var blob = BlobHelper.PristupiBlobElementu("DataConnectionString6", "hranablob", uniqueBlobName.ToString());
                BlobHelper.ZapisiUBlob(blob, "Vrsta robe : " + vrstaRobe + "Kolicina : " + kolicina);
                uniqueBlobName++;
            }

            var queue = QueueHelper.InitQueue("DataConnectionString6", "hranaqueuenazad");
            queue.AddMessage(new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage(vrstaRobe + " " + kolicina + " " + uspesnost));
        }
    }
}
