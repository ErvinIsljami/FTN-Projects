using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Model
{
    public class Type
    {
        private string name;
        private string imgUri;

        public Type(string name, string imgUri)
        {
            this.name = name;
            this.imgUri = imgUri;
        }

        public string Name { get => name; set => name = value; }
        public string ImgUri { get => imgUri; set => imgUri = value; }

        public override string ToString()
        {
            return name;
        }
    }
}
