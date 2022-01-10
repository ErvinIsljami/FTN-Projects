using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_MapaManifestacijaGrada.Model
{
    [Serializable]
    public class Tag
    {
        private String id;
        private String selectedColor;
        private String description;


        public Tag() { }
        public Tag(string id, String color, string description)
        {
            this.id = id;
            this.selectedColor = color;
            this.description = description;
        }

        public String JedinstvenaOznaka
        {
            get { return id; }
            set { id = value; }
        }

        public String Boja
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        public String Opis
        {
            get { return description; }
            set { description = value; }
        }

        public override string ToString()
        {
            return selectedColor;
        }
    }
}
