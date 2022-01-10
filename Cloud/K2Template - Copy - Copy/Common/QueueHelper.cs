using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class QueueHelper
    {
        private CloudStorageAccount _storageAccount;
        private CloudQueue _queue;
        public QueueHelper(string queueName)
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var queueClient = _storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(queueName);
        }

        public void EnqueueMessage(string messageTekst)
        {
            CloudQueueMessage message = new CloudQueueMessage(messageTekst);
            //TimeSpan(sati, minuti, sekunde)
            //_queue.AddMessage(message, new TimeSpan(0, 2, 0), new TimeSpan(0, 0, 2)); //oktomentarisati ako treba podesiti trajanje poruke i delay
            //_queue.AddMessage(message, null, new TimeSpan(0, 0, 2));   //otkomentarisati ako treba podesiti delay
            //_queue.AddMessage(message, new TimeSpan(0, 2, 0), null);   //otkomentarisati ako treba podesiti trajanje poruke
            _queue.AddMessage(message);
        }

        public CloudQueueMessage DequeueMessage()
        {
            return _queue.GetMessage();
        }

        public void DeleteMessage(CloudQueueMessage message)
        {
            _queue.DeleteMessage(message);
        }
    }
}
