using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IProdavnicaTelefona
    {
        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        [FaultContract(typeof(DatabaseException))]
        void DodajTelefon(Telefon t, string token);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        [FaultContract(typeof(SecurityException))]
        void ObrisiTelefon(int id, string token);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        [FaultContract(typeof(SecurityException))]
        List<Telefon> VratiSortirane(string token);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        [FaultContract(typeof(SecurityException))]
        Telefon NadjiNajskupljiIzMarke(string marka, string token);


    }
}
