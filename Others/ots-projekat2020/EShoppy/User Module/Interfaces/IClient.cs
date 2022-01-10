using EShoppy.Finantial_Module.Interfaces;
using EShoppy.Transactional_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.User_Module.Interfaces
{
    public interface IClient
    {
        Guid ID { get; set; }
        string Email { get; set; }

        List<ITransaction> ListOfBuyingTransaction { get; set; }

        List<ITransaction> ListOfSellingTransaction { get; set; }

        List<IAccount> ListOfAccounts { get; set; }
    }
}