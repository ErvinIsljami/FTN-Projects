using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class BlobHelper
    {
        CloudStorageAccount storageAccount =
        CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
        static CloudBlobClient blobStorage;
        static bool initilized = false;
        public BlobHelper()
        {
            blobStorage = storageAccount.CreateCloudBlobClient();
        }

        public static string GetItem(String containerName, String blobName)
        {
            if (!initilized)
                InitBlobs();

            CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
            using (MemoryStream ms = new MemoryStream())
            {
                blob.DownloadToStream(ms);

                return Encoding.Default.GetString(ms.ToArray());
            }
        }
        
        public static void InsertItem(string item, String containerName, String blobName)
        {
            if (!initilized)
                InitBlobs();

            CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
            using (MemoryStream memoryStream = new MemoryStream(Encoding.Default.GetBytes("text")))
            {
                blob.UploadFromStream(memoryStream);
                memoryStream.Position = 0;
                blob.Properties.ContentType = item;
                blob.UploadFromStream(memoryStream);
            }
        }
        
        public static void InitBlobs()
        {
            try
            {
                // read account configuration settings
                var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

                // create blob container for images
                blobStorage = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobStorage.GetContainerReference("komande");
                container.CreateIfNotExists();
                // configure container for public access
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
                initilized = true;
            }
            catch (WebException w)
            {
                Trace.TraceInformation("Blob error: " + w.Message);
            }
        }
    }
}
