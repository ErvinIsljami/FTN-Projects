using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributedTransaction
{
    [ServiceContract]
    public interface IPurchase
    {
        [OperationContract]
        bool OrderItem(string bookId, string userId, int numOfBooksToBuy);
    }
}
