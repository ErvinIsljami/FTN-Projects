using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlIExpress_Data
{
    public class TableHelper
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        private TableOperation insertOperation;

        public TableHelper()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("table");
            _table.CreateIfNotExists();
        }

        public IQueryable<Sluzba> RetrieveAll()
        {
            var results = from g in _table.CreateQuery<Sluzba>()
                          where g.PartitionKey == "Sluzba"
                          select g;
            return results;
        }

        public void InsertNewData(Sluzba data)
        {
            TableOperation insertOperation = TableOperation.Insert(data);
            _table.Execute(insertOperation);
        }
        public void DeleteData(Sluzba data)
        {
            TableOperation deleteOperation = TableOperation.Delete(data);
            _table.Execute(deleteOperation);
        }
        public void AddOrUpdate(Sluzba data)
        {
            TableOperation insertOperation = TableOperation.InsertOrReplace(data);
            _table.Execute(insertOperation);
        }
        public void Update(Sluzba data)
        {
            TableOperation updateOperation = TableOperation.Replace(data);
            _table.Execute(updateOperation);
        }

     
    }
}
