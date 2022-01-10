using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_MapaManifestacijaGrada.Model
{
    [Serializable]
    public class ManifestationType
    {
        private String id;
        private String name;
        private String description;
        private String icon;
        private String iconName;

        public ManifestationType() { }
        public ManifestationType(String id, String name, String description, String icon, String iconName)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.icon = icon;
            this.iconName = iconName;
        }

        public String JedinstvenaOznaka
        {
            get { return id; }
            set { id = value; }
        }

        public String Ime
        {
            get { return name; }
            set { name = value; }
        }

        public String Opis
        {
            get { return description; }
            set { description = value; }
        }

        public String Ikona
        {
            get { return icon; }
            set { icon = value; }
        }

        public String ImeIkonice
        {
            get { return iconName; }
            set { iconName = value; }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
