using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Client(DataModel modelData, string id, Server server)
        {
            this.ModelData = modelData;
            this.Id = id;
            this.server = server;
            queues = null;
        }

        public Client(Server server)
        {
            modelData = new DataModel();
            Id = Guid.NewGuid().ToString();
            this.server = server;
        }

        public Client() //mora zbog xml-a
        {
        }

        public DataModel ModelData { get => modelData; set => modelData = value; }
        [XmlIgnore]
        public Server Server { get => server; set => server = value; }
        public string Id { get => id; set => id = value; }

        public string ModelToXml()
        {
            string path = "../../Database/" + "model_" + Id + ".xml";
            XmlSerializer serializer = new XmlSerializer(typeof(DataModel));
            using (TextWriter textWriter = new StreamWriter(path))
            {
                // ispis u XML datoteku
                serializer.Serialize(textWriter, modelData);
            }

            return path;
        }

        public void Create()
        {
            string queueName = "";
            Console.WriteLine("Input queue name");
            queueName = Console.ReadLine();
            ClientRequest request = new ClientRequestCreate(queueName, id);
            server.ServerQueue.QueueA.Enqueue(request);
        }
    }
}
