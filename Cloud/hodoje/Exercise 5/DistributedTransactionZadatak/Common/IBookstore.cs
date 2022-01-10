using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
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
