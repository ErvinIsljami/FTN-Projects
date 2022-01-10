using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Finantial_Module.Interfaces
{
    public interface IBank
    {
        Guid ID { get; set; }

        string Name { get; set; }

        string Address { get; set; }

        List<ICredit> ListOfCredits { get; set; }

        string AccountPreffix { get; set; }

        List<IAccount> ListOfAccounts { get; set; }

        double MinimumCreditAmount { get; set; }
        
        double MaximumCreditAmount { get; set; }

        int MaximumCreditYear { get; set; }
    }
}
