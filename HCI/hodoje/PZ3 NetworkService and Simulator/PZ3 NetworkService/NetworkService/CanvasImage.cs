using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService
{
    class CanvasImage
    {
        public string ImgUri { get; set; }

        public CanvasImage()
        {
            ImgUri = "";
        }

        public CanvasImage(string uri)
        {
            ImgUri = uri;
        }
    }
}
