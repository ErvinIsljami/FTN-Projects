using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IBrother
    {
        [OperationContract]
        void Posalji(string vrstaHrane, string kolicina, bool uspjesno);
    }
}
