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
        string ime;
        string prezime;
        string jmbg;
        List<Knjiga> listaKnjiga;

        public Clan()
        {
            listaKnjiga = new List<Knjiga>();
        }

        public Clan(string ime, string prezime, string jmbg)
        {
            this.Ime = ime;
            this.Prezime = prezime;
            this.Jmbg = jmbg;
            listaKnjiga = new List<Knjiga>();
        }

        [DataMember]
        public string Ime { get => ime; set => ime = value; }
        [DataMember]
        public string Prezime { get => prezime; set => prezime = value; }
        [DataMember]
        public string Jmbg { get => jmbg; set => jmbg = value; }
        [DataMember]
        public List<Knjiga> ListaKnjiga { get => listaKnjiga; set => listaKnjiga = value; }

        public override string ToString()
        {
            string ret = "";
            ret += $"Clan {ime} {prezime} jmbg: {jmbg} ima knjige: \n";

            foreach (Knjiga knjiga in listaKnjiga)
            {
                ret += knjiga + "\n";
            }


            return ret;
        }

    }
}
