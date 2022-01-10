using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueService.Service
{
    public class ServerResponseAS : ServerResponse
    {
        public QueueModel Queues;

        public ServerResponseAS()
        {
            Queues = new QueueModel("empty");
        }

        public ServerResponseAS(string userId, EResponseType type, string message) : base(userId, type, message)
        {
            Queues = new QueueModel("empty");
        }
    }
}
