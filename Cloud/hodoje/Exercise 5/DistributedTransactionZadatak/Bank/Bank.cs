using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Bank
{
    public class Bank : IBank
    {
        private static BankDataRepository bankDataRepository;
        private static string userToBuyId;
        private static double amountToPay;

        public Bank()
        {
            bankDataRepository = new BankDataRepository();
        }

        public bool Prepare()
        {
            Client clientWithCurrentAmount = bankDataRepository.GetClientById(userToBuyId).FirstOrDefault();

            if (clientWithCurrentAmount != null)
            {
                if (clientWithCurrentAmount.Amount - amountToPay > 0)
                {
                    // we need to set all the properties or it will fail.
                    Client clientWithUpdatedAmount = new Client
                    {
                        PartitionKey = clientWithCurrentAmount.PartitionKey,
                        RowKey = userToBuyId + "prep",
                        UserId = userToBuyId + "prep",
                        Amount = clientWithCurrentAmount.Amount - amountToPay
                    };
                    bankDataRepository.AddClient(clientWithUpdatedAmount);
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
            Client oldClient = bankDataRepository.GetClientById(userToBuyId).FirstOrDefault();
            Client newClient = bankDataRepository.GetClientById(userToBuyId + "prep").FirstOrDefault();

            if (oldClient != null && newClient != null)
            {
                oldClient.Amount = newClient.Amount;

                bankDataRepository.UpdateClient(oldClient);
                bankDataRepository.RemoveClient(newClient.UserId);
            }
            else
            {
                Trace.WriteLine("Either oldClient or newClient is null.");
            }
        }

        public void Rollback()
        {
            if (bankDataRepository.GetClientById(userToBuyId + "prep") != null)
            {
                bankDataRepository.RemoveClient(userToBuyId + "prep");
            }
        }

        public void ListClients()
        {
            foreach (var c in bankDataRepository.GetAllClients().ToList())
            {
                Trace.WriteLine($"Client: {c.UserId}, Amount: {c.Amount}");
            }
        }

        public void EnlistMoneyTransfer(string userId, double amount)
        {
            userToBuyId = userId;
            amountToPay = amount;
        }
    }
}
