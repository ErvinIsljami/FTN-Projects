using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueService.Service
{
    class ServerResponseAS : ServerResponse
    {
        QueueModel queues;

        public ServerResponseAS()
        {
            queues = new QueueModel("empty");
        }

        public ServerResponseAS(string userId, EResponseType type, string message) : base(userId, type, message)
        {
            queues = new QueueModel("empty");
        }
    }
}
