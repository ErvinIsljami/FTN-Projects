using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
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

        public IQueryable<Lek> RetrieveAll()
        {
            var results = from g in _table.CreateQuery<Lek>()
                          where g.PartitionKey == "data"
                          select g;
            return results;
        }

        public void InsertNewData(Lek data)
        {
            TableOperation insertOperation = TableOperation.Insert(data);
            _table.Execute(insertOperation);
        }
        public void DeleteData(Lek data)
        {
            TableOperation deleteOperation = TableOperation.Delete(data);
            _table.Execute(deleteOperation);
        }
        public void AddOrUpdate(Lek data)
        {
            TableOperation insertOperation = TableOperation.InsertOrReplace(data);
            _table.Execute(insertOperation);
        }
        public void Update(Lek data)
        {
            TableOperation updateOperation = TableOperation.Replace(data);
            _table.Execute(updateOperation);
        }
        
    }
}
