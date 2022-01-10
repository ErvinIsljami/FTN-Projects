using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class BlobHelper
    {
        public static CloudBlockBlob GetBlobReference(string containerName, string blobName)
        {
            //var acc = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = acc.CreateCloudBlobClient();
            var container = client.GetContainerReference(containerName);
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(blobName);
            return blob;
        }
    }
}
