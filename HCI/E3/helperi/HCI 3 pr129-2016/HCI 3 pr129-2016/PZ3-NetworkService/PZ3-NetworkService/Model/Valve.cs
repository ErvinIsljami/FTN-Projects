using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Model
{
    public class Valve : BindableBase
    {
        private int id;
        private string name;
        private ValveType valveType;
        private double val;

        public int ID
        {
            get { return id; }
            set
            {
				SetProperty(ref id, value);
            }
        }
        
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public ValveType ValveType
        {
            get { return valveType; }
            set
            {
                if (valveType != value)
                {
                    valveType = value;
                    OnPropertyChanged("ValveType");
                }
            }
        }

        public double Val
        {
            get { return val; }
            set
            {
                if (val != value)
                {
                    val = value;
                    OnPropertyChanged("Val");
                }
            }
        }
        public override string ToString()
        {
            return Name;
        }

    }
}
