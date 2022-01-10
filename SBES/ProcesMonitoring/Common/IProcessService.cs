using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IProcessService
    {
        [OperationContract]
        [FaultContract(typeof(Exception))]
        void StartProcess(ProcessModel process);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void StopProcess(long id);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        string ShowActiveProcess();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void StopAllProcess();
    }
}
