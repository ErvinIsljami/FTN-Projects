using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface ITransaction
    {
        [OperationContract]
        bool Prepare();

        [OperationContract]
        void Commit();

        [OperationContract]
        void Rollback();
    }
}
