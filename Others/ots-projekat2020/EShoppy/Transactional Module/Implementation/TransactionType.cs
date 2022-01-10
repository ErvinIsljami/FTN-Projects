using EShoppy.Transactional_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Transactional_Module.Implementation
{
    public class TransactionType : ITransactionType
    {
        private Guid iD;
        private string description;

        public TransactionType(string description)
        {
            this.description = description;
            ID = Guid.NewGuid();
        }

        public Guid ID { get => iD; set => iD = value; }
        public string Description { get => description; set => description = value; }
    }
}
