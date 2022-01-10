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
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(SecException))]
        void DodajClana(Clan clan, string token);

        [OperationContract]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(SecException))]
        void IzmeniClana(Clan clan, string token);

        [OperationContract]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(SecException))]
        void IzbrisiClana(string jmbg, string token);

        [OperationContract]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(SecException))]
        void DodajKnjiguClanu(string jmbg, Knjiga k, string token);

        [OperationContract]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(SecException))]
        void DodajKnjiguClanuPoImenu(string ime, string prezime, Knjiga k, string token);

        [OperationContract]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(SecException))]
        void DodajKnjigu(Knjiga k, string token);

        [OperationContract]
        [FaultContract(typeof(SecException))]
        List<Clan> VratiSortiraneClanovePoUkupnojCeniKnjiga(string token);
        
    }
}
