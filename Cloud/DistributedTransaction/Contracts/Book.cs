using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Book : TableEntity
    {
        int cnt;
        double cost;
        string name;

        public int Cnt { get => cnt; set => cnt = value; }
        public double Cost { get => cost; set => cost = value; }
        public string Name { get => name; set => name = value; }

        public Book(int c, int cs, string name)
        {
            this.cnt = c;
            this.cost = cs;
            this.name = name;
            PartitionKey = "books";
            RowKey = name + "prep";
        }
        public Book() { }
    }
}
