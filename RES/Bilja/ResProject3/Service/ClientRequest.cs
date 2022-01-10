using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Service
{
    public abstract class ClientRequest
    {
        protected string userId;
        protected ERequestType type;
        //todo 
        public ClientRequest(string userId, ERequestType type)
        {
            this.UserId = userId;
            this.Type = type;
        }

        public string UserId { get => userId; set => userId = value; }
        public ERequestType Type { get => type; set => type = value; }
    }
}
