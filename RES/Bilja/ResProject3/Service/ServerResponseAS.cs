using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Service
{
    class ServerResponseAS : ServerResponse
    {
        QueueModel queues;

        public ServerResponseAS()
        {
            queues = new QueueModel("empty");
        }

        public ServerResponseAS(string userId, EResponseType type, string message, QueueModel queue) : base(userId, type, message)
        {
            this.queues = queue;
        }
    }
}
