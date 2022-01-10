using NUnit.Framework;
using QueueService.Service;
using QueueService.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Tests
{
    [TestFixture]
    public class TestClient
    {
        Server server;
        Client client;
        DataModel clientDataModel;

        [SetUp]
        public void SetUP()
        {
            server = new Server(true);
            client = new Client(true);
            client.Server = server;
            client.Queues = server.Queues["testQueues"];
            clientDataModel = new DataModel();
            clientDataModel.Id = "testModel";
            clientDataModel.Items.Add(new Item("test1", 1, true, 1));
            clientDataModel.Items.Add(new Item("test2", 2, true, 2));
            clientDataModel.Positions.Add(new Position(1, 2, 3));
            clientDataModel.Positions.Add(new Position(4, 5, 6));
            client.ModelData = clientDataModel;
        }

        [Test]
        public void TestConstructors()
        {
            Client client2 = new Client();
            Client client3 = new Client(new Server());
            Client client4 = new Client(new DataModel(), "4234", new Server());

            Assert.IsNotNull(client);
            Assert.IsNotNull(client2);
            Assert.IsNotNull(client3);
            Assert.IsNotNull(client4);
        }

        [Test]
        public void TestModelToJSON()
        {
            string jsonData = client.ModelToJSON();
            JavaScriptSerializer js = new JavaScriptSerializer();

            DataModel deserializedData = js.Deserialize<DataModel>(jsonData);
            Assert.AreEqual(client.ModelData.Id, deserializedData.Id);
        }

        [Test]
        public void TestCreate()
        {
            string str = "newQueues";
            var input = new StringReader(str);
            Console.SetIn(input);

            client.Create();

            Assert.AreEqual(server.ServerQueue.QueueA.Count, 1);
            var request = server.ServerQueue.QueueA.Dequeue();
            Assert.AreEqual(request.Type, ERequestType.CREATE);
            Assert.AreEqual(request.UserId, client.Id);

            ClientRequestCreate crq = (ClientRequestCreate)request;
            Assert.AreEqual(crq.QueueName, str);
        }

        [Test]
        public void TestSubscribe()
        {
            string str = "testQueues";
            var input = new StringReader(str);
            Console.SetIn(input);

            client.Subscribe();

            Assert.AreEqual(server.ServerQueue.QueueA.Count, 1);
            var request = server.ServerQueue.QueueA.Dequeue();
            Assert.AreEqual(request.Type, ERequestType.SUBSCRIBE);
            Assert.AreEqual(request.UserId, client.Id);

            ClientRequestSubscribe crq = (ClientRequestSubscribe)request;
            Assert.AreEqual(crq.QueueName, str);
        }
        [Test]
        public void TestUpdate()
        {
            string str = "item,1,true,5345" + Environment.NewLine;
            str += "1,43,32" + Environment.NewLine;

            var input = new StringReader(str);
            Console.SetIn(input);

            client.Update();

            Assert.AreEqual(client.Queues.QueueA.Count, 1);
            var request = client.Queues.QueueA.Dequeue();
            Assert.AreEqual(request.Type, ERequestType.UPDATE);
            Assert.AreEqual(request.UserId, client.Id);

            ClientRequestUpdate crq = (ClientRequestUpdate)request;
            Assert.AreEqual(crq.DataModel, client.ModelData);
        }

        [Test]
        public void TestAutomaticReadFromQueue()
        {
            client.Queues.QueueB.Enqueue(new ServerResponseUpdate(client.Id, EResponseType.Ok, "testOk"));
            Task testTask = new Task(() => client.AutomaticReadFromQueue());
            testTask.Start();
            Thread.Sleep(100);  //odspavamo i damo mu sansu da odradi posao

            Assert.AreEqual(client.Queues.QueueB.Count, 0);
        }

        public void TestAutomaticReadFromServerQueue()
        {
            server.ServerQueue.QueueB.Enqueue(new ServerResponseAS(client.Id, EResponseType.Ok, "testOk"));
            Task testTask = new Task(() => client.AutomaticReadFromServerQueue());
            testTask.Start();
            Thread.Sleep(100);  //odspavamo i damo mu sansu da odradi posao

            Assert.AreEqual(server.ServerQueue.QueueB.Count, 0);
        }
    }
}
