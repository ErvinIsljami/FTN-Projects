using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public interface IChatServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendMessageToClients(string name, byte[] cryptMessage);
    }
}
