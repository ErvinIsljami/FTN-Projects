using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Contract;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string exitOrNot = "";
            string tcAddress = "";
            string userId = "";
            string bookId = "";
            int numOfBooksToBuy;

            Console.WriteLine("Enter address of Transaction Coordinator:");
            tcAddress = Console.ReadLine();

            while (exitOrNot != "exit")
            {
                var binding = new NetTcpBinding();
                var endpoint = new EndpointAddress(new Uri($"net.tcp://{tcAddress}/TransactionCoordinator"));
                var factory = new ChannelFactory<IPurchase>(binding);
                var proxy = factory.CreateChannel(endpoint);
                Console.WriteLine("Connected to Transaction Coordinator.");

                Console.WriteLine("Enter user id:");
                userId = Console.ReadLine();
                Console.WriteLine("Enter book id:");
                bookId = Console.ReadLine();
                do
                {
                    Console.WriteLine("Enter number of books you want to buy:");
                    Int32.TryParse(Console.ReadLine(), out numOfBooksToBuy);
                } while (numOfBooksToBuy < 1);

                if (proxy.OrderItem(bookId, userId, numOfBooksToBuy))
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
