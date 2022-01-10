using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Logistic_Module.Interfaces
{
    public interface ITransport
    {
        // Identifikaciona oznaka transporta
        Guid ID { get; set; }

        // Opis transporta
        string Description { get; set; }

        // Koeficient transporta - vrednost mora biti izmedju 1 i 1,5
        double Coef { get; set; }
    }
}
