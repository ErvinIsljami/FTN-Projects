using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezba6
{
    public class Slika
    {
        public String imageUri { get; set; }

        public Slika()
        {
            imageUri = "";
        }

        public Slika(String uri)
        {
            imageUri = uri;
        }
    }
}
