using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Models
{
    public class Types
    {
        private Types() { }     //niko ne sme da pozove konstruktor

        private static Types Typess { get; set; }
        
        public static Types Instance
        {
            get
            {
                if(Typess == null)
                {
                    Typess = new Types();
                }
                return Typess;
            }
        }

        public List<Type> ListOfTypes = new List<Type>
        {
            new Type() { NAME = "Select type", IMG_URL = "" },
            new Type() { NAME = "IA", IMG_URL = "pack://application:,,,/Images/highway.png"},
            new Type() { NAME = "IB", IMG_URL = "pack://application:,,,/Images/motorway.png"}
        };
    }
}
