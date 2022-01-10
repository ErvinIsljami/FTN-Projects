using Common.ServiceContracts;
using Common.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankServiceApp.BankContracts
{
    public class BankTransactionService : IBankTransactionService
    {
        public bool ExecuteTransaction(ITransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
