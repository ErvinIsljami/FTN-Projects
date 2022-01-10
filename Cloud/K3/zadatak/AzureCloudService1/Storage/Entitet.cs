using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public class Entitet : TableEntity
    {
        public String VrstaHrane { get; set; }
        public String Kolicina { get; set; }

        
        public Entitet(string vrstaHrane, string kolicina)
        {
            PartitionKey = "Hrana";
            RowKey = vrstaHrane;
            Kolicina = kolicina;
            
        }

        public Entitet()
        {

        }
    }
}
