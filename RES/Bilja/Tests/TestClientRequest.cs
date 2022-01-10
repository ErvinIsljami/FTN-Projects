using QueueService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueueService.User;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TestClientRequest
    {
        [Test]
        public void TestClientRequestADD()
        {
            ClientRequestCreate crq = new ClientRequestCreate("queue1", "asodfjiasodf");

            Assert.IsNotNull(crq);
            Assert.AreEqual(crq.Type, ERequestType.CREATE);
          
        }
        [Test]
        public void TestClientRequestSubscribe()
        {
            ClientRequestSubscribe crq = new ClientRequestSubscribe("queue1", "asodfjiasodf");

            Assert.IsNotNull(crq);
            Assert.AreEqual(crq.Type, ERequestType.SUBSCRIBE);
        }
        [Test]
        public void TestClientRequestUpdate()
        {
            DataModel dataModel = new DataModel();
            ClientRequestUpdate crq = new ClientRequestUpdate("user1", dataModel);
            Assert.IsNotNull(crq);
            Assert.AreEqual(crq.DataModel, dataModel);
        }

        
    }
}
