using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IObavesti
    {
        [OperationContract]
        void ObavestiSvogBrata(Roba r, string uspesno);
    }
}
