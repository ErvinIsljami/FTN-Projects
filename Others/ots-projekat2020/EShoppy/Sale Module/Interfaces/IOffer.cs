using EShoppy.User_Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Logistic_Module.Interfaces
{
    public interface IOffer
    {
        // Identifikaciona oznaka ponude
        Guid ID { get; set; }

        // Korisnik koji kreira ponudu
        IClient Client { get; set; }

        // Lista proizvoda u ponudi
        List<IProduct> ListOfProducts { get; set; }

        // Cena ponude
        double Price { get; set; }

        // Datum postavljanja ponude u sistem
        DateTime SubmitionDate { get; set; }

        // Lista transporta koje ponuda obuhvata
        List<ITransport> ListOfTransports { get; set; }
    }
}
