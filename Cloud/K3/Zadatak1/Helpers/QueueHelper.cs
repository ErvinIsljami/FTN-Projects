using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class QueueHelper
    {
        /// <param name="queueName">MUST BE ALL LOWERCASE</param>
        /// <returns></returns>
        public static CloudQueue GetQueueReference(string queueName)
        {
            //var acc = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = acc.CreateCloudQueueClient();
            var queue = client.GetQueueReference(queueName);
            queue.CreateIfNotExists();
            return queue;
        }
    }
}
