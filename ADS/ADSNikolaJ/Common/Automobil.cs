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
        public int Cena { get; set; }
        [DataMember]
        public string Marka { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string BrojSasije { get; set; }

        public Automobil(int cena, string marka, string model, string brojSasije)
        {
            Cena = cena;
            Marka = marka;
            Model = model;
            BrojSasije = brojSasije;
        }

        public Automobil()
        {
        }

        public override string ToString()
        {
            return $"Automobil {Marka} {Model} ima {BrojSasije} broj sasije i kosta {Cena}.";
        }
    }
}
