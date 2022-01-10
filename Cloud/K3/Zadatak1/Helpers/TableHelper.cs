using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class TableHelper
    {
        public static CloudTable GetTableReference(string tableName)
        {
            //var acc = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = acc.CreateCloudTableClient();
            var table = client.GetTableReference(tableName);
            table.CreateIfNotExists();
            return table;
        }
    }
}
