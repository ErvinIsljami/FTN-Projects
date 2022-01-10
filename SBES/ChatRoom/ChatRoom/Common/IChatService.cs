using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void ConnectTo();

        [OperationContract(IsOneWay = true)]
        void SendMessageToServer(byte[] message);

        [OperationContract]
        ICryptographyInterface GetCryptoAlg();
    }
}