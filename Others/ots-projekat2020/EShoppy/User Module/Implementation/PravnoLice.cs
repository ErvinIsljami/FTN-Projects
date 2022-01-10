using EShoppy.Finantial_Module.Interfaces;
using EShoppy.Transactional_Module.Interfaces;
using EShoppy.User_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.User_Module.Implementation
{
    public class PravnoLice : IOrganization
    {
        private string name;
        private string pIB;
        private string place;
        private DateTime dateOfEst;
        private List<IAccount> listOfAccounts;
        private Guid iD;
        private List<ITransaction> listOfBuyingTransaction;
        private List<ITransaction> listOfSellingTransaction;
        private string email;

        public PravnoLice(string name, string pIB, string place, string email, DateTime dateOfEst, List<IAccount> listOfAccounts)
        {
            this.name = name;
            this.pIB = pIB;
            this.place = place;
            this.dateOfEst = dateOfEst;
            this.listOfAccounts = listOfAccounts;
            this.iD = Guid.NewGuid();
            this.email = email;
            ListOfBuyingTransaction = new List<ITransaction>();
            ListOfSellingTransaction = new List<ITransaction>();
        }

        public string Name { get => name; set => name = value; }
        public string PIB { get => pIB; set => pIB = value; }
        public string Place { get => place; set => place = value; }
        public DateTime DateOfEst { get => dateOfEst; set => dateOfEst = value; }
        public Guid ID { get => iD; set => iD = value; }
        public List<ITransaction> ListOfBuyingTransaction { get => listOfBuyingTransaction; set => listOfBuyingTransaction = value; }
        public List<ITransaction> ListOfSellingTransaction { get => listOfSellingTransaction; set => listOfSellingTransaction = value; }
        public List<IAccount> ListOfAccounts { get => listOfAccounts; set => listOfAccounts = value; }
        public string Email { get => email; set => email = value; }

    }
}
