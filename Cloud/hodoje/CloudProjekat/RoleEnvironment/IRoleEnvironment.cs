using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RoleEnvironment
{
    [ServiceContract]
    public interface IRoleEnvironment
    {
        [OperationContract]
        string GetAddress(string myAssemblyName, string containerId);

        [OperationContract]
        string[] BrotherInstances(string myAssemblyName, string myAddress);

        [OperationContract]
        string GetServiceAddress(string serviceName);
    }
}
