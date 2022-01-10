using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Model
{
    public class Tip : BindableBase
    {
        private string name;
        private string img_Src;
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
        public string Img_Src
        {
            get { return img_Src; }
            set
            {
                if (img_Src != value)
                {
                    img_Src = value;
                    OnPropertyChanged("Img_Src");
                }
            }
        }

        public Tip() { }

        public Tip(string name, string img)
        {
            this.Name = name;
            this.Img_Src = img;
        }
    }
}
