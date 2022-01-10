using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Common
{
   [DataContract]
    public class Clan
    {
        string ime;
        string prezime;
        string jmbg;
        List<Knjiga> lista;

        public Clan(string ime, string prezime, string jmbg)
        {
            this.Ime = ime;
            this.Prezime = prezime;
            this.Jmbg = jmbg;
            Lista = new List<Knjiga>();
        }

        [DataMember]
        public string Ime { get => ime; set => ime = value; }
        [DataMember]
        public string Prezime { get => prezime; set => prezime = value; }
        [DataMember]
        public string Jmbg { get => jmbg; set => jmbg = value; }
        [DataMember]
        public List<Knjiga> Lista { get => lista; set => lista = value; }

        public override string ToString()
        {
            string ret = "";
            ret += $"Clan {ime} {prezime} : {jmbg} ima knjige: " + "\n";
            foreach(Knjiga k in lista)
            {
                ret += k + "\n";
            }
            return ret;
        }
    }
}
