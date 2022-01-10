using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IProdavnicaAutomobila
    {
        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        void DodajAuto(Automobil auto, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        void ObrisiAuto(int id, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        Automobil VratiNajskupljiIzMarke(string marka, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        List<Automobil> VratiSveAutomobile(string token);

        [OperationContract]
        List<Automobil> UzmiSve();

        [OperationContract]
        void UpisiSve(List<Automobil> automobili);
    }
}
