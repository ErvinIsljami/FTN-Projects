using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class KonfiguracijaServisa
    {
        private EStanjeServera stanjeServera;
        public EStanjeServera StanjeServera
        {
            get => stanjeServera;
            set => stanjeServera = value;
        }
        public KonfiguracijaServisa()
        {
            this.stanjeServera
            = Properties.Settings.Default.StanjeServera;
            Console.WriteLine("Stanje servisa je: " +this.StanjeServera.ToString());
        }
    }
}
