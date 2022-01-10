using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributedTransaction
{
    public class TransactionCoordinator : IPurchase
    {
        private static IBank bankProxy;
        private static IBookstore bookstoreProxy;
        private string bankAddress;
        private string bookstoreAddress;

        private static Func<string, string> GetServiceAddress;

        public TransactionCoordinator() { }

        public TransactionCoordinator(string bankAddress, string bookstoreAddress, Func<string, string> getServiceAddress)
        {
            this.bankAddress = bankAddress;
            this.bookstoreAddress = bookstoreAddress;
            GetServiceAddress = getServiceAddress;
            ConnectWithBank(bankAddress);   
            ConnectWithBookstore(bookstoreAddress);
        }
        
        public void ConnectWithBank(string bankAddress)
        {
            if (!String.IsNullOrWhiteSpace(bankAddress))
            {
                var binding = new NetTcpBinding();
                var endpoint = new EndpointAddress(new Uri($"net.tcp://{bankAddress}/Bank"));
                var channelFactory = new ChannelFactory<IBank>(binding);
                bankProxy = channelFactory.CreateChannel(endpoint);
                Console.WriteLine("Connected with bank.");
            }
            else
            {
                Console.WriteLine("Unable to connect to bank. No address.");
            }
        }

        public void ConnectWithBookstore(string bookstoreAddress)
        {
            if (!String.IsNullOrWhiteSpace(bookstoreAddress))
            {
                var binding = new NetTcpBinding();
                var endpoint = new EndpointAddress(new Uri($"net.tcp://{bookstoreAddress}/Bookstore"));
                var channelFactory = new ChannelFactory<IBookstore>(binding);
                bookstoreProxy = channelFactory.CreateChannel(endpoint);
                Console.WriteLine("Connected with bookstore.");
            }
            else
            {
                Console.WriteLine("Unable to connect with bookstore. No address.");   
            }
        }


        public bool OrderItem(string bookId, string userId, int numOfBooksToBuy)
        {
            Console.WriteLine("Transaction in progress...");
            double bookPrice;
            try
            {
                bookPrice = bookstoreProxy.GetItemPrice(bookId);
            }
            catch (Exception)
            {
                bookstoreProxy = null;
                bookstoreAddress = GetServiceAddress("Bookstore");
                ConnectWithBookstore(bookstoreAddress);
                bookPrice = bookstoreProxy.GetItemPrice(bookId);
            }
            
            double priceToPay = numOfBooksToBuy * bookPrice;

            try
            {
                bankProxy.EnlistMoneyTransfer(userId, priceToPay);
            }
            catch (Exception)
            {
                bankProxy = null;
                bankAddress = GetServiceAddress("Bank");
                ConnectWithBank(bankAddress);
                bankProxy.EnlistMoneyTransfer(userId, priceToPay);
            }

            try
            {
                bookstoreProxy.EnlistPurchase(bookId, numOfBooksToBuy);
            }
            catch (Exception)
            {
                bookstoreProxy = null;
                bookstoreAddress = GetServiceAddress("Bookstore");
                ConnectWithBookstore(bookstoreAddress);
                bookstoreProxy.EnlistPurchase(bookId, numOfBooksToBuy);
            }
            

            if (bankProxy.Prepare() && bookstoreProxy.Prepare())
            {
                bankProxy.Commit();
                bookstoreProxy.Commit();
                Console.WriteLine("Transaction was successful.");
                return true;
            }
            else
            {
                bankProxy.Rollback();
                bookstoreProxy.Rollback();
                Console.WriteLine("Transaction was unsuccessful.");
                return false;
            }
        }
    }
}
