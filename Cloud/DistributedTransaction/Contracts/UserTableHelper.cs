using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class UserTableHelper
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public UserTableHelper()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString2"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("banktable");
            _table.CreateIfNotExists();
           
        }

        public IQueryable<User> RetrieveAllElements()
        {
            var results = from g in _table.CreateQuery<User>() where g.PartitionKey == "users" select g;
            return results;
        }

        public void AddElement(User newElement)
        {
            // Samostalni rad: izmestiti tableName u konfiguraciju servisa.
            TableOperation insertOperation = TableOperation.InsertOrReplace(newElement);

            _table.Execute(insertOperation);
        }

        public void Update(string userName, double newValue)
        {
            var results = from g in _table.CreateQuery<User>() where g.PartitionKey == "users" select g;
            User user = null;
            foreach (User b in results)
            {
                if (b.RowKey == userName + "prep")
                {
                    user = b;
                    break;
                }
            }
            if(user != null)
            {
                user.Money -= newValue;
                TableOperation insertOperation = TableOperation.InsertOrReplace(user);

                _table.Execute(insertOperation);
            }


        }

        public double GetMoney(string user)
        {
            double value = 0;
            var results = from g in _table.CreateQuery<User>() where g.PartitionKey == "users" select g;

            foreach (User b in results)
            {
                if (b.RowKey == user + "prep")
                    value = b.Money;
            }

            return value;
        }
    }
}
