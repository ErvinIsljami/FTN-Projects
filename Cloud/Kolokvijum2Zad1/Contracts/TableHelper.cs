﻿using Microsoft.Azure;
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
            _table = tableClient.GetTableReference("ArtikalTable");
            _table.CreateIfNotExists();
        }

        public IQueryable<Artikal> RetrieveAllElements()
        {
            var results = from g in _table.CreateQuery<Artikal>() where g.PartitionKey == "Artikli" select g;
            return results;
        }

        public void AddElement(Artikal newElement)
        {
            // Samostalni rad: izmestiti tableName u konfiguraciju servisa.
            TableOperation insertOperation = TableOperation.InsertOrReplace(newElement);

            _table.Execute(insertOperation);
        }

        

        public string GetId(string id)
        {
            string value = null;
            var results = from g in _table.CreateQuery<Artikal>()
                          where g.PartitionKey == "Artikal"
                          select g;

            foreach (Artikal b in results)
            {
                if (b.RowKey == id)
                    value = b.Name;
            }

            return value;
        }

    }
}
