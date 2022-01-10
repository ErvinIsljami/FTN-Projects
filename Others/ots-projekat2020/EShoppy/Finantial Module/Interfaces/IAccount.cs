using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Finantial_Module.Interfaces
{
    public interface IAccount
    {
        // Identifikaciono obelezje racuna
        Guid ID { get; set; }

        // Broj racuna
        string AccountNumber { get; set; }

        // Banka u okviru koje je racun kreiran
        IBank Bank { get; set; }

        // Stanje racuna
        double Balance { get; set; }

        // Stanje zaduzenja racuna
        double CreditPayment { get; set; }

        // Dostupnost kredita
        bool CreditAvailable { get; set; }
    }
}
