using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Racun
    {
        private string brojRacuna;
        private double stanjeRacuna;
        private bool isDevizni;
        private int id;
        private static int brojac = 0;

        [DataMember]                                                                       
        public string BrojRacuna { get => brojRacuna; set => brojRacuna = value; }         
        [DataMember]                                                                       
        public double StanjeRacuna { get => stanjeRacuna; set => stanjeRacuna = value; }   
        [DataMember]                                                                       
        public bool IsDevizni { get => isDevizni; set => isDevizni = value; }              
        [DataMember]                                                                       
        public int Id { get => id; set => id = value; }                                    
        [DataMember]                                                                       
        public static int Brojac { get => brojac; set => brojac = value; }                 

        public Racun()
        {
        }

        public Racun(string brojRacuna, double stanjeRacuna, bool isDevizni)
        {
            this.BrojRacuna = brojRacuna;
            this.StanjeRacuna = stanjeRacuna;
            this.IsDevizni = isDevizni;
            this.Id = Brojac++;
        }

        public override string ToString()
        {
            return $"Racun: {brojRacuna}, ima {stanjeRacuna}";
        }
    }
}
