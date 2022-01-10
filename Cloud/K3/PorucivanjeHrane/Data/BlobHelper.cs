using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class BlobHelper
    {
        CloudStorageAccount _storage = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
        CloudBlobClient _blobStorageClient;
        CloudBlobContainer _container;

        public BlobHelper()
        {
            _blobStorageClient = _storage.CreateCloudBlobClient();
            _container = _blobStorageClient.GetContainerReference("hranablob");
            _container.CreateIfNotExists();

            BlobContainerPermissions permissions = _container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            _container.SetPermissions(permissions);
        }

        public CloudBlockBlob GetBlobRef(int id)
        {
            string uniqueBlobName = string.Format(id.ToString()); 

            CloudBlockBlob blob = null;
            try
            {
                blob = _container.GetBlockBlobReference(uniqueBlobName);
            }
            catch { }

            return blob;
        }

        public void UploadToStream(CloudBlockBlob blob, string text)
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(text)))
            {
                blob.UploadFromStream(stream);
            }
        }

        public string DownloadToStream(CloudBlockBlob blob)
        {
            string text;

            using (var stream = new MemoryStream())
            {
                blob.DownloadToStream(stream);
                text = Encoding.Default.GetString(stream.ToArray());
            }

            return text;
        }

    }
}
