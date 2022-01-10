using EShoppy.Finantial_Module;
using EShoppy.Finantial_Module.Implementation;
using EShoppy.Finantial_Module.Interfaces;
using EShoppy.Logistic_Module.Interfaces;
using EShoppy.NotificationModule.Interfaces;
using EShoppy.Sale_Module;
using EShoppy.Sale_Module.Implementation;
using EShoppy.Sale_Module.Interfaces;
using EShoppy.Transactional_Module;
using EShoppy.Transactional_Module.Implementation;
using EShoppy.Transactional_Module.Interfaces;
using EShoppy.User_Module;
using EShoppy.User_Module.Implementation;
using EShoppy.User_Module.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazaar.UnitTest.TransactionModule
{
    [TestFixture]
    public class TransactionManagaerTests
    {
        [Test]
        public void CreateTransaction_InvalidCustomerID()
        {
            IClientManager clientManager = Substitute.For<IClientManager>();
            ISalesManager salesManager = Substitute.For<ISalesManager>();
            IFinanceManager financeManager = Substitute.For<IFinanceManager>();
            IEmailSender emailSender = Substitute.For<IEmailSender>();

            ITransactionManager transactionManager = new TransactionManager(clientManager, salesManager, financeManager, emailSender);

            Assert.Throws<KeyNotFoundException>(() => transactionManager.CreateTransaction(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 5), "Client does not exists in database.");
        }

        [Test]
        public void CreateTransaction_InvalidCompanyID()
        {
            IEmailSender emailSender = Substitute.For<IEmailSender>();
            IFinanceManager financeManager = new FinanceManager();
            IClientManager clientManager = new ClientManager();
            ISalesManager salesManager = new SalesManager();

            clientManager.RegisterUser("Pera", "Peric", "peraperic@gmail.com", "peraPeric123", "Novi Sad", new DateTime(1992, 5, 6), new List<IAccount>());
            IClient client = ShoppingClient.Clients[0];

            ITransactionManager transactionManager = new TransactionManager(clientManager, salesManager, financeManager, emailSender);
            Assert.Throws<KeyNotFoundException>(() => transactionManager.CreateTransaction(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 5), "Company does not exists in database.");
        }

        [Test]
        public void CreateTransaction_InvalidOfferID()
        {
            IEmailSender emailSender = Substitute.For<IEmailSender>();
            IFinanceManager financeManager = new FinanceManager();
            IClientManager clientManager = new ClientManager();
            ISalesManager salesManager = new SalesManager();

            clientManager.RegisterUser("Pera", "Peric", "peraperic@gmail.com", "peraPeric123", "Novi Sad", new DateTime(1992, 5, 6), new List<IAccount>());
            clientManager.RegisterOrg("Prodavnica", "q234ffsad", "Novi Sad", "prodavnica@gmail.com", new DateTime(2010, 1, 1), new List<IAccount>());
            IClient client = ShoppingClient.Clients.FirstOrDefault(x => x is FizickoLice);
            IClient company = ShoppingClient.Clients.FirstOrDefault(x => x is PravnoLice);

            ITransactionManager transactionManager = new TransactionManager(clientManager, salesManager, financeManager, emailSender);
            Assert.Throws<KeyNotFoundException>(() => transactionManager.CreateTransaction(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 5), "Offer does not exists in database.");
        }

        [Test]
        [TestCase(2020, 1, 1, 5, "full")]
        public void CreateTransaction_ValidTest(int year, int month, int day, int rating, string transcationTypeKey)
        {
            IEmailSender emailSender = Substitute.For<IEmailSender>();
            IFinanceManager financeManager = new FinanceManager();
            IClientManager clientManager = new ClientManager();
            ISalesManager salesManager = new SalesManager();
            ShoppingClient.Clients.Clear();
            ShoppingOffers.Offers.Clear();
            clientManager.RegisterUser("Pera", "Peric", "peraperic@gmail.com", "peraPeric123", "Novi Sad", new DateTime(1992, 5, 6), new List<IAccount>());
            clientManager.RegisterOrg("Prodavnica", "q234ffsad", "Novi Sad", "prodavnica@gmail.com", new DateTime(2010, 1, 1), new List<IAccount>());
            IClient client = ShoppingClient.Clients.FirstOrDefault(x => x is FizickoLice);
            IClient company = ShoppingClient.Clients.FirstOrDefault(x => x is PravnoLice);

            IProduct product = new Product("Product", "Description", 3000, 1);
            salesManager.CreateOffer(company, new List<IProduct>() { product }, new List<ITransport>());
            financeManager.CreateAccount("41234123453425", new Bank(), 100000, 0, false);
            financeManager.CreateAccount("456345634567456", new Bank(), 100000, 0, false);
            IAccount customerAccount = FinantialDB.Accounts.Values.ToList()[0];
            IAccount companyAccount = FinantialDB.Accounts.Values.ToList()[1];
            client.ListOfAccounts.Add(customerAccount);
            customerAccount.Balance = 1000000;
            company.ListOfAccounts.Add(companyAccount);
            companyAccount.Balance = 2000000;

            IOffer offer = ShoppingOffers.Offers.Values.ToList()[0];
            offer.SubmitionDate = new DateTime(year, month, day);
            
            ITransactionManager transactionManager = new TransactionManager(clientManager, salesManager, financeManager, emailSender);
            ITransactionType transactionType = ShoppingTransaction.TransactionTypes[transcationTypeKey];
            
            transactionManager.CreateTransaction(client.ID, company.ID, offer.ID, transactionType.ID, rating);

            Assert.IsTrue(client.ListOfBuyingTransaction.Count == 1);
            Assert.IsTrue(company.ListOfSellingTransaction.Count == 1);
        }
    }
}
