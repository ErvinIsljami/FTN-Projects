using EShoppy.Finantial_Module.Interfaces;
using EShoppy.Transactional_Module.Interfaces;
using EShoppy.User_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.User_Module.Implementation
{
    public class FizickoLice : IUser
    {
        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private DateTime dateOfBirth;
        private string place;
        private List<IAccount> listOfAccounts;
        private Guid id;
        private List<ITransaction> listOfBuyingTransaction;
        private List<ITransaction> listOfSellingTransaction;

        public FizickoLice(string firstName, string lastName, string email, string password, DateTime dateOfBirth, string place, List<IAccount> listOfAccounts)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;
            this.dateOfBirth = dateOfBirth;
            this.place = place;
            this.listOfAccounts = listOfAccounts;
            id = Guid.NewGuid();
            ListOfBuyingTransaction = new List<ITransaction>();
            listOfSellingTransaction = new List<ITransaction>();
        }

        public FizickoLice()
        {
            id = Guid.NewGuid();
            ListOfBuyingTransaction = new List<ITransaction>();
            listOfSellingTransaction = new List<ITransaction>();
        }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public string Place { get => place; set => place = value; }
        public Guid ID { get => id; set => id = value; }
        public List<ITransaction> ListOfBuyingTransaction { get => listOfBuyingTransaction; set => listOfBuyingTransaction = value; }
        public List<ITransaction> ListOfSellingTransaction { get => listOfSellingTransaction; set => listOfSellingTransaction = value; }
        public List<IAccount> ListOfAccounts { get => listOfAccounts; set => listOfAccounts = value; }



    }
}
