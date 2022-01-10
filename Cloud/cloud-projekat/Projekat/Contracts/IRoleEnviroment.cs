using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface IRoleEnvironment
    {
        [OperationContract]
        string GetAddress(String myAssemblyName, String containerId);

        [OperationContract]
        string[] BrotherInstances(String myAssemblyName, String myAddress);
    }
}
