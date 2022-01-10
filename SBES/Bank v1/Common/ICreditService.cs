using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ICreditService
    {
        [OperationContract]
        void ApplyForCredit(double amount, string userName);

        [OperationContract]
        List<Credit> GetAllCreditRequests();

        [OperationContract]
        void ApproveCredit(string userName);

        [OperationContract]
        void DenyCredit(string userName);
    }
}