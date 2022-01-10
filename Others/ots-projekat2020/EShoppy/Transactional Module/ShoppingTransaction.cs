using EShoppy.Transactional_Module.Implementation;
using EShoppy.Transactional_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Transactional_Module
{
    public static class ShoppingTransaction
    {
        public static Dictionary<Guid, ITransaction> Transactions { get; private set; }
        public static Dictionary<string, TransactionType> TransactionTypes {get; private set;}
        static ShoppingTransaction()
        {
            Transactions = new Dictionary<Guid, ITransaction>();
            TransactionTypes = new Dictionary<string, TransactionType>();
            TransactionTypes.Add("full", new TransactionType("full"));
            TransactionTypes.Add("rate", new TransactionType("rate"));
        }
    }
}
