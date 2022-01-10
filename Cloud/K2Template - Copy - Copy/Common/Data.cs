using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Data : TableEntity
    {
        public string Propertie1 { get; set; }
        public string Propertie2 { get; set; }

        public Data(string prop1, string prop2, string key)
        {
            Propertie1 = prop1;
            Propertie2 = prop2;
            RowKey = key;
            PartitionKey = "Data";
        }

        public Data()
        {
            PartitionKey = "Data";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
