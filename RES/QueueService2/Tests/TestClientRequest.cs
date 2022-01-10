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
    }
}
