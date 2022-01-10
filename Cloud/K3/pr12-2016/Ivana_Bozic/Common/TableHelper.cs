using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            _table = tableClient.GetTableReference("robatable");
            _table.CreateIfNotExists();
        }
        public void DodajRobu(Roba novaRoba)
        {
            TableOperation insertOperation = TableOperation.InsertOrReplace(novaRoba);
            _table.Execute(insertOperation);
        }
        public int RobaPostoji(string vrstaRobe) {
            int kolicina = 0;
            var results = from g in _table.CreateQuery<Roba>() where g.PartitionKey == "Roba" && g.RowKey == vrstaRobe select g;
            if (results.ToList().Count != 0) { Roba roba = results.ToList()[0]; kolicina = roba.Kolicina;
            }
            return kolicina;
        }
        public Roba PreuzmiRobu(string vrstaRobe)
        {
            var results = from g in _table.CreateQuery<Roba>() where g.PartitionKey == "Roba" && g.RowKey == vrstaRobe select g;
            if (results.ToList().Count == 0)
            {
                
                return new Roba()
                {
                    Vrsta = " ", Kolicina = 0
                };
            }
            return results.ToList()[0];
        }
    }
}

