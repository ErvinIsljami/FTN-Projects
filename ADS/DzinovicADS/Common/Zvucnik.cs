using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Zvucnik
    {
        string marka;
        string model;
        int snaga;
        int jacinaUDB;
        int cena;
        int id;

        public Zvucnik()
        {
        }

        public Zvucnik(string marka, string model, int snaga, int jacinaUDB, int cena, int id)
        {
            this.Marka = marka;
            this.Model = model;
            this.Snaga = snaga;
            this.JacinaUDB = jacinaUDB;
            this.Cena = cena;
            this.Id = id;
        }

        [DataMember]
        public string Marka { get => marka; set => marka = value; }
        [DataMember]
        public string Model { get => model; set => model = value; }
        [DataMember]
        public int Snaga { get => snaga; set => snaga = value; }
        [DataMember]
        public int JacinaUDB { get => jacinaUDB; set => jacinaUDB = value; }
        [DataMember]
        public int Cena { get => cena; set => cena = value; }
        [DataMember]
        public int Id { get => id; set => id = value; }

        public override string ToString()
        {
            return $"Zvucnik {model} {marka} snaga: {snaga}w, {jacinaUDB} dB";
        }
    }
}
