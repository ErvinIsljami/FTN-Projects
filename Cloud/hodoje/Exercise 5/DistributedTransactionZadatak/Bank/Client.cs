using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Bank
{
    public class Client : TableEntity
    {
        public string UserId { get; set; }

        public double Amount { get; set; }

        public Client() { }

        public Client(string userId)
        {
            PartitionKey = "Client";
            RowKey = userId;
        }
    }
}
