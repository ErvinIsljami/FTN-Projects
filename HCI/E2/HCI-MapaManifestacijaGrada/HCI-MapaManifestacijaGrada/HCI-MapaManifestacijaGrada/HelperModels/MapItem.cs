using HCI_MapaManifestacijaGrada.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCI_MapaManifestacijaGrada.HelperModels
{
	// Object of this class will represent a single manifestation on the map
	// It holds all the manifestation data and the x and y coordinate
	// In terms of images on the map, this data is used only to set up the Image and Image Tooltip
	[Serializable]
	public class MapItem
	{
		public Manifestation Manifestation { get; set; }
		public double X { get; set; }
		public double Y { get; set; }
	}
}
