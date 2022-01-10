using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IBanka
    {
        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        [FaultContract(typeof(SecurityException))]
        void DodajRacun(Racun r, string token);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        [FaultContract(typeof(SecurityException))]
        void ObrisiRacun(int id, string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        List<Racun> VratiSveDevizne(string token);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        Racun NajbogatijiDinarski(string token);

        [OperationContract]
        List<Racun> UzmiSve();

        [OperationContract]
        void UpisiSve(List<Racun> lista);
   
    }
}
