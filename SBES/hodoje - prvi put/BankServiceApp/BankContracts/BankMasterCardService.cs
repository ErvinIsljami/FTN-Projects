using Common.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankServiceApp.BankContracts
{
    public class BankMasterCardService : IBankMasterCardService
    {
        public bool RequestNewCard(string pin)
        {
            throw new NotImplementedException();
        }

        public bool RevokeExistingCard(string pin)
        {
            throw new NotImplementedException();
        }
    }
}
