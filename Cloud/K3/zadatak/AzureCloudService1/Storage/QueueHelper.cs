using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public class QueueHelper
    {
        private CloudStorageAccount storageAccount;
        public CloudQueue queue;

        public QueueHelper(String queueName)
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudQueueClient queueClient = new CloudQueueClient
                (
                new Uri(storageAccount.QueueEndpoint.AbsoluteUri),
                storageAccount.Credentials
                );
            queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();
        }
    }
}
