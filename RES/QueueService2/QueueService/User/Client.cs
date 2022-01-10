using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using QueueService.Service;

namespace QueueService.User
{
    public class Client
    {
        private DataModel modelData;
        private string id;
        private Server server;
        private QueueModel queues;
        private List<Client> clients;
        private object padlock = new object();
        private Task automaticReadFromQueueTask;

        public Client(DataModel modelData, string id, Server server)
        {
            this.ModelData = modelData;
            this.Id = id;
            this.server = server;
            Queues = null;
            clients = new List<Client>();
            automaticReadFromQueueTask = new Task(() => AutomaticReadFromQueue());
            automaticReadFromQueueTask.Start();
        }

        public Client(Server server)
        {
            modelData = new DataModel();
            Id = Guid.NewGuid().ToString();
            modelData.Id = this.id + "_model";
            this.server = server;
            clients = new List<Client>();
            automaticReadFromQueueTask = new Task(() => AutomaticReadFromQueue());
            automaticReadFromQueueTask.Start();
        }

        public Client() 
        {

        }

        public DataModel ModelData { get => modelData; set => modelData = value; }
        [NotMapped]
        public Server Server { get => server; set => server = value; }
        public string Id { get => id; set => id = value; }
        [NotMapped]
        public QueueModel Queues { get => queues; set => queues = value; }

        public string ModelToJSON()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            return js.Serialize(modelData);
        }

        public void Create()
        {
            string queueName = "";
            Console.WriteLine("Input queue name");
            queueName = Console.ReadLine();
            ClientRequest request = new ClientRequestCreate(queueName, id);
            server.ServerQueue.QueueA.Enqueue(request);
        }
        public void Subscribe()
        {
            Console.WriteLine("Available queues for subscribe:");
            var queues = server.GetClientQueues();
            queues.ForEach(x => Console.WriteLine($"\t" + x.QueueName));
            Console.WriteLine("Input queue name: ");
            string queueName = Console.ReadLine();

            ClientRequest clientRequest = new ClientRequestSubscribe("queueName", Id);
            server.ServerQueue.QueueA.Enqueue(clientRequest);
        }
        public void Update()
        {
            string id;
            string items;
            string positions;

            Console.WriteLine("Input items separated with semicolons ('name','quantity','isActive','destructivePower';...)");
            items = Console.ReadLine();
            Console.WriteLine("Input positions separated with semicolon ('x','y','z';...)");
            positions = Console.ReadLine();
            DataModel dataModel = new DataModel();
            var itemsList = items.Split(';');
            var positionList = positions.Split(';');
            dataModel.Id = this.Id + "_model";
            foreach (string item in itemsList)
            {
                if (item == string.Empty)
                    continue;
                var itemParts = item.Split(',');
                Item newItem = new Item(itemParts[0], double.Parse(itemParts[1]), bool.Parse(itemParts[2]), double.Parse(itemParts[3]));
                dataModel.Items.Add(newItem);
            }
            foreach (string position in positionList)
            {
                if (position == string.Empty)
                    continue;
                var posParts = position.Split(',');
                Position newPosition = new Position(double.Parse(posParts[0]), double.Parse(posParts[1]), double.Parse(posParts[2]));
                dataModel.Positions.Add(newPosition);
            }
            this.modelData = dataModel;

            ClientRequest clientRequest = new ClientRequestUpdate(this.id, this.modelData);
            this.queues.QueueA.Enqueue(clientRequest);
        }

        private async void AutomaticReadFromQueue()
        {
            while(true)
            {
                if(queues.QueueB.Count == 0)
                {
                    await Task.Delay(100);
                    continue;
                }
                ServerResponseUpdate response = null;
                lock (padlock)
                {
                    response = (ServerResponseUpdate)queues.QueueB.Peek();
                }

                if (response != null)
                {
                    if (response.UserId == this.id)
                    {
                        Console.WriteLine($"Client[{id}]: " + response.Message);
                        queues.QueueB.Dequeue();
                    }
                    else
                    {
                        Console.WriteLine($"Client[{id}]: Client with id {response.UserId} sent message to queue.");
                    }
                }

            }
        }

    }
}
