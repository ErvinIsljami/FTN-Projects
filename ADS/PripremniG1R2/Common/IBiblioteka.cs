using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IBiblioteka
    {
        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        void DodajClana(Clan clan, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        void DodajKnjigu(Knjiga knjiga, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        void ObrisiKnjigu(string isbn, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        void IzmeniClana(Clan clan, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        void IzbrisiClana(string jmbg, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void PosaljiBazu(Dictionary<string, Clan> baza, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        Dictionary<string, Clan> PreuzmiBazu(string token);
    }
}
