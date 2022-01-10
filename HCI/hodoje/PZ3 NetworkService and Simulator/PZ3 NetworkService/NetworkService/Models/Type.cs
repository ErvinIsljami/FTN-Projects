using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Models
{
    [Serializable]
    public class Type
    {
        public string NAME { get; set; }
        public string IMG_URL { get; set; }

        public Type() { }

        public Type(string name, string img_url)
        {
            NAME = name;
            IMG_URL = img_url;
        }

        public override string ToString()
        {
            return NAME;
        }
    }
}
