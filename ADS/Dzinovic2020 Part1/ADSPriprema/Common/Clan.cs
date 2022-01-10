using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Clan
    {

        [DataMember]
        public string Ime { get; set; }
        [DataMember]
        public string Prezime { get; set; }
        [DataMember]
        public long JMBG { get; set; }
        [DataMember]
        public List<string> Knjige { get; set; }

        public Clan()
        {
            Knjige = new List<string>();
        }

        public Clan(string ime, string prezime, long jmbg)
        {
            this.Ime = ime;
            this.Prezime = prezime;
            this.JMBG = jmbg;
            this.Knjige = new List<string>();
        }
        
    }
}
