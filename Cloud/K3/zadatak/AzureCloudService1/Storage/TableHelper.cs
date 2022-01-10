using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public class TableHelper
{
        private CloudStorageAccount storageAccount;
        private CloudTable table;

        public TableHelper(String tableName)
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient
                (
                new Uri(storageAccount.TableEndpoint.AbsoluteUri),
                storageAccount.Credentials
                );
            table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();

        }

        public Entitet GetEntity(string vrstaHrane)
        {
            var result = from g in table.CreateQuery<Entitet>()
                         where g.RowKey == vrstaHrane
                         select g;

            return result.ToList().First();
        }

        public void UpdateEntitet(Entitet entitet)
        {

            TableOperation replaceOperation = TableOperation.Replace(entitet);
            table.Execute(replaceOperation);
        }


    }
}
