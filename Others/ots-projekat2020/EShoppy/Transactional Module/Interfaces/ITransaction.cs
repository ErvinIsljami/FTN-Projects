using EShoppy.Logistic_Module.Interfaces;
using EShoppy.User_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Transactional_Module.Interfaces
{
    public interface ITransaction
    {
        // Identifikaciona oznaka transakcije
        Guid ID { get; set; }

        // Kupac u transakciji
        IClient Customer { get; set; }

        // Ponuda koja je predmet prodaje u transakciji
        IOffer Offer { get; set; }

        // Tip transakcije
        ITransactionType TransactionType { get; set; }

        // Cena transakcije
        double Price { get; set; }

        // Iznos popusta transakcije
        double Discount { get; set; }

        // Datum izvrsavanja transakcije
        DateTime Date { get; set; }

        // Ocenu koju kupac daje za izvrsenu transackiju
        int Rating { get; set; }
    }
}
