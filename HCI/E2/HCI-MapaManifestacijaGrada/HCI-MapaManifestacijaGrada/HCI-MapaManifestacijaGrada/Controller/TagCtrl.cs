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
    class TagCtrl
    {
        public List<Tag> tags;
        private readonly string datoteka;

        public TagCtrl()
        {
            datoteka = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tag.data");
            Deserialize();
        }

        public List<Tag> FindAll()
        {
            return new List<Tag>(tags);
        }

        public Tag FindById(string tagId)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].JedinstvenaOznaka.Equals(tagId))
                {
                    return tags[i];
                }
            }

            return null;
        }

        public bool Save(Tag tag)
        {
            foreach (var t in tags)
            {
                if (t.JedinstvenaOznaka.Equals(tag.JedinstvenaOznaka))
                {
                    return false;
                }
            }

            tags.Add(tag);
            Serialize();

            return true;
        }

        public void Delete(String tagId)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].JedinstvenaOznaka.Equals(tagId))
                {
                    tags.Remove(tags[i]);
                }
            }
            Serialize();
        }

        public void Change(Tag tag)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].JedinstvenaOznaka.Equals(tag.JedinstvenaOznaka))
                {
                    tags[i].JedinstvenaOznaka = tag.JedinstvenaOznaka;
                    tags[i].Boja = tag.Boja;
                    tags[i].Opis = tag.Opis;
                    Serialize();
                }
            }
        }

        private void Serialize()
        {
            using (FileStream stream = File.Open(datoteka, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, tags);
            }
        }
        private void Deserialize()
        {
            if (File.Exists(datoteka))
            {
                using (FileStream stream = File.Open(datoteka, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    tags = (List<Tag>)formatter.Deserialize(stream);
                }
            }
            else tags = new List<Tag>();
        }
    }
}
