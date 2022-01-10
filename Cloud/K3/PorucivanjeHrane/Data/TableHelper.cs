using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class TableHelper
    {
        private CloudTable _table;
        private CloudStorageAccount _storageAccount;
        private bool initialised = false;


        public TableHelper()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("HranaTabela");
            _table.CreateIfNotExists();

            if (!initialised)
            {
                Fill();
                initialised = true;
            }
        }

        private void Fill()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();

            Hrana hrana1 = new Hrana("Sendvic");
            hrana1.Kolicina = 10;

            Hrana hrana2 = new Hrana("Pljeskavica");
            hrana2.Kolicina = 7;

            Hrana hrana3 = new Hrana("Pizza");
            hrana3.Kolicina = 8;

            batchOperation.InsertOrReplace(hrana1);
            batchOperation.InsertOrReplace(hrana2);
            batchOperation.InsertOrReplace(hrana3);

            _table.ExecuteBatch(batchOperation);
        }

        public IQueryable<Hrana> RetrieveAll()
        {
            var results = from g in _table.CreateQuery<Hrana>()
                          where g.PartitionKey == "Hrana"
                          select g;
            return results;
        }

        public void Update(Hrana hrana)
        {
            if (Exists(hrana.RowKey))
            {
                TableOperation updateOperation = TableOperation.Replace(hrana);
                _table.Execute(updateOperation);
            }
        }

        private bool Exists(string rowKey)
        {
            List<Hrana> lista = RetrieveAll().ToList();
            foreach (Hrana h in lista)
            {
                if (h.RowKey == rowKey)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
