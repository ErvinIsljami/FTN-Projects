using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Transactional_Module.Interfaces
{
    public interface ITransactionType
    {
        // Identifikaciona oznaka tipa transakcije
        Guid ID { get; set; }

        // Opis tipa transakcije
        string Description { get; set; }
    }
}
