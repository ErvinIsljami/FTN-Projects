using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Hrana : TableEntity
    {
        public int Kolicina { get; set; }

        public Hrana(string vrstaHrane)
        {
            PartitionKey = "Hrana";
            RowKey = vrstaHrane;
        }

        public Hrana() { }
    }
}
