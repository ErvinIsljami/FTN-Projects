using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Classes
{
    [Serializable]
    public class Kalashnikov
    {
        public string Vendor { get; set; }
        public string Model { get; set; }
        public int OriginYear { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public Kalashnikov() { }
        public Kalashnikov(string vendor, string model, int originYear, string description, string imagePath)
        {
            Vendor = vendor;
            Model = model;
            OriginYear = originYear;
            Description = description;
            Image = imagePath;
        }
    }
}