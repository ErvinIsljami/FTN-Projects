using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class TableHelper
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public TableHelper()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("table");
            _table.CreateIfNotExists();

        }

        public IQueryable<Information> RetrieveAllElements()
        {
            var results = from g in _table.CreateQuery<Information>()
                          where g.PartitionKey == "InformationTable"
                          select g;
            return results;
        }

        public void AddElement(Information newElement)
        {
            TableOperation insertOperation = TableOperation.InsertOrReplace(newElement);

            _table.Execute(insertOperation);
        }

        public string GetInformation(string id)
        {
            string value = null;
            var results = from g in _table.CreateQuery<Information>()
                          where g.PartitionKey == "InformationTable"
                          select g;

            foreach (Information temp in results)
            {
                if (temp.RowKey == id)
                    value = temp.Trace;
            }

            return value;
        }
    }
}
