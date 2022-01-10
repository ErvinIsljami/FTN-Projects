using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IApoteka
    {
        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        void DodajLek(Lek l, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        void ObrisiLek(int id, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        Lek VratiNajskupljiOdProizvodjaca(string proizvodjac, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        List<Lek> VratiSortirane(string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        List<Lek> VratiSkupljeOd(double cena, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        int VratiBrojLekovaProizvodjaca(string proizvodjac, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        Dictionary<int, Lek> PreuzmiBazu(string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void PosaljiBazu(Dictionary<int, Lek> baza, string token);
    }
}
