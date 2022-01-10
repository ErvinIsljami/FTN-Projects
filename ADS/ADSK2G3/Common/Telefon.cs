using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Telefon
    {
        string marka;
        string model;
        int cena;
        double memorija;
        int id;
        static int brojac;
        
        [DataMember]
        public string Marka { get => marka; set => marka = value; }
        [DataMember]
        public string Model { get => model; set => model = value; }
        [DataMember]
        public int Cena { get => cena; set => cena = value; }
        [DataMember]
        public double Memorija { get => memorija; set => memorija = value; }
        [DataMember]
        public int Id { get => id; set => id = value; }

        public Telefon()    //OBAVEZNO
        {
            id = brojac++;
        }

        public Telefon(string marka, string model, int cena, double memorija)
        {
            this.Marka = marka;
            this.Model = model;
            this.Cena = cena;
            this.Memorija = memorija;
            this.Id = brojac++;
        }

        public override string ToString()
        {
            return $"Telefon {marka} {model} ima memriju {memorija} i kosta {cena}";
        }
    }
}
