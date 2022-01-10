using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageData
{
    public class Roba : TableEntity
    {
        public int Stanje { get; set; } 

        public Roba(String vrsta, int stanje)
        {
            PartitionKey = "Roba";
            RowKey = vrsta;
            Stanje = stanje;
        }

        public Roba()
        {

        }
    }
}
