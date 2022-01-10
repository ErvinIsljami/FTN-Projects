using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract(Name = "EStanjeServera")]
    public enum EStanjeServera
    {
        [EnumMemberAttribute]
        Nepoznato,
        [EnumMemberAttribute]
        Primarni,
        [EnumMemberAttribute]
        Sekundarni
    }
    [ServiceContract]
    public interface IStanjeServisa
    {
        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        EStanjeServera ProveraStanja();
        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        void AzuriranjeStanja(EStanjeServera stanje);
    }
}
