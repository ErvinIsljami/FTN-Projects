using NUnit.Framework;
using QueueService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class TestServer
    {
        Server server;
        [SetUp]
        public void TestSetUp()
        {
            server = new Server(true);
        }

        [Test]
        public void TestContructors()
        {
            Server server2 = new Server();

            Assert.IsNotNull(server);
            Assert.IsNotNull(server2);
        }

        [Test]
        public void TestGetQueues()
        {
            Assert.IsNotNull(server.GetClientQueues());
        }

        [Test]
        public void TestCreateSubscribeTaskCreate()
        {
            server.ServerQueue.QueueA.Enqueue(new ClientRequestCreate("testQ","client1"));
            Task testTask = new Task(() => server.CreateSubscribeTask());
            testTask.Start();
            Thread.Sleep(100);  //odspavamo i damo mu sansu da odradi posao

            Assert.AreEqual(server.ServerQueue.QueueB.Count, 1);
            ServerResponseAS serverResponse = (ServerResponseAS)server.ServerQueue.QueueB.Dequeue();

            Assert.AreEqual(serverResponse.UserId, "client1");
            Assert.IsNotNull(serverResponse.Queues);

        }

        [Test]
        public void TestCreateSubscribeTaskSubscribe()
        {
            server.ServerQueue.QueueA.Enqueue(new ClientRequestSubscribe("clientQueue1", "client1"));
            Task testTask = new Task(() => server.CreateSubscribeTask());
            testTask.Start();
            Thread.Sleep(100);  //odspavamo i damo mu sansu da odradi posao

            Assert.AreEqual(server.ServerQueue.QueueB.Count, 1);
            ServerResponseAS serverResponse = (ServerResponseAS)server.ServerQueue.QueueB.Dequeue();

            Assert.AreEqual(serverResponse.UserId, "client1");
            Assert.IsNotNull(serverResponse.Queues);

        }

        [Test]
        public void TestCreateSubscribeTaskFail1()
        {
            server.ServerQueue.QueueA.Enqueue(new ClientRequestUpdate("unknown", null));
            Task testTask = new Task(() => server.CreateSubscribeTask());
            testTask.Start();
            Thread.Sleep(100);  //odspavamo i damo mu sansu da odradi posao

            Assert.AreEqual(server.ServerQueue.QueueB.Count, 1);
            ServerResponseAS serverResponse = (ServerResponseAS)server.ServerQueue.QueueB.Dequeue();

            Assert.AreEqual(serverResponse.Type, EResponseType.Error);

        }

        [Test]
        public void TestCreateCreateTaskFail2()
        {
            try
            {
                server.ServerQueue.QueueA.Enqueue(new ClientRequestCreate("testQueues", "client1"));
                Task testTask = new Task(() => server.CreateSubscribeTask());
                testTask.Start();
                testTask.Wait();
                
            }
            catch(Exception e)
            {
                Assert.AreEqual(e.Message, "A queue with that name alredy exsits");
            }
        }



    }
}
