using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESProject.Classes
{
    public class DeltaCD
    {
        public string TransactionID { get; set; }
        public List<CollectionDescription> Add { get; set; }
        public List<CollectionDescription> Update { get; set; }
        public List<CollectionDescription> Delete { get; set; }

        public DeltaCD(string transactionID, List<CollectionDescription> add, List<CollectionDescription> update, List<CollectionDescription> delete)
        {
            TransactionID = transactionID;
            Add = add;
            Update = update;
            Delete = delete;
        }

        public DeltaCD()
        {
            Add = new List<CollectionDescription>();
            Update = new List<CollectionDescription>();
            Delete = new List<CollectionDescription>();
        }
    }
}
