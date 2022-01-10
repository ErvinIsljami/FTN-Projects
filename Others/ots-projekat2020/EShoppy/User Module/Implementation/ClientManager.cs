using EShoppy.Finantial_Module.Interfaces;
using EShoppy.NotificationModule.Interfaces;
using EShoppy.Transactional_Module.Interfaces;
using EShoppy.User_Module.Implementation.Mocks;
using EShoppy.User_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.User_Module.Implementation
{
    public class ClientManager : IClientManager
    {
        private IFinanceManager financeManager;
        private IEmailSender emailSender;
        public ClientManager(IFinanceManager financeManager, IEmailSender emailSender)
        {
            this.financeManager = financeManager;
            this.emailSender = emailSender;
        }

        public ClientManager()
        {

        }
        
        public void AddFunds(IClient client, Guid accountID, double amount, ICurrency currency)
        {
            if(client == null)
            {
                throw new ArgumentNullException();
            }

            if(currency == null)
            {
                throw new ArgumentNullException();
            }

            List<IAccount> accounts = client.ListOfAccounts;

            amount = financeManager.Convert(currency, amount);
            if(client is PravnoLice && amount < 10000)
            {
                throw new Exception("Amount has to be greater than 10000.");
            }


            var clientAccount = client.ListOfAccounts.FirstOrDefault(x => x.ID == accountID);

            if (clientAccount == null)
            {
                throw new KeyNotFoundException("Account not found in clients database.");
            }
            var dbAccount = financeManager.GetAccountByID(accountID);

            if(dbAccount == null)
            {
                throw new KeyNotFoundException("Account not found in database.");
            }
            
            if(clientAccount.CreditAvailable)
            {
                financeManager.CreditPayment(accountID, amount);
                emailSender.SendEmail(client.Email, "Rata uspesno placena.", "Rata za kredit uspesno placena.\nIznos koji je uplacen je: " + amount);
            }
            else
            {
                financeManager.AccountPayment(accountID, amount);
                emailSender.SendEmail(client.Email, "Uplata uspesna.", "Uspesno uplacen novac na racun.\nIznos koji je uplacen je: " + amount);
            }
        }

        public void ChangeOrgAccount(IOrganization organization, List<IAccount> accounts)
        {
            if(organization == null)
            {
                throw new ArgumentNullException();
            }

            if(accounts == null)
            {
                throw new ArgumentNullException();
            }

            IOrganization targetOrganization = ShoppingClient.Clients.FirstOrDefault(x => x.ID == organization.ID) as IOrganization;

            if(targetOrganization == null)
            {
                throw new KeyNotFoundException("Organizacija sa datim id-ijem ne postoji u bazi.");
            }

            targetOrganization.ListOfAccounts = accounts;
        }

        public void ChangeUserAccount(IUser user, List<IAccount> accounts)
        {
            if(user == null)
            {
                throw new ArgumentNullException();
            }

            if(accounts == null)
            {
                throw new ArgumentNullException();
            }

            IUser targetUser = ShoppingClient.Clients.FirstOrDefault(x => x.ID == user.ID) as IUser;

            
            if(targetUser == null)
            {
                throw new KeyNotFoundException("Korisnik sa datim id-ijem ne postoji u bazi");
            }

            targetUser.ListOfAccounts = accounts;
        }

        public IClient GetClientByID(Guid clientID)
        {
            if(clientID == null)
            {
                throw new ArgumentNullException();
            }

            IClient targetClient = ShoppingClient.Clients.FirstOrDefault(x => x.ID == clientID);

            return targetClient;
        }

        public void RegisterOrg(string name, string PIB, string place, string email, DateTime dateOfBirth, List<IAccount> accounts)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(PIB))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(place))
            {
                throw new ArgumentNullException();
            }

            if (dateOfBirth == null)
            {
                throw new ArgumentNullException();
            }

            if (accounts == null)
            {
                throw new ArgumentNullException();
            }

            IClient organizacija = new PravnoLice(name, PIB, place, email, dateOfBirth, accounts);
            ShoppingClient.Clients.Add(organizacija);
        }

        public void RegisterUser(string firstName, string lastName, string email, string password, string place, DateTime dateOfBirth, List<IAccount> accounts)
        {
            if(string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(place))
            {
                throw new ArgumentNullException();
            }

            if(dateOfBirth == null)
            {
                throw new ArgumentNullException();
            }

            if (accounts == null)
            {
                throw new ArgumentNullException();
            }

            IClient novoLice = new FizickoLice(firstName, lastName, email, password, dateOfBirth, place, accounts);
            ShoppingClient.Clients.Add(novoLice);

        }

        public List<ITransaction> SearchHistory(IClient client, DateTime? date, int? transType, int? rating)
        {
            if (client == null)
            {
                throw new ArgumentNullException();
            }

            IClient targetClient = ShoppingClient.Clients.FirstOrDefault(x => x.ID == client.ID);

            if(transType == null || transType == 0)
            {
                return targetClient.ListOfBuyingTransaction.Where(x => x.Date > date && x.Rating > rating).ToList();
            }
            else
            {
                return targetClient.ListOfSellingTransaction.Where(x => x.Date > date && x.Rating > rating).ToList();
            }
        }
    }
}
