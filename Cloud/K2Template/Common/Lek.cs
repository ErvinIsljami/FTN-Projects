using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Lek : TableEntity
    {
        public string naziv { get; set; }
        public int Cena { get; set; }

        public Lek(string naziv, int Cena, string key)
        {
            this.naziv = naziv;
            this.Cena = Cena;
            RowKey = key;   // 
            PartitionKey = "data";
        }

        public Lek()
        {
            PartitionKey = "data";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
