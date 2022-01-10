using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    [Serializable]
    public class Student
    {
        public String Ime { get; set; }
        public String Prezime { get; set; }
        public char Pol { get; set; }
        public String Smer { get; set; }

        public Student()
        {

        }

        public Student(string ime, string prezime, char pol, string smer)
        {
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Smer = smer;
        }
    }
}
