using System;
using EShoppy.User_Module.Implementation;
using EShoppy.User_Module.Interfaces;
using System.Collections.Generic;
using EShoppy.Finantial_Module.Interfaces;
using EShoppy.User_Module;
using EShoppy.Finantial_Module.Implementation;
using EShoppy.Finantial_Module;
using System.Linq;
using EShoppy.NotificationModule.Implementation;
using Castle.Core.Smtp;
using NUnit.Framework;

namespace EBazaar.UnitTest.FinantialModule
{
    [TestFixture]
    public class FinanceManagerTest
    {
        [SetUp]
        public void Init()
        {
            FinantialDB.Accounts.Clear();
            FinantialDB.Banks.Clear();
            FinantialDB.Credits.Clear();
        }
        [Test]
        public void CreateAccount_ValidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Accounts.Clear();
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);

            Assert.IsTrue(FinantialDB.Accounts.Count == 1);
        }

        [Test]
        public void CreateAccount_InvalidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Accounts.Clear();

            Assert.Throws<ArgumentNullException>(() => financeManager.CreateAccount("412341234", null, 34534, 600, true));
        }

        [Test]
        public void CreateBank_ValidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Banks.Clear();
            financeManager.CreateBank("UniCredit", "Narodnog fronta 23", new List<ICredit>(), "odi", new List<IAccount>(), 1000, 100000, 2);

            Assert.IsTrue(FinantialDB.Banks.Count == 1);
        }

        [Test]
        public void CreateBank_InvalidTest()
        {
            FinanceManager financeManager = new FinanceManager();
            FinantialDB.Banks.Clear();

            Assert.Throws<ArgumentNullException>(() => financeManager.CreateBank("", "Narodnog fronta 23", null, "odi", new List<IAccount>(), 1000, 100000, 2));
        }

        [Test]
        public void CreateCredit_ValidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Credits.Clear();
            financeManager.CreateCredit("Dobar kredit", 0.04, new Currency(), new Bank(), 10);
            Assert.IsTrue(FinantialDB.Credits.Count == 1);
        }

        [Test]
        public void CreateCredit_InvalidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Credits.Clear();
            Assert.Throws<ArgumentNullException>(() => financeManager.CreateCredit("Dobar kredit", 0.04, null, null, 10));

        }

        [Test]
        public void GetAccountById_ValidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Accounts.Clear();
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            Guid targetId = FinantialDB.Accounts.Values.ToList()[0].ID;

            IAccount account = financeManager.GetAccountByID(targetId);

            Assert.IsTrue(account.ID == targetId);
        }

        [Test]
        public void GetAccountById_InvalidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Accounts.Clear();
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            Guid targetId = FinantialDB.Accounts.Values.ToList()[0].ID;

            Assert.Throws<KeyNotFoundException>(() => financeManager.GetAccountByID(Guid.NewGuid()));
        }

        [Test]
        public void AccountPayment_ValidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Accounts.Clear();
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            IAccount account = FinantialDB.Accounts.Values.ToList()[0];

            double oldBalance = account.Balance;
            financeManager.AccountPayment(account.ID, 100);

            Assert.IsTrue(account.Balance == oldBalance + 100);
        }

        [Test]
        public void AccountPayment_InvalidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Accounts.Clear();
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            IAccount account = FinantialDB.Accounts.Values.ToList()[0];

            double oldBalance = account.Balance;
            
            Assert.Throws<KeyNotFoundException>(() => financeManager.AccountPayment(Guid.NewGuid(), 100));

        }

        [Test]
        public void CreditPayment_ValidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Accounts.Clear();
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            IAccount account = FinantialDB.Accounts.Values.ToList()[0];

            double oldCredit = account.CreditPayment;
            financeManager.CreditPayment(account.ID, 100);

            Assert.IsTrue(account.CreditPayment == oldCredit - 100 * 1.05);
        }

        [Test]
        public void CreditPayment_InvalidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Accounts.Clear();
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            IAccount account = FinantialDB.Accounts.Values.ToList()[0];

            double oldCredit = account.CreditPayment;

            Assert.Throws<KeyNotFoundException>(() => financeManager.CreditPayment(Guid.NewGuid(), 100));
        }

        [Test]
        public void Convert_ValidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            double amount = 1000;
            ICurrency currency = FinantialDB.Currency["EUR"];
            double convertAmount = financeManager.Convert(currency, amount);

            Assert.AreEqual(convertAmount, amount * currency.Value);
        }

        [Test]
        public void Convert_InvalidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            double amount = 1000;
            ICurrency currency = FinantialDB.Currency["EUR"];

            Assert.Throws<ArgumentNullException>(() => financeManager.Convert(null, amount));
        }

        [Test]
        public void CheckBalance_ValidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Accounts.Clear();
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            IAccount account = FinantialDB.Accounts.Values.ToList()[0];

            account.Balance = 500;
            double balance = account.Balance;
            double balance2 = financeManager.CheckBalance(account.ID);

            Assert.AreEqual(balance, balance2);
        }

        [Test]
        public void CheckBalance_InvalidTest()
        {
            FinanceManager financeManager = new FinanceManager();

            FinantialDB.Accounts.Clear();
            financeManager.CreateAccount("412341234", new Bank(), 34534, 600, true);
            IAccount account = FinantialDB.Accounts.Values.ToList()[0];

            Assert.Throws<KeyNotFoundException>(() => financeManager.CheckBalance(Guid.NewGuid()));
        }

        [Test]
        public void AskCredit_InvalidNotFoundTest()
        {
            FinanceManager financeManager = new FinanceManager(new ClientManager(), new EmailSenderMock());

            Assert.Throws<KeyNotFoundException>(() => financeManager.AskCredit(Guid.NewGuid(), Guid.NewGuid(), 123, 50000));
        }

        [Test]
        public void AskCredit_CreditNotAvailableTest()
        {
            IClientManager clientManager = new ClientManager();
            FinanceManager financeManager = new FinanceManager(clientManager, new EmailSenderMock());

            ShoppingClient.Clients.Clear();
            clientManager.RegisterUser("Marko", "Sinisic", "markos@gmail.com", "markos123", "Novi Sad", new DateTime(1990, 5, 25), new List<IAccount>());
            IClient client = ShoppingClient.Clients[0];

            financeManager.CreateAccount("f231rf34f34f3f4", new Bank(), 10000, 0, false);
            IAccount account = FinantialDB.Accounts.Values.ToList()[0];
            
            financeManager.CreateBank("Raiffeisen", "Cara Lazara 55", new List<ICredit>(), "din", new List<IAccount>() { account }, 1000, 1000000, 100);
            IBank bank = FinantialDB.Banks.Values.ToList()[0];

            financeManager.CreateCredit("kredit", 0.05, FinantialDB.Currency["EUR"], bank, 10);
            ICredit credit = FinantialDB.Credits.Values.ToList()[0];

            bank.ListOfCredits.Add(credit);

            Assert.Throws<Exception>(() => financeManager.AskCredit(client.ID, credit.ID, 123, 50000), "Credit not available on any account.");
        }

        [Test]
        public void AskCredit_BankNoCreditTest()
        {
            IClientManager clientManager = new ClientManager();
            FinanceManager financeManager = new FinanceManager(clientManager, new EmailSenderMock());

            ShoppingClient.Clients.Clear();
            clientManager.RegisterUser("Marko", "Sinisic", "markos@gmail.com", "markos123", "Novi Sad", new DateTime(1990, 5, 25), new List<IAccount>());
            IClient client = ShoppingClient.Clients[0];

            financeManager.CreateAccount("f231rf34f34f3f4", new Bank(), 10000, 0, false);
            IAccount account = FinantialDB.Accounts.Values.ToList()[0];

            financeManager.CreateBank("Raiffeisen", "Cara Lazara 55", new List<ICredit>(), "din", new List<IAccount>() { account }, 1000, 1000000, 100);
            IBank bank = FinantialDB.Banks.Values.ToList()[0];

            financeManager.CreateCredit("kredit", 0.05, FinantialDB.Currency["EUR"], bank, 10);
            ICredit credit = FinantialDB.Credits.Values.ToList()[0];

            Assert.Throws<Exception>(() => financeManager.AskCredit(client.ID, credit.ID, 123, 50000), "No bank offers that credit.");
        }

        [Test]
        public void AskCredit_PeriodTooLongTest()
        {
            IClientManager clientManager = new ClientManager();
            FinanceManager financeManager = new FinanceManager(clientManager, new EmailSenderMock());

            ShoppingClient.Clients.Clear();
            clientManager.RegisterUser("Marko", "Sinisic", "markos@gmail.com", "markos123", "Novi Sad", new DateTime(1990, 5, 25), new List<IAccount>());
            IClient client = ShoppingClient.Clients[0];

            financeManager.CreateAccount("f231rf34f34f3f4", new Bank(), 10000, 0, false);
            IAccount account = FinantialDB.Accounts.Values.ToList()[0];

            financeManager.CreateBank("Raiffeisen", "Cara Lazara 55", new List<ICredit>(), "din", new List<IAccount>() { account }, 1000, 1000000, 100);
            IBank bank = FinantialDB.Banks.Values.ToList()[0];

            financeManager.CreateCredit("kredit", 0.05, FinantialDB.Currency["EUR"], bank, 10);
            ICredit credit = FinantialDB.Credits.Values.ToList()[0];

            bank.ListOfCredits.Add(credit);

            
            Assert.Throws<Exception>(() => financeManager.AskCredit(client.ID, credit.ID, 123, 50000), "Payment period too long.");
        }


        [Test]
        public void AskCredit_AmountExceeded()
        {
            IClientManager clientManager = new ClientManager();
            FinanceManager financeManager = new FinanceManager(clientManager, new EmailSenderMock());

            ShoppingClient.Clients.Clear();
            clientManager.RegisterUser("Marko", "Sinisic", "markos@gmail.com", "markos123", "Novi Sad", new DateTime(1990, 5, 25), new List<IAccount>());
            IClient client = ShoppingClient.Clients[0];

            financeManager.CreateAccount("f231rf34f34f3f4", new Bank(), 10000, 0, false);
            IAccount account = FinantialDB.Accounts.Values.ToList()[0];

            financeManager.CreateBank("Raiffeisen", "Cara Lazara 55", new List<ICredit>(), "din", new List<IAccount>() { account }, 1000, 10000, 100);
            IBank bank = FinantialDB.Banks.Values.ToList()[0];

            financeManager.CreateCredit("kredit", 0.05, FinantialDB.Currency["EUR"], bank, 10);
            ICredit credit = FinantialDB.Credits.Values.ToList()[0];

            bank.ListOfCredits.Add(credit);

            Assert.Throws<Exception>(() => financeManager.AskCredit(client.ID, credit.ID, 123, 50000), "Credit amount exceeds bank's maximum credit amount.");
        }

        [Test]
        public void AskCredit_Success()
        {
            IClientManager clientManager = new ClientManager();
            FinanceManager financeManager = new FinanceManager(clientManager, new EmailSenderMock());

            ShoppingClient.Clients.Clear();
            clientManager.RegisterUser("Marko", "Sinisic", "markos@gmail.com", "markos123", "Novi Sad", new DateTime(1990, 5, 25), new List<IAccount>());
            IClient client = ShoppingClient.Clients[0];

            financeManager.CreateAccount("f231rf34f34f3f4", new Bank(), 10000, 0, true);
            IAccount account = FinantialDB.Accounts.Values.ToList()[0];
            client.ListOfAccounts.Add(account);

            financeManager.CreateBank("Raiffeisen", "Cara Lazara 55", new List<ICredit>(), "din", new List<IAccount>() { account }, 1000, 100000, 100);
            IBank bank = FinantialDB.Banks.Values.ToList()[0];

            financeManager.CreateCredit("kredit", 0.05, FinantialDB.Currency["EUR"], bank, 10);
            ICredit credit = FinantialDB.Credits.Values.ToList()[0];

            bank.ListOfCredits.Add(credit);

            financeManager.AskCredit(client.ID, credit.ID, 20, 50000);

            Assert.IsTrue(account.CreditPayment > 0);
        }

    }
}
