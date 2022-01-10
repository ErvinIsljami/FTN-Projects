using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueService.Service
{
    public class QueueModel
    {
        string queueName;
        private Queue<ClientRequest> queueA;
        private Queue<ServerResponse> queueB;

        public QueueModel(string queueName)
        {
            this.QueueName = queueName;
            QueueA = new Queue<ClientRequest>();
            queueB = new Queue<ServerResponse>();
        }

        public string QueueName { get => queueName; set => queueName = value; }
        public Queue<ClientRequest> QueueA { get => queueA; set => queueA = value; }
        public Queue<ServerResponse> QueueB { get => queueB; set => queueB = value; }
    }
}
