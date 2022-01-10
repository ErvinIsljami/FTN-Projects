using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IServiceZvucnika
    {
        [OperationContract]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(SecException))]
        void DodajZvucnik(Zvucnik z);

        [OperationContract]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(SecException))]
        void ObrisiZvucnik(int id);

        [OperationContract]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(SecException))]
        List<Zvucnik> VratiSveZvucnike();
    }
}
