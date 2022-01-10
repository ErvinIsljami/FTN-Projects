using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Logistic_Module.Interfaces
{
    public interface IProduct
    {
        // Identifikaciona oznaka proizvoda
        Guid ID { get; set; }

        // Naziv proizvoda
        string Name { get; set; }

        // Opis proizvoda
        string Description { get; set; }

        // Cena proizvoda
        double Price { get; set; }

        // Stanje proizvoda - nov = 0 ; koriscen = 1;
        int State { get; set; }
    }
}
