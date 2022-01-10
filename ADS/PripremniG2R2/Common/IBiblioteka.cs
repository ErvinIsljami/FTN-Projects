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
        [FaultContract(typeof(SecException))]
        [FaultContract(typeof(DbException))]
        void DodajClana(Clan clan, string token);

        [OperationContract]
        [FaultContract(typeof(SecException))]
        [FaultContract(typeof(DbException))]
        void IzmeniClana(Clan clan, string token);

        [OperationContract]
        [FaultContract(typeof(SecException))]
        [FaultContract(typeof(DbException))]
        void IzbrisiClana(string jmbg, string token);

    }
}
