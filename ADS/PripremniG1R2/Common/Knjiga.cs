using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Knjiga
    {
        string naziv;
        double cena;
        string isbn;

        public Knjiga()
        {
        }

        public Knjiga(string naziv, double cena, string isbn)
        {
            this.Naziv = naziv;
            this.Cena = cena;
            this.Isbn = isbn;
        }

        [DataMember]
        public string Naziv { get => naziv; set => naziv = value; }
        [DataMember]
        public double Cena { get => cena; set => cena = value; }
        [DataMember]
        public string Isbn { get => isbn; set => isbn = value; }

        public override string ToString()
        {
            return $"Knjiga: {naziv} kosta {cena}, isbn: {isbn}";
        }
    }
}