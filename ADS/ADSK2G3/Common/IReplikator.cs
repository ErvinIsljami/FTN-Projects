using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IReplikator
    {
        [OperationContract]
        List<Telefon> UzmiSve();
        [OperationContract]
        void UpisiSve(List<Telefon> lista);
    }
}
