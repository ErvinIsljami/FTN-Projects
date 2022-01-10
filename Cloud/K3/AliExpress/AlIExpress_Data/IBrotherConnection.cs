using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace AlIExpress_Data
{  
    [ServiceContract]
    public interface IBrotherConnection
    {
       [OperationContract]
       void ReadMessage(string message);
    }
}
