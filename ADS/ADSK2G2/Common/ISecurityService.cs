using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ISecurityService
    {
        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        string AuthenticateUser(string username, string password);
    }
}
