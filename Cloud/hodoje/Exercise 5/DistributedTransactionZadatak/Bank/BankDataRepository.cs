using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Bank
{
    public class BankDataRepository
    {
        private CloudStorageAccount storageAccount;
        private CloudTable table;

        public BankDataRepository()
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("BankDataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(storageAccount.TableEndpoint.AbsoluteUri), storageAccount.Credentials);
            table = tableClient.GetTableReference("ClientTable");
            table.CreateIfNotExists();
        }

        public IQueryable<Client> GetAllClients()
        {
            var clientList = from x in table.CreateQuery<Client>() where x.PartitionKey == "Client" select x;
            if (clientList.ToList().Count > 0)
            {
                return clientList;
            }
            else
            {
                Trace.WriteLine("There are no clients in the database.");
                return clientList;
            }
        }

        public IQueryable<Client> GetClientById(string clientId)
        {
            var client = from x in table.CreateQuery<Client>()
                where x.PartitionKey == "Client" && x.RowKey == clientId
                select x;
            if (client.ToList().Count > 0)
            {
                return client;
            }
            else
            {
                Trace.WriteLine($"Client with id: {clientId} does not exist in the database.");
                return client;
            }
        }

        public bool AddClient(Client newClient)
        {
            if (!GetAllClients().ToList().Contains(newClient))
            {
                TableOperation insertOperation = TableOperation.Insert(newClient);
                table.Execute(insertOperation);
                return true;
            }
            else
            {
                Trace.WriteLine($"Client with id: {newClient.UserId} already exists.");
                return false;
            }
        }

        public bool RemoveClient(string clientId)
        {
            Client clientForDeletion = GetClientById(clientId).FirstOrDefault();
            if (clientForDeletion != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(clientForDeletion);
                table.Execute(deleteOperation);
                return true;
            }
            else
            {
                Trace.WriteLine($"Client with id: {clientId} could not be removed because it does not exist.");
                return false;
            }
        }

        public bool UpdateClient(Client updatedClient)
        {
            if (GetClientById(updatedClient.UserId) != null)
            {
                TableOperation updateOperation = TableOperation.Replace(updatedClient);
                table.Execute(updateOperation);
                return true;
            }
            else
            {
                Trace.WriteLine($"Client with id: {updatedClient.UserId} could not be updated because it does not exist.");
                return false;
            }
        }
    }
}
