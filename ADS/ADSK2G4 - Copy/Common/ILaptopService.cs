using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface ILaptopService
    {
        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        void DodajLaptop(Laptop l, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        void ObrisiLaptop(int id, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        List<Laptop> VratiSortirane(string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        Laptop VratiNajskupljiIzMarke(string marka, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        List<Laptop> VratiUIntervalu(double gornjaGranica, double donjaGranica, string token);

    }
}
