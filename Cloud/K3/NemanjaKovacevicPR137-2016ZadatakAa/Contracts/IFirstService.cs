using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IFirstService
    {
        [OperationContract]
        void ProslediBratskojInstanci(string vrstaRobe, int kolicina, bool uspesnost);
    }
}
