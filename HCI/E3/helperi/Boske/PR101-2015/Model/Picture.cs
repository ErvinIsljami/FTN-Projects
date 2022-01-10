using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR101_2015.Model
{
    public class Picture
    {
        public String imageUri { get; set; }
        public Picture()
        {
            imageUri = "";
        }

        public Picture(String uri)
        {
            imageUri = uri;
        }

    }
}
