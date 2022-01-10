using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditSector
{
    public class CreditService : ICreditService
    {
        private string dbPath = "../../../Resources/credits.xml";
        public void ApplyForCredit(double amount, string userName)
        {
            XmlReaderWriter xmlReaderWriter = new XmlReaderWriter();
            var lista = xmlReaderWriter.DeserializeObject<List<Credit>>(dbPath);

            lista.Add(new Credit(userName, amount));

            xmlReaderWriter.SerializeObject<List<Credit>>(lista, dbPath);
        }

        public void ApproveCredit(string userName)
        {
            XmlReaderWriter xmlReaderWriter = new XmlReaderWriter();
            var lista = xmlReaderWriter.DeserializeObject<List<Credit>>(dbPath);

            foreach (Credit c in lista)
            {
                if (c.User == userName)
                {
                    c.IsCreditAllowed = true;
                    xmlReaderWriter.SerializeObject<List<Credit>>(lista, dbPath);
                    break;
                }
            }
        }

        public void DenyCredit(string userName)
        {
            XmlReaderWriter xmlReaderWriter = new XmlReaderWriter();
            var lista = xmlReaderWriter.DeserializeObject<List<Credit>>(dbPath);

            foreach (Credit c in lista)
            {
                if (c.User == userName)
                {
                    lista.Remove(c);
                    xmlReaderWriter.SerializeObject<List<Credit>>(lista, dbPath);
                    break;
                }
            }
        }

        public List<Credit> GetAllCreditRequests()
        {
            XmlReaderWriter xmlReaderWriter = new XmlReaderWriter();
            var lista = xmlReaderWriter.DeserializeObject<List<Credit>>(dbPath);

            return lista.Where(x => x.IsCreditAllowed == false).ToList();
        }
    }
}
