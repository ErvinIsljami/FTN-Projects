using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IBank : ITransaction
    {
        [OperationContract]
        void ListClients();
        [OperationContract]
        void EnlistMoneyTransfer(string userId, double amount);
    }
}
