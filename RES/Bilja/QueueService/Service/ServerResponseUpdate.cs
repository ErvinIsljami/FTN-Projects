using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueService.Service
{
    public class ServerResponseUpdate : ServerResponse
    {
        public ServerResponseUpdate(string userId, EResponseType type, string message) : base(userId, type, message)
        {
        }
    }
}
