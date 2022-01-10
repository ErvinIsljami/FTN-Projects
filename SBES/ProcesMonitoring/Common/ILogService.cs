using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ILogService
    {
        [OperationContract]
        void LogEvent(string message, ECriticalLvl criticalLvl, DateTime time);
    }
}