using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Bank
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
