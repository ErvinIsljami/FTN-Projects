using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common;

namespace Contracts
{
    [ServiceContract]
    public interface IDeviceChangeSetService
    {
        [OperationContract]
        void SendNewMeasurement(Guid id, List<ChangeSet> measurement);
    }
}
