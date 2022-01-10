﻿using Microsoft.WindowsAzure;
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
        public static CloudQueue GetQueueReference(String queueName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse
            (CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue messageQueue = queueClient.GetQueueReference(queueName);
            messageQueue.CreateIfNotExists();
            return messageQueue;
        }
    }
}
