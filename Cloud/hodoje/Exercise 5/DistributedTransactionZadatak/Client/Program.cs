using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Client
{
    class Program
    {
        private static IPurchase purchaseProxy;

        static void Main(string[] args)
        {
            string exitOrNot = "";
            string userId = "";
            string bookId = "";
            int numOfBooksToBuy;

            Console.WriteLine("Enter your userId:");
            userId = Console.ReadLine();

            while (exitOrNot != "exit")
            {
                var binding = new NetTcpBinding();
                var endpoint = new EndpointAddress(new Uri($"net.tcp://localhost:45454/TransactionInputEndpoint"));
                var channelFactory = new ChannelFactory<IPurchase>(binding);
                purchaseProxy = channelFactory.CreateChannel(endpoint);

                Console.WriteLine("Enter bookId for the book you want to buy:");
                bookId = Console.ReadLine();        
                do
                {
                    Console.WriteLine("Enter the amount of books you want to buy:");
                    Int32.TryParse(Console.ReadLine(), out numOfBooksToBuy);
                }
                while (numOfBooksToBuy < 1);

                if (purchaseProxy.OrderItem(bookId, userId, numOfBooksToBuy))
                {
                    Console.WriteLine("Transaction was successful.");
                }
                else
                {
                    Console.WriteLine("Transaction was unsuccessful. Either not enough funds or not enough amount of books available.");
                }
                Console.WriteLine("Do you want to continue shopping? Enter 'exit' to stop shopping or anything else to continue.");
                exitOrNot = Console.ReadLine();
            }
        }
    }
}
