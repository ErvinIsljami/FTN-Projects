using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlIExpress_Data
{
    public class Sluzba : TableEntity
    {
        public String Vrstarobe { get; set; }
        public int Kolicina { get; set; }

        public Sluzba(string kolicina)
        {
            PartitionKey = "Sluzba";
            RowKey = kolicina;
        }

        public Sluzba()
        {

        }
    
    }
}
