using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface IBookstore : ITransaction
    {
        [OperationContract]
        void ListAvailableItems();

        [OperationContract]
        void EnlistPurchase(string bookID, int count);

        [OperationContract]
        double GetItemPrice(string bookID);
    }
}
