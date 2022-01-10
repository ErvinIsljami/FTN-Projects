using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Service
{
    public abstract class ServerResponse
    {
        string userId;
        EResponseType type;
        string message;

        public string UserId { get => userId; set => userId = value; }
        public EResponseType Type { get => type; set => type = value; }
        public string Message { get => message; set => message = value; }

        public ServerResponse(string userId, EResponseType type, string message)
        {
            this.UserId = userId;
            this.Type = type;
            this.Message = message;
        }

        public ServerResponse()
        {
        }
    }
}
