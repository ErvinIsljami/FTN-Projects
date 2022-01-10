using HCI_MapaManifestacijaGrada.HelperModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace HCI_MapaManifestacijaGrada.Controller
{
	class MapItemCtrl
	{
		private List<MapItem> points;
		private readonly string datoteka;

		public MapItemCtrl()
		{
			datoteka = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "points.data");
			Deserialize();
		}

		public List<MapItem> FindAll()
		{
			return new List<MapItem>(points);
		}

		public bool Save(MapItem point)
		{
			foreach(MapItem p in points)
			{
				if(p.Manifestation.JedinstvenaOznaka == point.Manifestation.JedinstvenaOznaka)
				{
					return false;
				}
			}
			points.Add(point);
			Serialize();
			return true;
		}

		public void Delete(string pointManifestationId)
		{
			for(int i = 0; i < points.Count; i++)
			{
				if(points[i].Manifestation.JedinstvenaOznaka == pointManifestationId)
				{
					points.Remove(points[i]);
				}
			}
			Serialize();
		}

		public void Change(MapItem point)
		{
			for(int i = 0; i < points.Count; i++)
			{
				if(points[i].Manifestation.JedinstvenaOznaka == point.Manifestation.JedinstvenaOznaka)
				{
					points[i].X = point.X;
					points[i].Y = point.Y;
					points[i].Manifestation.JedinstvenaOznaka = point.Manifestation.JedinstvenaOznaka;
					points[i].Manifestation.Ime = point.Manifestation.Ime;
					points[i].Manifestation.Opis = point.Manifestation.Opis;
					points[i].Manifestation.Etiketa = point.Manifestation.Etiketa;
					points[i].Manifestation.Tip = point.Manifestation.Tip;
					points[i].Manifestation.Alkohol = point.Manifestation.Alkohol;
					points[i].Manifestation.Ikona = point.Manifestation.Ikona;
					points[i].Manifestation.Hendikepirani = point.Manifestation.Hendikepirani;
					points[i].Manifestation.Pušenje = point.Manifestation.Pušenje;
					points[i].Manifestation.Unutra = point.Manifestation.Unutra;
					points[i].Manifestation.Cena = point.Manifestation.Cena;
					points[i].Manifestation.Publika = point.Manifestation.Publika;
					points[i].Manifestation.Datum = point.Manifestation.Datum;
					Serialize();
				}
			}
		}

		private void Serialize()
		{
			using(FileStream stream = File.Open(datoteka, FileMode.OpenOrCreate))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, points);
			}
		}

		private void Deserialize()
		{
			if (File.Exists(datoteka))
			{
				using (FileStream stream = File.Open(datoteka, FileMode.Open))
				{
					BinaryFormatter formatter = new BinaryFormatter();
					points = (List<MapItem>)formatter.Deserialize(stream);
				}
			}
			else points = new List<MapItem>();
		}
	}
}
