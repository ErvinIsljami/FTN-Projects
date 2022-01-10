using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IAutomobilService
    {
        [OperationContract]
        [FaultContract(typeof(MyException))]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        void DodajAutomobil(Automobil a, string token);

        [OperationContract]
        [FaultContract(typeof(MyException))]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        void ObrisiAutomobil(string brojSasije, string token);

        [OperationContract]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        string IzlistajSveAutomobile(string token);

        [OperationContract]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        List<Automobil> SortirajAutomobile(string token);

        [OperationContract]
        [FaultContract(typeof(BezbednosniIzuzetak))]
        List<Automobil> IzlistajAutomobileNekeMarke(string marka, string token);
    }
}
