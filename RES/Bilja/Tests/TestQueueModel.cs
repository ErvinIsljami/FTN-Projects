using NUnit.Framework;
using QueueService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class TestQueueModel
    {
        [TestCase("test")]
        [TestCase("clientQueue")]
        public void TestModelForQueues(string queueNames)
        {
            QueueModel queueModel = new QueueModel(queueNames);
            Assert.IsNotNull(queueModel);
            Assert.IsNotNull(queueModel.QueueA);
            Assert.IsNotNull(queueModel.QueueB);
            Assert.AreEqual(queueModel.QueueName, queueNames);
        }
    }
}
