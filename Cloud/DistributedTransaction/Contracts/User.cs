using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class User : TableEntity
    {
        double money;
        string name;
        public User(string name, double money)
        {
            this.Name = name;
            this.money = money;
            PartitionKey = "users";
            RowKey = name + "prep";
        }

        public double Money { get => money; set => money = value; }
        public string Name { get => name; set => name = value; }

        public User() { }


    }
}
