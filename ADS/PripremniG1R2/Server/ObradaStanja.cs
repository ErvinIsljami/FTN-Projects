using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ObradaStanja : IStanjeServisa
    {
        public static KonfiguracijaServisa konfiguracija
        = new KonfiguracijaServisa();
        public void AzuriranjeStanja(EStanjeServera stanje)
        {
            ObradaStanja.konfiguracija.StanjeServera = stanje;
        }
        public EStanjeServera ProveraStanja()
        {
            return ObradaStanja.konfiguracija.StanjeServera;
        }
    }
}
