using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IParkingService
    {
        [OperationContract]
        [FaultContract(typeof(Exception))]
        void AddParkingZone(ParkingZone parkingZone);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void PayParking(string zoneColor, string registration);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        bool IsParkingPayed(string registration);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void AddTicketToUser(string registration, string parkingZone);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void DeleteTicket(string registration);

    }
}
