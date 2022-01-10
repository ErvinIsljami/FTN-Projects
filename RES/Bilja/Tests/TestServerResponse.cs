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
    public class TestServerResponse
    {
        [Test]
        public void TestServerResponseAS()
        {
            ServerResponseAS serverResponse = new ServerResponseAS("asdf", EResponseType.Ok, "test");
            Assert.IsNotNull(serverResponse);
            Assert.AreEqual(serverResponse.Message, "test");
            Assert.AreEqual(serverResponse.Type, EResponseType.Ok);
            Assert.AreEqual(serverResponse.UserId, "asdf");
        }

        [Test]
        public void TestServerResponseUpdate()
        {
            ServerResponseUpdate response = new ServerResponseUpdate("test", EResponseType.Error, "test");
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Message, "test");
            Assert.AreEqual(response.Type, EResponseType.Error);
            Assert.AreEqual(response.UserId, "test");
        }
    }
}
