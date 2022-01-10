using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributedTransaction
{
    [ServiceContract]
    public interface IBookstore : ITransaction
    {
        [OperationContract]
        void ListAvailableItems();

        [OperationContract]
        void EnlistPurchase(string bookId, int count);

        [OperationContract]
        double GetItemPrice(string bookId);
    }
}
