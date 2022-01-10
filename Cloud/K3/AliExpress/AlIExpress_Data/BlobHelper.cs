using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace AlIExpress_Data
{
    public class BlobHelper
    {
        public static void InitBlobs(string blobName)
        {
            try
            {
                // read account configuration settings
                var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

                // create blob container for images
                CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobStorage.GetContainerReference(blobName);
                container.CreateIfNotExists();
                // configure container for public access
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }
            catch (WebException e)
            {
                Trace.TraceInformation("Doslo je do greske: " + e.Message);
            }
        }
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _blobContainer;
        private CloudBlockBlob _blockBlob;
        public BlobHelper(string blobName)
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));            _blobClient = _storageAccount.CreateCloudBlobClient();
            _blobContainer = _blobClient.GetContainerReference(blobName);
        }

        public CloudBlockBlob GetBlockBlobReference(string uniqueBlobName)
        {
            return _blobContainer.GetBlockBlobReference(uniqueBlobName);
        }

        public void AddNewBlockBlob(string uniqueBlobName, string file)
        {
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(uniqueBlobName);
            blob.Properties.ContentType = file.GetType().ToString();
            blob.UploadFromByteArray(Encoding.ASCII.GetBytes(file), 0, file.Count());
        }

        public void AddNewBlockBlob(string uniqueBlobName, byte[] file)
        {
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(uniqueBlobName);
            blob.Properties.ContentType = file.GetType().ToString();
            blob.UploadFromByteArray(file, 0, file.Count());
        }
       
    }
}
