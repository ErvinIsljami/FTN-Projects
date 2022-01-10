using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.User_Module.Interfaces
{
    public interface IOrganization : IClient
    {
        // Ime pravnog lica
        string Name { get; set; }

        // Jedinstveni broj pravnog lica
        string PIB { get; set; }

        // Sediste pravnog lica
        string Place { get; set; }

        // Datum osnivanja pravnog lica
        DateTime DateOfEst { get; set; }
    }
}
