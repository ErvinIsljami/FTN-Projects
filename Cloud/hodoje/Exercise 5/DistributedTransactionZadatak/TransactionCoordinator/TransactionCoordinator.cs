using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace TransactionCoordinator
{
    public class TransactionCoordinator : IPurchase
    {
        private static IBank bankProxy;
        private static IBookstore bookstoreProxy;

        private static string bankInternalEndpointName = "BankInternalEndpoint";
        private static string bookstoreInternalEndpointName = "BookstoreInternalEndpoint";

        public TransactionCoordinator()
        {
            ConnectWithBank();
            ConnectWithBookstore();
        }

        public void ConnectWithBank()
        {
            var binding = new NetTcpBinding();
            var bankInternalEndpoint = RoleEnvironment.Roles["Bank"].Instances[0].InstanceEndpoints[bankInternalEndpointName].IPEndpoint;
            var endpoint = new EndpointAddress(new Uri($"net.tcp://{bankInternalEndpoint}/{bankInternalEndpointName}"));
            var channelFactory = new ChannelFactory<IBank>(binding);
            bankProxy = channelFactory.CreateChannel(endpoint);
        }

        public void ConnectWithBookstore()
        {
            var binding = new NetTcpBinding();
            var bookstoreInternalEndpoint = RoleEnvironment.Roles["Bookstore"].Instances[0].InstanceEndpoints[bookstoreInternalEndpointName].IPEndpoint;
            var endpoint = new EndpointAddress(new Uri($"net.tcp://{bookstoreInternalEndpoint}/{bookstoreInternalEndpointName}"));
            var channelFactory = new ChannelFactory<IBookstore>(binding);
            bookstoreProxy = channelFactory.CreateChannel(endpoint);
        }

        public bool OrderItem(string bookId, string userId, int numOfBooksToBuy)
        {
            double bookPrice = bookstoreProxy.GetItemPrice(bookId);
            double priceToPay = numOfBooksToBuy * bookPrice;

            bankProxy.EnlistMoneyTransfer(userId, priceToPay);
            bookstoreProxy.EnlistPurchase(bookId, numOfBooksToBuy);

            if (bankProxy.Prepare() && bookstoreProxy.Prepare())
            {
                bankProxy.Commit();
                bookstoreProxy.Commit();
                return true;
            }
            else
            {
                bankProxy.Rollback();
                bookstoreProxy.Rollback();
                return false;
            }
        }
    }
}
