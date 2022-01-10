using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Komanda : TableEntity
    {
        public string Propertie1 { get; set; }

        public Komanda(string prop1, string prop2, string key)
        {
            Propertie1 = prop1;
            RowKey = key;
            PartitionKey = "Data";
        }

        public Komanda()
        {
            PartitionKey = "Data";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
