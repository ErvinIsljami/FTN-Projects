using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Service
{
    public class ClientRequestSubscribe : ClientRequest
    {
        private string queueName;

        public ClientRequestSubscribe(string queueName, string userId) : base(userId, ERequestType.SUBSCRIBE)
        {
            this.queueName = queueName;
        }

        public string QueueName { get => queueName; set => queueName = value; }
    }
}
