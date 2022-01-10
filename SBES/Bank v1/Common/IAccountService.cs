using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        void MakeNewAccount(string userName);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void ExecuteTransaction(double amount, string userName);

        [OperationContract]
        List<Account> GetAllAccountRequests();

        [OperationContract]
        void ApproveAccount(string userName);

        [OperationContract]
        void DenyAccount(string userName);
    }
}
