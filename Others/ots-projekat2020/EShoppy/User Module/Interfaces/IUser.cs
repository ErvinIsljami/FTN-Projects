using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.User_Module.Interfaces
{
    public interface IUser : IClient
    {
        // Ime fizickog lica
        string FirstName { get; set; }

        // Prezime fizickog lica
        string LastName { get; set; }

        // Sifra fizickog lica
        string Password { get; set; }

        // Datum rodjenja fizickog lica
        DateTime DateOfBirth { get; set; }

        // Mesto rodjenja fizickog lica
        string Place { get; set; }

    }
}
