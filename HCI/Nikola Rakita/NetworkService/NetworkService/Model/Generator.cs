using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class Generator : BindableBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private Tip tip;
        private double valuee;
        public double Valuee
        {
            get { return valuee; }
            set
            {
                if (valuee != value)
                {
                    valuee = value;
                    OnPropertyChanged("Valuee");
                }
            }
        }
        public Tip Tip {
            get { return tip; }
            set
            {
                if (tip != value)
                {
                    tip = value;
                    OnPropertyChanged("Tip");
                }
            }
        }
        public Generator()
        {
        
        }
        public Generator(int id, string name, Tip tip, double value)
        {
            this.Id = id;
            this.Name = name;
            this.Tip = tip;
            this.Valuee = value;
        }

    }
}
