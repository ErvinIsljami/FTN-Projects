using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR101_2015.Model
{
    public class Reaktor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }    // trenutno stanje, tj. posljednje od stanja iz liste values
        public string ImagePath { get; set; }
        public List<int> Values { get; set; }


        public Reaktor(int idd, string ime, string tip)
        {
            Id = idd;
            Name = ime;
            Type = tip;
            Value = 0;
            switch (tip)
            {
                case "FUZIONI": ImagePath = @"C:\Users\boske\source\repos\PR101-2015\PR101-2015\slike\fuzioni.jpg"; break;
                case "TERMALNI": ImagePath = @"C:\Users\boske\source\repos\PR101-2015\PR101-2015\slike\termalni.png"; break;
                case "TIP3": ImagePath = @"C:\Users\boske\source\repos\PR101-2015\PR101-2015\slike\tip3.jpg"; break;
            }
            Values = new List<int>();


        }


        public Reaktor()
        {
            Id = 0;
            Name = "";
            Type = "";
            Value = 0;
            ImagePath = "";
            Values = new List<int>();
        }

        public override string ToString()
        {
            return Id + " " + Name + " " + Type + " " + ReadValues() + "\n";
        }

        public string ReadValues()
        {
            string pom = "";

            for (int i = 0; i < Values.Count(); i++)
            {
                pom += "\n\t\t\t\tdate: " + DateTime.Now.ToString() + " value: " + Values[i] + "\n";
            }

            return pom;
        }
    }
}
