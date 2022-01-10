using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class KomandModel
    {
        string komanda;

        public KomandModel()
        {
        }

        public string Komanda { get => komanda; set => komanda = value; }
    }
}