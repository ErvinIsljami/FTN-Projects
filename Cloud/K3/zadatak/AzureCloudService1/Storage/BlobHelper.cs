using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public class BlobHelper
    {
        private CloudStorageAccount storageAccount;
        public CloudBlockBlob blob;

        public BlobHelper(String containerName, String blobName)
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudBlobClient blobCLient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobCLient.GetContainerReference(containerName);
            blobContainer.CreateIfNotExists();

            var permissions = blobContainer.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            blobContainer.SetPermissions(permissions);

            blob = blobContainer.GetBlockBlobReference(blobName);
        }

        public String DownloadFromBlob()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                blob.DownloadToStream(ms);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }

        public void UploadToBlob(String text)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(text)))
            {
                blob.UploadFromStream(ms);
            }
        }
    }
}
