using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Televizor : TableEntity
    {
        public string Model { get; set; }
        public string Marka { get; set; }
        public int Cena { get; set; }

        public Televizor(string marka, string model, int cena, string sifraTv)
        {
            this.Marka = marka;
            this.Model = model;
            this.Cena = cena;
            RowKey = sifraTv;
            PartitionKey = "Televizori";
        }

        public Televizor()
        {
            PartitionKey = "Televizori";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
