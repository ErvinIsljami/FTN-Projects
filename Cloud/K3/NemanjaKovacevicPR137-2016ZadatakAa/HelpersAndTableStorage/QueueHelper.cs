
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpersAndTableStorage
{
    public class QueueHelper
    {
        public static CloudQueue InitQueue(string connectionString, string queueName)
        {
            var _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(connectionString));
            CloudQueueClient queueClient = new CloudQueueClient(new Uri(_storageAccount.QueueEndpoint.AbsoluteUri), _storageAccount.Credentials);
            var queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();

            return queue;
        }
    }
}
