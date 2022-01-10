using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Bank : IBank
    {
        private static List<Client> bankRepository = new List<Client>
        {
            new Client(){Amount = 500, UserId = "user1" },
            new Client(){Amount = 600, UserId = "user2" }
        };

        private static string userToBuyId;
        private static double amountToPay;


        public bool Prepare()
        {
            Client clientWithCurrentAmount = bankRepository.FirstOrDefault(x => x.UserId == userToBuyId);

            if (clientWithCurrentAmount != null)
            {
                if (clientWithCurrentAmount.Amount - amountToPay > 0)
                {
                    Client clientWithUpdatedAmount = new Client()
                    {
                        UserId = userToBuyId + "prep",
                        Amount = clientWithCurrentAmount.Amount - amountToPay
                    };
                    bankRepository.Add(clientWithUpdatedAmount);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Commit()
        {
            Client oldClient = bankRepository.FirstOrDefault(x => x.UserId == userToBuyId);
            Client newClient = bankRepository.FirstOrDefault(x => x.UserId == userToBuyId + "prep");

            if (oldClient != null && newClient != null)
            {
                Console.WriteLine($"Old client[{oldClient.UserId}] amount state: {oldClient.Amount}");
                oldClient.Amount = newClient.Amount;
                bankRepository.Remove(newClient);
                Console.WriteLine($"New client[{oldClient.UserId}] amount state: {oldClient.Amount}");
            }
            else
            {
                Console.WriteLine("Either oldClient or newClient is null.");
            }
        }

        public void Rollback()
        {
            if (bankRepository.FirstOrDefault(x => x.UserId == userToBuyId + "prep") != null)
            {
                Client clientToDelete = bankRepository.FirstOrDefault(x => x.UserId == userToBuyId + "prep");
                bankRepository.Remove(clientToDelete);
            }
        }

        public void ListClients()
        {
            if (bankRepository.Count > 0)
            {
                foreach (var client in bankRepository)
                {
                    Console.WriteLine($"Client: {client.UserId}, Amount: {client.Amount}");
                }
            }
            else
            {
                Console.WriteLine("No clients in bank.");
            }
        }

        public void EnlistMoneyTransfer(string userId, double amount)
        {
            userToBuyId = userId;
            amountToPay = amount;
        }
    }
}
