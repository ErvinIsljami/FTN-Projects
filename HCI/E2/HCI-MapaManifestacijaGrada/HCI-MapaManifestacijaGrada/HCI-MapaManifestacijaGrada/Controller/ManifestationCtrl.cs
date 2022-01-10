using HCI_MapaManifestacijaGrada.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace HCI_MapaManifestacijaGrada.Controller
{
    class ManifestationCtrl
    {
        private List<Manifestation> manifestations;
        private readonly string datoteka;

        public ManifestationCtrl()
        {
            datoteka = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Manifestation.data");
            Deserialize();
        }

        public List<Manifestation> FindAll()
        {
            return new List<Manifestation>(manifestations);
        }

        public bool Save(Manifestation manifestation)
        {
            foreach (var man in manifestations)
            {
                if (man.JedinstvenaOznaka.Equals(manifestation.JedinstvenaOznaka))
                {
                    return false;
                }
            }

            manifestations.Add(manifestation);
            Serialize();

            return true;
        }

        public void Delete(String manifestationId)
        {
            for (int i = 0; i < manifestations.Count; i++)
            {
                if (manifestations[i].JedinstvenaOznaka.Equals(manifestationId))
                {
                    manifestations.Remove(manifestations[i]);
                }
            }
            Serialize();
        }

        public void Change(Manifestation manifestation)
        {
            for (int i = 0; i < manifestations.Count; i++)
            {
                if (manifestations[i].JedinstvenaOznaka.Equals(manifestation.JedinstvenaOznaka))
                {
                    manifestations[i].JedinstvenaOznaka = manifestation.JedinstvenaOznaka;
                    manifestations[i].Ime = manifestation.Ime;
                    manifestations[i].Opis = manifestation.Opis;
                    manifestations[i].Etiketa = manifestation.Etiketa;
                    manifestations[i].Tip = manifestation.Tip;
                    manifestations[i].Alkohol = manifestation.Alkohol;
                    manifestations[i].Ikona = manifestation.Ikona;
                    manifestations[i].Hendikepirani = manifestation.Hendikepirani;
                    manifestations[i].Pušenje = manifestation.Pušenje;
                    manifestations[i].Unutra = manifestation.Unutra;
                    manifestations[i].Cena = manifestation.Cena;
                    manifestations[i].Publika = manifestation.Publika;
                    manifestations[i].Datum = manifestation.Datum;
                    Serialize();
                }
            }
        }

        private void Serialize()
        {
            using (FileStream stream = File.Open(datoteka, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, manifestations);
            }
        }
        private void Deserialize()
        {
            if (File.Exists(datoteka))
            {
                using (FileStream stream = File.Open(datoteka, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    manifestations = (List<Manifestation>)formatter.Deserialize(stream);
                }
            }
            else manifestations = new List<Manifestation>();
        }
    }
}
