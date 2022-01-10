using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.ServiceModel;

namespace TransactionCoordinator
{
    class Purchase : IPurchase
    {
        static IBookstore bookProxy;
        static IBank bankProxy;

        public bool OrderItem(string bookId, string userId)
        {
            ConnectToBank();
            ConnectToBookStore();

            double cena = bookProxy.GetItemPrice(bookId);
            bool bankRdy;
            bool bookRdy;
            //enlist
            bankProxy.EnlistMoneyTransfer(userId, cena);
            bookProxy.EnlistPurchase(bookId, 1);

            //prepare
            bankRdy = bankProxy.Prepare();
            if (!bankRdy)
            {
                bankProxy.Rollback();
                bookProxy.Rollback();
            }
            bookRdy = bookProxy.Prepare();

            if (bookRdy)
            {
                bankProxy.Commit();
                bookProxy.Commit();

                return true;
            }
            else
            {
                bankProxy.Rollback();
                bookProxy.Rollback();
                return false;
            }
        }


        static void ConnectToBank()
        {
            string port = RoleEnvironment.Roles["Bank"].Instances[0].InstanceEndpoints["InternalEndpoint"].IPEndpoint.Port.ToString();
            var binding = new NetTcpBinding();
            ChannelFactory<IBank> factory = new
            ChannelFactory<IBank>(binding, new
            EndpointAddress("net.tcp://localhost:" + port + "/Bank"));
            bankProxy = factory.CreateChannel();
        }
        static void ConnectToBookStore()
        {
            string port = RoleEnvironment.Roles["Bookstore"].Instances[0].InstanceEndpoints["InternalEndpoint"].IPEndpoint.Port.ToString();
            var binding = new NetTcpBinding();
            ChannelFactory<IBookstore> factory = new
            ChannelFactory<IBookstore>(binding, new
            EndpointAddress("net.tcp://localhost:" + port + "/BookStore"));
            bookProxy = factory.CreateChannel();
        }
    }
}
