using EShoppy.Finantial_Module.Interfaces;
using EShoppy.Logistic_Module.Interfaces;
using EShoppy.NotificationModule.Interfaces;
using EShoppy.Sale_Module;
using EShoppy.Sale_Module.Interfaces;
using EShoppy.Transactional_Module.Interfaces;
using EShoppy.User_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Transactional_Module.Implementation
{
    public class TransactionManager : ITransactionManager
    {
        private IClientManager clientManager;
        private ISalesManager salesManager;
        private IFinanceManager financeManager;
        private IEmailSender emailSender;

        public TransactionManager(IClientManager clientManager, ISalesManager salesManager, IFinanceManager financeManager, IEmailSender emailSender)
        {
            this.clientManager = clientManager;
            this.salesManager = salesManager;
            this.financeManager = financeManager;
            this.emailSender = emailSender;
        }
        public void CreateTransaction(Guid customerID, Guid companyID, Guid offerID, Guid transactionTypeID, int rating)
        {
            IClient customer = clientManager.GetClientByID(customerID);
            if(customer == null)
            {
                throw new KeyNotFoundException("Client does not exists in database.");
            }

            IClient company = clientManager.GetClientByID(companyID);
            if (company == null)
            {
                throw new KeyNotFoundException("Company does not exists in database.");
            }

            IOffer offer = ShoppingOffers.Offers.Values.FirstOrDefault(x => x.ID == offerID);
            if (offer == null)
            {
                throw new KeyNotFoundException("Offer does not exists in database.");
            }

            ITransaction transaction = new Transaction();
            transaction.Price = offer.Price;
            transaction.Rating = rating;
            transaction.Customer = customer;
            transaction.Offer = offer;
            transaction.Date = DateTime.Now;
            transaction.TransactionType = ShoppingTransaction.TransactionTypes.Values.FirstOrDefault(x => x.ID == transactionTypeID);

            if(transaction.TransactionType == null)
            {
                throw new KeyNotFoundException("Transaction type does not exists in database.");
            }

            if(((transaction.Date.Year - offer.SubmitionDate.Year) * 12) + transaction.Date.Month - offer.SubmitionDate.Month > 6)
            {
                transaction.Discount += 12;
            }
            if(transaction.Date.Month == 1 || transaction.Date.Month == 12)
            {
                transaction.Discount += 5;
            }
            if(offer.ListOfProducts.Count > 3)
            {
                transaction.Discount += 5;
            }

            IAccount customerAccount = customer.ListOfAccounts[0];
            IAccount companyAccount = company.ListOfAccounts[0];
            double customerBalance = financeManager.CheckBalance(customerAccount.ID);

            if(transaction.TransactionType.Description == "full")
            {
                if(customerBalance > transaction.Price)
                {
                    double price = transaction.Price * (1 - transaction.Discount / 100);
                    financeManager.AccountPayment(customerAccount.ID, price * -1);
                    financeManager.AccountPayment(companyAccount.ID, price);

                    ShoppingTransaction.Transactions.Add(transaction.ID, transaction);
                    customer.ListOfBuyingTransaction.Add(transaction);
                    company.ListOfSellingTransaction.Add(transaction);
                    
                    emailSender.SendEmail(customer.Email, "Transaction success!", "Transaction succesfully finished.");
                    emailSender.SendEmail(company.Email, "Transaction success!", "Transaction succesfully finished.");
                }    
            }
            else
            {
                double price = transaction.Price * (1 - transaction.Discount / 100);
                financeManager.AccountPayment(customerID, price / 3 * -1);
                financeManager.AccountPayment(companyID, price / 3);
                
                ShoppingTransaction.Transactions.Add(transaction.ID, transaction);
                customer.ListOfBuyingTransaction.Add(transaction);
                company.ListOfSellingTransaction.Add(transaction);

                emailSender.SendEmail(customer.Email, "Transaction success!", "Transaction succesfully finished.");
                emailSender.SendEmail(company.Email, "Transaction success!", "Transaction succesfully finished.");
            }
        }
    }
}
