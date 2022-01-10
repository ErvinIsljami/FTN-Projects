using Common;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface IControllerChangeSetService
    {
        [OperationContract]
        void SendMeasurements(LocalControllerDataModel dataModel);
    }
}
