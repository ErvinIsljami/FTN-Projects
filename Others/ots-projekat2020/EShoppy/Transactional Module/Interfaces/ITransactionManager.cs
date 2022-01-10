using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Transactional_Module.Interfaces
{
    public interface ITransactionManager
    {
        void CreateTransaction(Guid customerID, Guid companyID, Guid offerID, Guid transactionTypeID, int rating);
    }
}
