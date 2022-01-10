using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class BlobHelper
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse
        (CloudConfigurationManager.GetSetting("DataConnectionString"));
        CloudBlobClient blobStorage;
        public BlobHelper()
        {
            blobStorage = storageAccount.CreateCloudBlobClient();
        }
       
    }
}
