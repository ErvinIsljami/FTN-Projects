using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Artikal : TableEntity
    {
        public string Name { get; set; }

        public Artikal(string id)
        {
            PartitionKey = "Artikal";
            RowKey = id;
        }
        public Artikal() { }
    }
}
