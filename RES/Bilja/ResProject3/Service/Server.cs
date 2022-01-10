using QueueService.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Service
{
    public class Server
    {
        Dictionary<string, QueueModel> queues;
        object padLock = new object();
        public QueueModel ServerQueue { get; set; }
        public QueueModel FirstQueue { get; set; }
        private Dictionary<string, QueueModel> clientQueueMap;

        public Server()
        {
            this.queues = new Dictionary<string, QueueModel>();
            ServerQueue = new QueueModel("serverQueue");
            Task addUpdateTask = new Task(() => AddUpdateTask());
            addUpdateTask.Start();
            FirstQueue = new QueueModel("FirstQueue");
            clientQueueMap = new Dictionary<string, QueueModel>();
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
                        ServerResponse response = new ServerResponseAS( clientRequest.UserId, 
                                                                        EResponseType.Error,
                                                                        "Queue with given name alraedy excits", 
                                                                        null);
                        ServerQueue.QueueB.Enqueue(response);
                    }
                    else
                    {
                        QueueModel queueModel = new QueueModel(clientRequest.QueueName);
                        queues.Add(clientRequest.QueueName, queueModel);
                        Task task = new Task(() => QueueTask(queueModel));
                        task.Start();
                        FirstQueue.QueueA.Enqueue(request);
                    }
                }
                else if (request.Type == ERequestType.SUBSCRIBE)
                {
                    ClientRequestSubscribe clientRequest = request as ClientRequestSubscribe;
                    if (queues.ContainsKey(clientRequest.QueueName))
                    {
                        ServerResponse response = new ServerResponseAS( clientRequest.UserId,   
                                                                        EResponseType.Ok, 
                                                                        "Subscribe succesfull",
                                                                        queues[clientRequest.QueueName]);
                        
                        ServerQueue.QueueB.Enqueue(response);
                    }
                    else
                    {
                        ServerResponse response = new ServerResponseAS( clientRequest.UserId, 
                                                                        EResponseType.Error,    
                                                                        "Subscribe unsuccesfull",
                                                                        null);
                        ServerQueue.QueueB.Enqueue(response);
                    }
                }
                else
                {
                    throw new Exception("Request type not supported");
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
                    if (request.Type == ERequestType.UPDATE)
                    {
                        ClientRequestUpdate clientRequest = request as ClientRequestUpdate;
                        //TODO zavrsiti prvo kopmonentu repository
                    }
                    else
                    {
                        ServerResponse response = new ServerResponseUpdate(request.UserId,
                                                                        EResponseType.Error,
                                                                        "Request type unrecognized");
                        ServerQueue.QueueB.Enqueue(response);
                    }
                }
            }
        }

        private async void ReadRepositoryResponses()
        {
            while(true)
            {
                if(FirstQueue.QueueB.Count == 0)
                {
                    await Task.Delay(100);
                }
                var message = FirstQueue.QueueB.Dequeue();
                if(message.Type == EResponseType.Error)
                {
                    string userId = message.UserId;
                    clientQueueMap[userId].QueueB.Enqueue(new ServerResponseUpdate(userId, message.Type, message.Message));
                }


            }
        }


    }
}