using System;
using EShoppy.User_Module.Implementation;
using EShoppy.User_Module.Interfaces;
using System.Collections.Generic;
using EShoppy.Finantial_Module.Interfaces;
using EShoppy.User_Module;
using EShoppy.Finantial_Module.Implementation;
using EShoppy.User_Module.Implementation.Mocks;
using EShoppy.NotificationModule.Implementation;
using EShoppy.Finantial_Module;
using System.Linq;
using EShoppy.NotificationModule.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace EBazaar.UnitTest.UserModule
{
    [TestFixture]
    public class ClientManagerTests
    {
        public ClientManagerTests()
        {

        }
        [Test]
        public void RegisterUser_ValidTest()
        {
            IClientManager clientManager = new ClientManager();

            ShoppingClient.Clients.Clear();
            clientManager.RegisterUser("Pera", "Peric", "peraperic@gmail.com", "peraPeric123", "Novi Sad", new DateTime(1992, 5, 6), new List<IAccount>());

            Assert.IsTrue(ShoppingClient.Clients.Count == 1);
        }

        [Test]
        public void RegisterUser_InvalidTest()
        {
            IClientManager clientManager = new ClientManager();

            ShoppingClient.Clients.Clear();
            
            Assert.Throws<ArgumentNullException>(() => clientManager.RegisterUser("Pera", "", "peraperic@gmail.com", "peraPeric123", "Novi Sad", new DateTime(1992, 5, 6), new List<IAccount>()));
        }

        [Test]
        public void RegisterOrg_ValidTest()
        {
            IClientManager clientManager = new ClientManager();
            ShoppingClient.Clients.Clear();
            clientManager.RegisterOrg("Prodavnica", "q234ffsad", "Novi Sad", "prodavnica@gmail.com", new DateTime(2010, 1, 1), new List<IAccount>());

            Assert.IsTrue(ShoppingClient.Clients.Count == 1);
        }

        [Test]
        public void RegisterOrg_InvalidTest()
        {
            IClientManager clientManager = new ClientManager();
            ShoppingClient.Clients.Clear();
            
            Assert.Throws<ArgumentNullException>(() => clientManager.RegisterOrg("Prodavnica", null, "Novi Sad", "mail@gmail.com", new DateTime(2010, 1, 1), new List<IAccount>()));
        }

        [Test]
        public void ChangeUserAccount_ValidTest()
        {
            IClientManager clientManager = new ClientManager();
            ShoppingClient.Clients.Clear();
            clientManager.RegisterUser("Pera", "Peric", "peraperic@gmail.com", "peraPeric123", "Novi Sad", new DateTime(1992, 5, 6), new List<IAccount>());

            List<IAccount> newList = new List<IAccount>();

            IUser user = ShoppingClient.Clients[0] as IUser;

            clientManager.ChangeUserAccount(user, newList);

            Assert.AreEqual(newList, user.ListOfAccounts);
        }

        [Test]
        public void ChangeUserAccount_InvalidTest()
        {
            IClientManager clientManager = new ClientManager();
            ShoppingClient.Clients.Clear();
            clientManager.RegisterUser("Pera", "Peric", "peraperic@gmail.com", "peraPeric123", "Novi Sad", new DateTime(1992, 5, 6), new List<IAccount>());

            List<IAccount> newList = new List<IAccount>();

            IUser user = ShoppingClient.Clients[0] as IUser;
            IUser user2 = new FizickoLice("Pera", "Peric", "peraperic@gmail.com", "peraPeric123", new DateTime(1992, 5, 6), "Novi Sad", new List<IAccount>());

            Assert.Throws<KeyNotFoundException>(() => clientManager.ChangeUserAccount(user2, newList));
        }

        [Test]
        public void ChangeOrgAccount_ValidTest()
        {
            IClientManager clientManager = new ClientManager();
            ShoppingClient.Clients.Clear();
            clientManager.RegisterOrg("Prodavnica", "q234ffsad", "Novi Sad", "prodavnica@gmail.com", new DateTime(2010, 1, 1), new List<IAccount>());

            List<IAccount> newList = new List<IAccount>();
            IOrganization org = ShoppingClient.Clients[0] as IOrganization;
            clientManager.ChangeOrgAccount(org, newList);

            Assert.AreEqual(newList, org.ListOfAccounts);
        }

        [Test]
        public void ChangeOrgAccount_InvalidTest()
        {
            IClientManager clientManager = new ClientManager();
            ShoppingClient.Clients.Clear();
            clientManager.RegisterOrg("Prodavnica", "q234ffsad", "Novi Sad", "prodavnica@gmail.com", new DateTime(2010, 1, 1), new List<IAccount>());

            List<IAccount> newList = new List<IAccount>();
            IOrganization org = ShoppingClient.Clients[0] as IOrganization;
            IOrganization org2 = new PravnoLice("fasdfasd", "frfewrf", "ASDFASDF", "afsfdas@gmail.com", DateTime.Now, null);

            
            Assert.Throws<KeyNotFoundException>(() => clientManager.ChangeOrgAccount(org2, newList));
        }

        [Test]
        public void GetClientById_ValidTest()
        {
            IClientManager clientManager = new ClientManager();

            ShoppingClient.Clients.Clear();
            clientManager.RegisterUser("Pera", "Peric", "peraperic@gmail.com", "peraPeric123", "Novi Sad", new DateTime(1992, 5, 6), new List<IAccount>());

            Guid id = ShoppingClient.Clients[0].ID;
            IClient client = clientManager.GetClientByID(id);

            Assert.AreEqual(client.ID, id);
        }

        [Test]
        public void AddFunds_InvalidArgumentsTest()
        {
            IClientManager clientManager = new ClientManager();
            
            Assert.Throws<ArgumentNullException>(() => clientManager.AddFunds(null, Guid.NewGuid(), 123, null));
        }

        [Test]
        [TestCase(5000)]
        [TestCase(1000)]
        public void AddFunds_ValidTestFizickoLiceRacun(double amount)
        {
            IFinanceManager financeManager = new FinanceManager();
            IClientManager clientManager = new ClientManager(financeManager, new EmailSenderMock());
            FinantialDB.Accounts.Clear();
            financeManager.CreateAccount("6345634563456", new Bank(), 1500, 0, false);
            var account = FinantialDB.Accounts.Values.ToList()[0];
            IClient client = new FizickoLice("Marko", "Markovic","markom@gmail.com", "marko123", DateTime.Now, "Novi Sad", new List<IAccount>() { account });
            ICurrency currency = FinantialDB.Currency["EUR"];

            double oldAmount = account.Balance;

            clientManager.AddFunds(client, account.ID, amount, currency);

            Assert.AreEqual(oldAmount + amount * currency.Value, account.Balance);
        }

        [Test]
        [TestCase(50000)]
        [TestCase(10000)]
        public void AddFunds_ValidTestFizickoLiceKredit(double amount)
        {
            IFinanceManager financeManager = new FinanceManager();
            IClientManager clientManager = new ClientManager(financeManager, new EmailSenderMock());
            financeManager.CreateAccount("6345634563456", new Bank(), 1500, 0, true);
            var account = FinantialDB.Accounts.Values.ToList()[0];
            IClient client = new PravnoLice("dasdfasdf", "r34f324f", "Novi Sad", "fasdfaf@gmail.com", DateTime.Now, new List<IAccount>() { account });
            ICurrency currency = FinantialDB.Currency["EUR"];

            double oldAmount = account.CreditPayment;
            clientManager.AddFunds(client, account.ID, amount, currency);

            Assert.IsTrue(oldAmount > account.CreditPayment);
        }

        [Test]
        [TestCase(50000)]
        [TestCase(10000)]
        public void AddFunds_InvalidTestClientDb(double amount)
        {
            IFinanceManager financeManager = new FinanceManager();
            IClientManager clientManager = new ClientManager(financeManager, new EmailSenderMock());
            financeManager.CreateAccount("6345634563456", new Bank(), 1500, 0, true);
            var account = FinantialDB.Accounts.Values.ToList()[0];
            IClient client = new PravnoLice("dasdfasdf", "r34f324f", "Novi Sad", "fasgqegr@gmail.com", DateTime.Now, new List<IAccount>());
            ICurrency currency = FinantialDB.Currency["EUR"];

            double oldAmount = account.CreditPayment;

            Assert.Throws<KeyNotFoundException>(() => clientManager.AddFunds(client, account.ID, amount, currency), "Account not found in clients database.");
        }

        [Test]
        [TestCase(50000)]
        [TestCase(10000)]
        public void AddFunds_InvalidTestDb(double amount)
        {
            IFinanceManager financeManager = new FinanceManager();
            IClientManager clientManager = new ClientManager(financeManager, new EmailSenderMock());
            //financeManager.CreateAccount("6345634563456", new Bank(), 1500, 0, true);
            //var account = FinantialDB.Accounts.Values.ToList()[0];
            IAccount account = new Account("6345634563456", new Bank(), 1500, 0, true);
            IClient client = new PravnoLice("dasdfasdf", "r34f324f", "Novi Sad", "pravnoLice@gmail.com", DateTime.Now, new List<IAccount>() { account });
            ICurrency currency = FinantialDB.Currency["EUR"];

            double oldAmount = account.CreditPayment;
            
            Assert.Throws<KeyNotFoundException>(() => clientManager.AddFunds(client, account.ID, amount, currency), "Account not found in database.");
        }

        [Test]
        [TestCase(500)]
        [TestCase(100)]
        public void AddFunds_ValidTestPravnoLiceKredit(double amount)
        {
            IFinanceManager financeManager = new FinanceManager();
            IEmailSender emailSender = Substitute.For<IEmailSender>();
            IClientManager clientManager = new ClientManager(financeManager, emailSender);
            FinantialDB.Accounts.Clear();
            financeManager.CreateAccount("6345634563456", new Bank(), 150000, 500000, true);
            var account = FinantialDB.Accounts.Values.ToList()[0];
            IClient client = new PravnoLice("dasdfasdf", "r34f324f", "Novi Sad", "fasfqwef@gmail.com", DateTime.Now, new List<IAccount>() { account });
            ICurrency currency = FinantialDB.Currency["EUR"];


            double oldAmount = account.CreditPayment;

            clientManager.AddFunds(client, account.ID, amount, currency);
            emailSender.Received();
            Assert.IsTrue(oldAmount > account.CreditPayment);
        }

        [Test]
        [TestCase(500)]
        [TestCase(1000)]
        public void AddFunds_ValidTestPravnoLiceRacun(double amount)
        {
            IFinanceManager financeManager = new FinanceManager();
            IClientManager clientManager = new ClientManager(financeManager, new EmailSenderMock());
            FinantialDB.Accounts.Clear();
            financeManager.CreateAccount("6345634563456", new Bank(), 1500, 0, false);
            var account = FinantialDB.Accounts.Values.ToList()[0];
            IClient client = new PravnoLice("dasdfasdf", "r34f324f", "Novi Sad", "asoifjasodifj@gmail.com", DateTime.Now, new List<IAccount>() { account });
            ICurrency currency = FinantialDB.Currency["EUR"];


            double oldAmount = account.Balance;

            clientManager.AddFunds(client, account.ID, amount, currency);

            Assert.AreEqual(oldAmount + amount * currency.Value, account.Balance);

        }

        [Test]
        [TestCase(5000)]
        [TestCase(10)]
        [TestCase(0)]
        public void AddFunds_InvalidTestPravnoLiceRacun(double amount)
        {
            IFinanceManager financeManager = new FinanceManagerMock();
            IClientManager clientManager = new ClientManager(financeManager, new EmailSenderMock());
            IClient client = new PravnoLice("dasdfasdf", "r34f324f", "Novi Sad", "fasfqf@gmail.com", DateTime.Now, new List<IAccount>());
          
            Assert.Throws<Exception>(() => clientManager.AddFunds(client, Guid.NewGuid(), amount, new CurrencyMock()), "Amount has to be greater than 10000.");
        }

    }
}
