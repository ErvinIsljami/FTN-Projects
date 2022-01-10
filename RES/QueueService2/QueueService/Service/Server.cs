using QueueService.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueService.Service
{
    public class Server
    {
        Dictionary<string, QueueModel> queues;
        object padLock = new object();
        QueueModel serverQueue;

        public Server()
        {
            this.queues = new Dictionary<string, QueueModel>();
            this.queues.Add("clientQueue1", new QueueModel("clientQueue1"));
            ServerQueue = new QueueModel("serverQueue");
            Task addUpdateTask = new Task(() => AddUpdateTask());
            addUpdateTask.Start();
        }

        public QueueModel ServerQueue { get => serverQueue; set => serverQueue = value; }

        public List<QueueModel> GetClientQueues()
        {
            return queues.Values.ToList();
        }

        private async void AddUpdateTask()
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
                    if (queues.ContainsKey(clientRequest.QueueName))
                    {
                        throw new Exception("A queue with that name alredy exsits");
                    }
                    else
                    {
                        QueueModel queueModel = new QueueModel(clientRequest.QueueName);
                        queues.Add(clientRequest.QueueName, queueModel);
                        Task task = new Task(() => QueueTask(queueModel));
                        task.Start();
                        
                        ServerResponse response = new ServerResponseAS(clientRequest.UserId, EResponseType.Ok, "Create succesfull");
                        ServerQueue.QueueB.Enqueue(response);
                    }
                }
                else if (request.Type == ERequestType.SUBSCRIBE)
                {
                    ClientRequestSubscribe clientRequest = request as ClientRequestSubscribe;
                    if (queues.ContainsKey(clientRequest.QueueName))
                    {
                        ServerResponse response = new ServerResponseAS(clientRequest.UserId, EResponseType.Ok, "Subscribe succesfull");
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

        private async void QueueTask(QueueModel model)
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
                        if (queues.ContainsKey(clientRequest.QueueName))
                        {
                            ServerResponse response = new ServerResponseAS(request.UserId,
                                                                         EResponseType.Error,
                                                                         "A queue with that name alredy exsits");
                        }
                        else
                        {
                            QueueModel queueModel = new QueueModel(clientRequest.QueueName);
                            queues.Add(clientRequest.QueueName, queueModel);
                        }
                    }
                    else if (request.Type == ERequestType.SUBSCRIBE)
                    {
                        ClientRequestSubscribe clientRequest = request as ClientRequestSubscribe;
                        if(queues.ContainsKey(clientRequest.QueueName))
                        {
                            //dodati mehanizam kako da server odgovori i u odgovoru posalje queue referencu
                        }
                    }
                    else if (request.Type == ERequestType.UPDATE)
                    {
                        ClientRequestUpdate clientRequest = request as ClientRequestUpdate;
                        
                        UserDB dataBase = new UserDB();
                        var client = dataBase.Clients.Find(clientRequest.UserId);
                        if(client != null)
                        {
                            client.ModelData = clientRequest.DataModel;
                            dataBase.SaveChanges();
                        }
                    }
                }
            }
        }

    }
}
