using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Roba : TableEntity
    {
        public string Vrsta { get; set; }
        public int Kolicina { get; set; }

        public Roba(string vrsta)
        {
            PartitionKey = "Roba";
            RowKey = vrsta;
        }

        public Roba() { }

        public override string ToString()
        {
            return Vrsta + " " + Kolicina;
        }
    }
}
