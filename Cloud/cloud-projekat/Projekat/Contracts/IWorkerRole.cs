using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface IWorkerRole
    {
        [OperationContract]
        void Start(String containerId);
        [OperationContract]
        void Stop();
    }
}
