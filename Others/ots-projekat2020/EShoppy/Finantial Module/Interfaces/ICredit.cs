using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.Finantial_Module.Interfaces
{
    public interface ICredit
    {
        // Identifikaciona oznaka kredita
        Guid ID { get; set; }

        // Naziv kredita
        string Name { get; set; }

        // Fiksna kamatna stopa kredita
        double Interest { get; set; }

        // Valuta u okviru koje je kredit izdat
        ICurrency Currency { get; set; }

        // Banka koja izdaje kredit
        IBank Bank { get; set; }

        // Minimalni broj godina za vracanje kredita
        int MinimumYear { get; set; }
    }
}
