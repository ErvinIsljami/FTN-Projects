using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common.Contracts
{
    [ServiceContract]
    public interface IUpdateSetpoint
    {
        [OperationContract]
        void SetpointUpdate(List<SetpointArray> setpoints);
    }
}
