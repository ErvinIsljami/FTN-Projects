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
    class ManifestationTypeCtrl
    {
        private List<ManifestationType> manifestationTypes;
        private readonly string dataFile;

        public ManifestationTypeCtrl()
        {
            dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ManifestationType.data");
            Deserialize();
        }

        public List<ManifestationType> FindAll()
        {
            return new List<ManifestationType>(manifestationTypes);
        }

        public ManifestationType FindById(string manifestationTypeId)
        {
            for (int i = 0; i < manifestationTypes.Count; i++)
            {
                if (manifestationTypes[i].JedinstvenaOznaka.Equals(manifestationTypeId))
                {
                    return manifestationTypes[i];
                }
            }

            return null;
        }

        public bool Save(ManifestationType manifestationType)
        {
            foreach (var mType in manifestationTypes)
            {
                if (mType.JedinstvenaOznaka.Equals(manifestationType.JedinstvenaOznaka))
                {
                    return false;
                }
            }

            manifestationTypes.Add(manifestationType);
            Serialize();

            return true;
        }

        public void Delete(String manifestationTypeId)
        {
            for (int i = 0; i < manifestationTypes.Count; i++)
            {
                if (manifestationTypes[i].JedinstvenaOznaka.Equals(manifestationTypeId))
                {
                    manifestationTypes.Remove(manifestationTypes[i]);
                }
            }
            Serialize();
        }

        public void Change(ManifestationType manifestationType)
        {
            for (int i = 0; i < manifestationTypes.Count; i++)
            {
                if (manifestationTypes[i].JedinstvenaOznaka.Equals(manifestationType.JedinstvenaOznaka))
                {
                    manifestationTypes[i].Ime = manifestationType.Ime;
                    manifestationTypes[i].Opis = manifestationType.Opis;
                    manifestationTypes[i].Ikona = manifestationType.Ikona;
                    manifestationTypes[i].ImeIkonice = manifestationType.ImeIkonice;

                    Serialize();
                }
            }
        }

        private void Serialize()
        {
            using (FileStream stream = File.Open(dataFile, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, manifestationTypes);
            }
        }

        private void Deserialize()
        {
            if (File.Exists(dataFile))
            {
                using (FileStream stream = File.Open(dataFile, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    manifestationTypes = (List<ManifestationType>)formatter.Deserialize(stream);
                }
            }
            else manifestationTypes = new List<ManifestationType>();
        }
    }
}
