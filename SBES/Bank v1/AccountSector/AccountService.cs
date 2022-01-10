using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountSector
{
    public class AccountService : IAccountService
    {
        private string dbPath = "../../../Resources/users.xml";

        public void ApproveAccount(string userName)
        {
            XmlReaderWriter xmlReaderWriter = new XmlReaderWriter();
            var lista = xmlReaderWriter.DeserializeObject<List<Account>>(dbPath);

            foreach (Account a in lista)
            {
                if (Formatter.ParseName(userName) == a.User)
                {
                    a.IsAccountActive = true;
                    xmlReaderWriter.SerializeObject<List<Account>>(lista, dbPath);
                    break;
                }
            }
        }

        public void DenyAccount(string userName)
        {
            XmlReaderWriter xmlReaderWriter = new XmlReaderWriter();
            var lista = xmlReaderWriter.DeserializeObject<List<Account>>(dbPath);

            foreach (Account a in lista)
            {
                if (Formatter.ParseName(userName) == a.User)
                {
                    lista.Remove(a);
                    xmlReaderWriter.SerializeObject<List<Account>>(lista, dbPath);
                    break;
                }
            }
        }

        public void ExecuteTransaction(double amount, string userName)
        {
            XmlReaderWriter xmlReaderWriter = new XmlReaderWriter();
            var lista = xmlReaderWriter.DeserializeObject<List<Account>>(dbPath);

            foreach (Account a in lista)
            {
                if (Formatter.ParseName(userName) == a.User && a.IsAccountActive)
                {
                    a.Money += amount;
                    xmlReaderWriter.SerializeObject<List<Account>>(lista, dbPath);
                    return;
                }
            }

            throw new Exception("Account not approved.");
        }

        public List<Account> GetAllAccountRequests()
        {
            XmlReaderWriter xmlReaderWriter = new XmlReaderWriter();
            var lista = xmlReaderWriter.DeserializeObject<List<Account>>(dbPath);

            return lista.Where(x => x.IsAccountActive == false).ToList();

        }

        public void MakeNewAccount(string userName)
        {
            XmlReaderWriter xmlReaderWriter = new XmlReaderWriter();
            var lista = xmlReaderWriter.DeserializeObject<List<Account>>(dbPath);

            lista.Add(new Account(userName, 0));

            xmlReaderWriter.SerializeObject<List<Account>>(lista, dbPath);
        }
    }
}
