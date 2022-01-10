using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        void MakeNewAccount(string userName);

        [OperationContract]
        void ApplyForCredit(string userName, double amount);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void ExecuteTransaction(string userName, double amount);

        [OperationContract]
        List<Account> GetAllAccountRequests();

        [OperationContract]
        void ApproveAccount(string userName);

        [OperationContract]
        List<Credit> GetAllCreditRequests();

        [OperationContract]
        void ApproveCredit(string userName);

        [OperationContract]
        void DenyCredit(string userName);

        [OperationContract]
        void DenyAccount(string userName);
    }
}
