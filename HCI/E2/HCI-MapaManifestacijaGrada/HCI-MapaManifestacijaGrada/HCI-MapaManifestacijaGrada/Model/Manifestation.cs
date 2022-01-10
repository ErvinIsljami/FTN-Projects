using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_MapaManifestacijaGrada.Model
{
    [Serializable]
    public class Manifestation
    {
        public string JedinstvenaOznaka { get; set; }
        public string Ime { get; set; }
        public string Opis { get; set; }
        public string Alkohol { get; set; }
        public string Ikona { get; set; }
        public ManifestationType Tip { get; set; }
        public Tag Etiketa { get; set; }
        public bool Hendikepirani { get; set; }
        public bool Pušenje { get; set; }
        public bool Unutra { get; set; }
        public string Cena { get; set; }
        public string Publika { get; set; }
        public string Datum { get; set; }

        public Manifestation(string iD, string name, string description, string alcohol, string icon, ManifestationType manifestationType,
            Tag tag, bool hendicapped, bool smoking, bool inside, string price, string crawd, string dateOfManifestation)
        {
            JedinstvenaOznaka = iD;
            Ime = name;
            Opis = description;
            Alkohol = alcohol;
            Ikona = icon;
            Tip = manifestationType;
            Etiketa = tag;
            Hendikepirani = hendicapped;
            Pušenje = smoking;
            Unutra = inside;
            Cena = price;
            Publika = crawd;
            Datum = dateOfManifestation;
        }

        public Manifestation(string iD, string name, string description, string alcohol, ManifestationType manifestationType,
            Tag tag, bool hendicapped, bool smoking, bool inside, string price, string crawd, string dateOfManifestation)
        {
            JedinstvenaOznaka = iD;
            Ime = name;
            Opis = description;
            Alkohol = alcohol;
            Tip = manifestationType;
            Etiketa = tag;
            Hendikepirani = hendicapped;
            Pušenje = smoking;
            Unutra = inside;
            Cena = price;
            Publika = crawd;
            Datum = dateOfManifestation;
        }
    }
}
