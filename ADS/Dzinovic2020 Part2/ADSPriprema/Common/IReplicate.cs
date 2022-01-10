using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IReplicate
    {
        [OperationContract]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        void PosaljiBazu(string token, Dictionary<long, Clan> baza);

        [OperationContract]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        Dictionary<long, Clan> PreuzmiBazu(string token);
    }
}
