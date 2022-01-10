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
    public class BookTableHelper
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public BookTableHelper()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("booktable");
            _table.CreateIfNotExists();
        }

        public IQueryable<Book> RetrieveAllElements()
        {
            var results = from g in _table.CreateQuery<Book>() where g.PartitionKey == "books" select g;
            return results;
        }

        public void AddElement(Book newElement)
        {
            // Samostalni rad: izmestiti tableName u konfiguraciju servisa.
            TableOperation insertOperation = TableOperation.InsertOrReplace(newElement);

            _table.Execute(insertOperation);
        }

        public double GetPrice(string bookName)
        {
            double value = 0;
            var results = from g in _table.CreateQuery<Book>() where g.PartitionKey == "books" select g;
            
            foreach(Book b in results)
            {
                if (b.RowKey == bookName + "prep")
                    value = b.Cost;
            }

            return value;
        }

        public int GetCount(string bookName)
        {
            int value = 0;
            var results = from g in _table.CreateQuery<Book>() where g.PartitionKey == "books" select g;

            foreach (Book b in results)
            {
                if (b.RowKey == bookName + "prep")
                    value = b.Cnt;
            }

            return value;
        }

        public void Update(string bookName, int newValue)
        {
            var results = from g in _table.CreateQuery<Book>() where g.PartitionKey == "books" select g;
            Book bk = null;
            foreach (Book b in results)
            {
                if (b.RowKey == bookName + "prep")
                {
                    bk = b;
                    break;
                }
                   
            }
            if(bk != null)
            {
                bk.Cnt -= newValue;
                TableOperation insertOperation = TableOperation.InsertOrReplace(bk);

                _table.Execute(insertOperation);
            }
        }

    }
}
;