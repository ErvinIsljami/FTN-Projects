using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Automobil
    {

        [DataMember]
        public string Marka { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public int Cena { get; set; }
        [DataMember]
        public int Godiste { get; set; }
        [DataMember]
        private static int brojac = 0;
        [DataMember]
        public int Id { get; set; }


        public Automobil()
        {
            Id = brojac++;
        }

        public Automobil(string marka, string model, int cena, int godiste)
        {
            Marka = marka;
            Model = model;
            Cena = cena;
            Godiste = godiste;
            Id = brojac++;

        }
        public override string ToString()
        {
            return $"Automobil {Marka} {Model} godista {Godiste} kosta {Cena}";
        }
    }
}
