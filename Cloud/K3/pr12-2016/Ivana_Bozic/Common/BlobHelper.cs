using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class BlobHelper
    {
        CloudBlobClient blobStorage;
        public BlobHelper()
        {
            InitBlobs();
        }
        public void InitBlobs()
        {
            try
            {    // read account configuration settings       
                var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                // create blob container for images           
                blobStorage = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobStorage.GetContainerReference("blobime");
                container.CreateIfNotExists();
                // configure container for public access              
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }
            catch (WebException)
            {

            }


        }
        public CloudBlockBlob GetBlobRef(int id)
        {             // kreiranje blob sadrzaja i kreiranje blob klirepo.RetrieveAllStudents()jenta 
            string uniqueBlobName = string.Format(id.ToString()); //u zavisnosti od teksta zadatka treba promeniti nacin cuvanja u container 
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            blobStorage = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobStorage.GetContainerReference("blobime");
            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
            return blob;
        }

        public void UploadToStream(CloudBlockBlob blob, string text)
        { using (var stream = new MemoryStream(Encoding.Default.GetBytes(text)))
            { blob.UploadFromStream(stream);
            }
        }
        public string DownloadToStream(CloudBlockBlob blob)
        {
            string text;
            using (var stream = new MemoryStream()) { blob.DownloadToStream(stream); text = Encoding.Default.GetString(stream.ToArray()); }
            return text;
        }
    }


}

