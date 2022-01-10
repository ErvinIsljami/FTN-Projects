using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Information : TableEntity
    {
        public string Info { get; set; }
        public string Trace { get; set; }

        private static int cnt = 0;

        public Information()
        {
            PartitionKey = "InformationTable";
            RowKey = cnt.ToString();
        }

        public Information(string info, string trace)
        {
            PartitionKey = "InformationTable";
            RowKey = cnt.ToString();
            Info = info;
            Trace = trace;
        }
        
    }
}
