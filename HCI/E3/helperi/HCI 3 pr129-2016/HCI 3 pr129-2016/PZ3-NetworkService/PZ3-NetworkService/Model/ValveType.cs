using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Model
{
    public class ValveType : BindableBase
    {
        private string name;
        private string img_src;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Img_src
        {
            get
            {
                return img_src;
            }
            set
            {
                if (img_src != value)
                {
                    img_src = value;
                    OnPropertyChanged("Img_src");
                }
            }
        }
        public ValveType(string nm, string im)
        {
            name = nm;
            img_src = im;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
