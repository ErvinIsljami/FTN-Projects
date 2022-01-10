using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Service
{
    public class ClientRequestCreate : ClientRequest
    {
        private string queueName;

        public ClientRequestCreate(string queueName, string userId) : base(userId, ERequestType.CREATE)
        {
            this.queueName = queueName;
        }

        public string QueueName { get => queueName; set => queueName = value; }
    }
}
