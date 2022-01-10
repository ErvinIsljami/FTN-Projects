using EShoppy.Finantial_Module.Interfaces;
using EShoppy.NotificationModule.Interfaces;
using EShoppy.User_Module;
using EShoppy.User_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Finantial_Module.Implementation
{
    public class FinanceManager : IFinanceManager
    {
        private IClientManager clientManager;
        private IEmailSender emailSender;
        public FinanceManager()
        {
        }

        public FinanceManager(IClientManager clientManager, IEmailSender emailSender)
        {
            this.clientManager = clientManager;
            this.emailSender = emailSender;
        }
        public void AccountPayment(Guid accountID, double amount)
        {
            if(FinantialDB.Accounts.ContainsKey(accountID))
            {
                IAccount account = FinantialDB.Accounts[accountID];
                account.Balance += amount;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void AskCredit(Guid clientID, Guid creditID, int years, double amount)
        {
            IClient client = clientManager.GetClientByID(clientID);

            if(client == null)
            {
                throw new KeyNotFoundException();
            }

            var accounts = client.ListOfAccounts;
            
            if(accounts == null)
            {
                throw new KeyNotFoundException();
            }

            ICredit credit = FinantialDB.Credits.Values.FirstOrDefault(x => x.ID == creditID);

            if (credit == null)
            {
                throw new KeyNotFoundException();
            }

            IAccount account = accounts.FirstOrDefault(x => x.CreditAvailable == true);
            if(account == null)
            {
                emailSender.SendEmail((client as IUser).Email, "Credit declined", "Credit not available on any account.");
                throw new Exception("Credit not available on any account.");
            }

            IBank bank = FinantialDB.Banks.Values.FirstOrDefault(x => x.ListOfCredits.Contains(credit));

            if(bank == null)
            {
                emailSender.SendEmail((client as IUser).Email, "Credit declined", "No bank offers that credit.");
                throw new Exception("No bank offers that credit.");
            }

            if(bank.MaximumCreditYear < years)
            {
                emailSender.SendEmail((client as IUser).Email, "Credit declined", "Payment period too long.");
                throw new Exception("Payment period too long.");
            }

            if(amount > bank.MaximumCreditAmount)
            {
                emailSender.SendEmail((client as IUser).Email, "Credit declined", "Credit amount exceeds bank's maximum credit amount.");
                throw new Exception("Credit amount exceeds bank's maximum credit amount.");
            }

            emailSender.SendEmail((client as IUser).Email, "Credit aproved", "Congratulations. You have succesfully applied for credit.");

            account.Balance += amount;

            account.CreditPayment += amount * 1.05;
        }

        public double CheckBalance(Guid accountID)
        {
            if (FinantialDB.Accounts.ContainsKey(accountID))
            {
                IAccount account = FinantialDB.Accounts[accountID];
                return account.Balance;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public double Convert(ICurrency currency, double amount)
        {
            if(currency == null)
            {
                throw new ArgumentNullException();
            }

            return amount * currency.Value;
        }

        public void CreateAccount(string accountNumber, IBank bank, double balance, double creditPayment, bool creditAvailable)
        {
            if(string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException();
            }

            if(bank == null)
            {
                throw new ArgumentNullException();
            }

            balance = 0; 
            IAccount account = new Account(accountNumber, bank, balance, creditPayment, creditAvailable);

            FinantialDB.Accounts.Add(account.ID, account);
        }

        public void CreateBank(string name, string address, List<ICredit> credits, string accountPreffix, List<IAccount> accounts, double minCreditAmount, double maxCreditAmount, int maxCreditYear)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(accountPreffix))
            {
                throw new ArgumentNullException();
            }
        
            if(credits == null)
            {
                throw new ArgumentNullException();
            }

            if (accounts == null)
            {
                throw new ArgumentNullException();
            }

            IBank bank = new Bank(name, address, credits, accountPreffix, accounts, minCreditAmount, maxCreditAmount, maxCreditYear);

            FinantialDB.Banks.Add(bank.ID, bank);
        }

        public void CreateCredit(string name, double interest, ICurrency currency, IBank bank, int minYear)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }

            if(currency == null)
            {
                throw new ArgumentNullException();
            }

            if (bank == null)
            {
                throw new ArgumentNullException();
            }

            ICredit credit = new Credit(name, interest, currency, bank, minYear);

            FinantialDB.Credits.Add(credit.ID, credit);

        }

        public void CreditPayment(Guid accountID, double amount)
        {
            if (FinantialDB.Accounts.ContainsKey(accountID))
            {
                IAccount account = FinantialDB.Accounts[accountID];
                account.CreditPayment -= amount * 1.05;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public IAccount GetAccountByID(Guid accountID)
        {
            if(FinantialDB.Accounts.ContainsKey(accountID))
            {
                return FinantialDB.Accounts[accountID];
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
    }
}
