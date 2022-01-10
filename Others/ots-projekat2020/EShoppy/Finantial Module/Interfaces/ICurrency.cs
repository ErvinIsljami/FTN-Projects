using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Finantial_Module.Interfaces
{
    public interface ICurrency
    {
        // Identifikaciona oznaka valute
        Guid ID { get; set; }

        // Naziv valute
        string Name { get; set; }

        // Kod valute
        string Code { get; set; }
        
        // Trenutni kurs valute u odnosu na dinar
        double Value { get; set; }
    }
}
