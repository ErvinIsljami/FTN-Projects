using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Laptop
    {
        private string model;
        private string marka;
        private double cena;
        private int id;
        private static int brojac = 0;

        [DataMember]
        public string Model { get => model; set => model = value; }
        [DataMember]
        public string Marka { get => marka; set => marka = value; }
        [DataMember]
        public double Cena { get => cena; set => cena = value; }
        [DataMember]
        public int Id { get => id; set => id = value; }
        [DataMember]
        public static int Brojac { get => brojac; set => brojac = value; }


        public Laptop(string model, string marka, double cena)
        {
            this.Model = model;
            this.Marka = marka;
            this.Cena = cena;
            this.Id = Brojac++;
        }
        public Laptop()
        {
            Id = Brojac++;
        }

        public override string ToString()
        {
            return $"Laptop {marka} {model} kosta {cena} a ima id {id}.";
        }
    }
}
