using QueueService.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Service
{
    public class ClientRequestUpdate : ClientRequest
    {
        private DataModel dataModel;
        public ClientRequestUpdate(string userId, DataModel dataModel) : base(userId, ERequestType.UPDATE)
        {
            this.DataModel = dataModel;
        }

        public DataModel DataModel { get => dataModel; set => dataModel = value; }
    }
}
