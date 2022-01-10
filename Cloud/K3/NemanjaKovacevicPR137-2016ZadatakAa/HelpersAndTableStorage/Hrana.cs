using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpersAndTableStorage
{
    public class Hrana : TableEntity
    {
        public String Vrsta { get; set; }
        public int Kolicina { get; set; }
       

        public Hrana(string vrsta, int kolicina)
        {
            this.PartitionKey = "Hrana";
            this.RowKey = vrsta;
            this.Vrsta = vrsta;
            this.Kolicina = kolicina;
        }

        public Hrana()
        {

        }
    }
}
