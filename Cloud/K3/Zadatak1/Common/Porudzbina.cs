using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Porudzbina : TableEntity
    {
        public string Type { get; set; }
        public int Amount { get; set; }

        public Porudzbina()
        {

        }

        public Porudzbina(string type, int amount)
        {
            Type = type;
            Amount = amount;
            PartitionKey = "Porudzbina";
            RowKey = Type;
        }

        public override string ToString()
        {
            return $"{Type} {Amount}";
        }
    }
}
