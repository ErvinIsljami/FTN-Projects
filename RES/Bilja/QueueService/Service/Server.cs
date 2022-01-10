using QueueService.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace QueueService.Service
{
    public class Server
    {
        Dictionary<string, QueueModel> queues;
        object padLock = new object();
        QueueModel serverQueue;

        public Server()
        {
            this.Queues = new Dictionary<string, QueueModel>();
            this.Queues.Add("clientQueue1", new QueueModel("clientQueue1"));
            ServerQueue = new QueueModel("serverQueue");
            Task addUpdateTask = new Task(() => CreateSubscribeTask());
            addUpdateTask.Start();
        }
        public Server(bool isTest)
        {
            if(isTest)
            {
                this.Queues = new Dictionary<string, QueueModel>();
                this.Queues.Add("testQueues", new QueueModel("testQueues"));
                ServerQueue = new QueueModel("serverQueue");
                
            }
        }

        public QueueModel ServerQueue { get => serverQueue; set => serverQueue = value; }

        public Dictionary<string, QueueModel> Queues { get => queues; set => queues = value; }

        public List<QueueModel> GetClientQueues()
        {
            return Queues.Values.ToList();
        }

        public async void CreateSubscribeTask()
        {
            while (true)
            {
                if(ServerQueue.QueueA.Count == 0)
                {
                    await Task.Delay(100);
                    continue;
                }
                var request = ServerQueue.QueueA.Dequeue(); 
                if (request.Type == ERequestType.CREATE)
                {
                    ClientRequestCreate clientRequest = request as ClientRequestCreate;
                    if (Queues.ContainsKey(clientRequest.QueueName))
                    {
                        throw new Exception("A queue with that name alredy exsits");
                    }
                    else
                    {
                        QueueModel queueModel = new QueueModel(clientRequest.QueueName);
                        Queues.Add(clientRequest.QueueName, queueModel);
                        Task task = new Task(() => QueueTask(queueModel));
                        task.Start();

                        ServerResponseAS response = new ServerResponseAS(clientRequest.UserId, EResponseType.Ok, "Create succesfull");
                        response.Queues = queueModel;
                        ServerQueue.QueueB.Enqueue(response);
                        
                    }
                }
                else if (request.Type == ERequestType.SUBSCRIBE)
                {
                    ClientRequestSubscribe clientRequest = request as ClientRequestSubscribe;
                    if (Queues.ContainsKey(clientRequest.QueueName))
                    {
                        ServerResponseAS response = new ServerResponseAS(clientRequest.UserId, EResponseType.Ok, "Subscribe succesfull");
                        response.Queues = Queues[clientRequest.QueueName];
                        
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var dataModel = js.Deserialize<DataModel>(clientRequest.JsonModel);


                        Client newClient = new Client(dataModel, request.UserId, this);
                        UserDB db = new UserDB("UsersDataBase");
                        if(!db.Clients.Any(x => x.Id == newClient.Id))
                        {
                            db.Clients.Add(newClient);
                            db.SaveChanges();
                        }
                        ServerQueue.QueueB.Enqueue(response);
                    }
                    else
                    {
                        ServerResponse response = new ServerResponseAS(clientRequest.UserId, EResponseType.Error, "Subscribe unsuccesfull");
                        ServerQueue.QueueB.Enqueue(response);
                    }
                }
                else
                {
                    ServerResponse response = new ServerResponseAS(request.UserId, EResponseType.Error, "Request type unrecognized");
                    ServerQueue.QueueB.Enqueue(response);
                }

            }
        }

        public async void QueueTask(QueueModel model)
        {
            while(true)
            {
                if(model.QueueA.Count == 0)
                {
                    await Task.Delay(100);
                }
                else
                {
                    var request = model.QueueA.Dequeue();
                    if(request.Type == ERequestType.CREATE)
                    {
                        ClientRequestCreate clientRequest = request as ClientRequestCreate;
                        if (Queues.ContainsKey(clientRequest.QueueName))
                        {
                            ServerResponse response = new ServerResponseAS(request.UserId,
                                                                         EResponseType.Error,
                                                                         "A queue with that name alredy exsits");
                        }
                        else
                        {
                            QueueModel queueModel = new QueueModel(clientRequest.QueueName);
                            Queues.Add(clientRequest.QueueName, queueModel);
                        }
                    }
                    else if (request.Type == ERequestType.SUBSCRIBE)
                    {
                        ClientRequestSubscribe clientRequest = request as ClientRequestSubscribe;
                        if(Queues.ContainsKey(clientRequest.QueueName))
                        {
                            //dodati mehanizam kako da server odgovori i u odgovoru posalje queue referencu
                        }
                    }
                    else if (request.Type == ERequestType.UPDATE)
                    {
                        ClientRequestUpdate clientRequest = request as ClientRequestUpdate;
                        
                        UserDB dataBase = new UserDB("UsersDataBase");
                        var client = dataBase.Clients.SingleOrDefault(x => x.Id == clientRequest.UserId); //linq
                        
                        if(client != null)
                        {
                            client.ModelData = clientRequest.DataModel;
                            client.ModelData.Id += DateTime.Now.ToString();
                            dataBase.SaveChanges();
                        }
                    }
                }
            }
        }

    }
}
