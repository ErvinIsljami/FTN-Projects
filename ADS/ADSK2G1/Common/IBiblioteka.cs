using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace Common
{
    [ServiceContract]
    public interface IBiblioteka
    {
        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        [FaultContract(typeof(SecurityException))]
        void DodajNovuKnjigu(Knjiga k, string token);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        [FaultContract(typeof(SecurityException))]
        void ObrisiKnjigu(int id, string token);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        [FaultContract(typeof(SecurityException))]
        void ObrisiKnjiguPoNazivu(string naziv, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        Knjiga VratiNajskupljuKnjiguAutora(string autor, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        List<Knjiga> VratiSortirane(string token);

        [OperationContract]
        //[FaultContract(typeof(SecurityException))]
        List<Knjiga> UzmiSve();

        [OperationContract]
        //[FaultContract(typeof(SecurityException))]
        void UpisiSve(List<Knjiga> lista);
    }
}
