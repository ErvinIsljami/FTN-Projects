using HCI_MapaManifestacijaGrada.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HCI_MapaManifestacijaGrada.HelperModels
{
	// Object of this class will represent a single HierarchicalItem in the TreeView
	// in other terms, it's an item that will have some children
	// an arror -> will be located on the left side of its DataTemplate
	// that will point out that it has children
	// We can view these data like this:
	//		A single object represents a Family and its members
	//		ManifestationType will be the Family class
	//		Manifestation will be the Member class
	//		So a single object contains data about a whole family and list of all members of that family
	public class CustomTreeViewItem
	{
		public ManifestationType ManifestationType { get; set; }
		public ObservableCollection<Manifestation> Manifestations { get; set; }

		public CustomTreeViewItem()
		{
			Manifestations = new ObservableCollection<Manifestation>();
		}
	}
}
