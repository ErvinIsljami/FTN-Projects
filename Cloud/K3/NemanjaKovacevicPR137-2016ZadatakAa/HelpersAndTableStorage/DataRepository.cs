using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpersAndTableStorage
{
    public class DataRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public DataRepository(string imeTabele)
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString6"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("tabelaHrane");
            _table.CreateIfNotExists();
        }

        public IQueryable<Hrana> RetrieveAllStudents()
        {
            var results = from g in _table.CreateQuery<Hrana>()
                            where g.PartitionKey == "Hrana"
                          select g;
            return results;
        }

        public void Add(Hrana roba)
        {
            TableOperation insertOperation = TableOperation.InsertOrReplace(roba);
            _table.Execute(insertOperation);
        }

        public void Modify(Hrana roba)
        {
            _table.Execute(TableOperation.Replace(roba));
        }
    }
    
}
