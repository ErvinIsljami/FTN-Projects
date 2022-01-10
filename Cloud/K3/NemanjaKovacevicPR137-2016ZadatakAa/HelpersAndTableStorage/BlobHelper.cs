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

namespace HelpersAndTableStorage
{
    public class BlobHelper
    {
        public static void InitBlob(string conectionString, string blobName)
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(conectionString));

                CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobStorage.GetContainerReference(blobName);
                container.CreateIfNotExists();
                
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }
            catch (WebException)
            {

            }
        }

        public static CloudBlockBlob PristupiBlobElementu(string conectionString, string blobName, string uniqueBlobName)
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(conectionString));
            CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobStorage.GetContainerReference(blobName);
            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);

            return blob;
        }

        public static void ZapisiUBlob(CloudBlockBlob blob, string zapis)
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(zapis)))
            {
                blob.UploadFromStream(stream);
            }

        }

        public static string CitajIzBoba(CloudBlockBlob blob)
        {
            string text = "";
            using (var stream = new MemoryStream())
            {
                blob.DownloadToStream(stream);
                text = Encoding.Default.GetString(stream.ToArray());
            }
            
            return text;
        }
    }
}
