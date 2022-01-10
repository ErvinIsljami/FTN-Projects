using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HCI_MapaManifestacijaGrada.Tutorial;
using HCI_MapaManifestacijaGrada.MyHelp;
using HCI_MapaManifestacijaGrada.Model;
using HCI_MapaManifestacijaGrada.HelperModels;
using HCI_MapaManifestacijaGrada.Controller;
using System.Collections.ObjectModel;
using System.IO;

namespace HCI_MapaManifestacijaGrada.View
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : Window
    {
		// Controllers used for data serialization and deserialization
		ManifestationCtrl manifestationCtrl = new ManifestationCtrl();
		ManifestationTypeCtrl manifestationTypeCtrl = new ManifestationTypeCtrl();
		MapItemCtrl mapItemCtrl = new MapItemCtrl();
		// Lists used to store all the needed data
		public ObservableCollection<ManifestationType> Types { get; set; }
		public ObservableCollection<Manifestation> Manifestations { get; set; }
		public ObservableCollection<CustomTreeViewItem> TreeViewItems { get; set; }
		public List<MapItem> Points { get; set; }

		public Map()
		{
			// Temp lists are used for null-checks
			// Get all manifestations
			List<Manifestation> tempList = manifestationCtrl.FindAll();
			if (tempList == null)
			{
				Manifestations = new ObservableCollection<Manifestation>();
			}
			else
			{
				Manifestations = new ObservableCollection<Manifestation>();
				foreach (var manifestacija in tempList)
				{
					Manifestations.Add(manifestacija);
				}
			}

			// Get all manifestations types
			List<ManifestationType> listaTipovaManifestacija = manifestationTypeCtrl.FindAll();
			if (listaTipovaManifestacija == null)
			{
				Types = new ObservableCollection<ManifestationType>();
			}
			else
			{
				Types = new ObservableCollection<ManifestationType>();
				foreach (var tipManifestacije in listaTipovaManifestacija)
				{
					Types.Add(tipManifestacije);
				}
			}

			// Using manifestation and manifestation types create a list of CustomTreeViewItems
			// that hold all the needed data for the TreeView
			TreeViewItems = new ObservableCollection<CustomTreeViewItem>();
			foreach (var tipManifestacije in Types)
			{
				var treeViewItem = new CustomTreeViewItem();
				treeViewItem.ManifestationType = tipManifestacije;

				foreach (var manifestacija in Manifestations)
				{
					if (manifestacija.Tip.JedinstvenaOznaka == tipManifestacije.JedinstvenaOznaka)
					{
						treeViewItem.Manifestations.Add(manifestacija);
					}
				}

				TreeViewItems.Add(treeViewItem);
			}

			// Set this window as the DataContext of this window (used for Data Binding)
			DataContext = this;
			InitializeComponent();
		}

		// This event handler is invoked each time an item is selected in the TreeView
		// Note that this will be invoked no matter if it's a Hierarchical item or not
		// (in the example in CustomTreeViewItem class, this means that this will be invoked
		// regardless of that if a Family is selected or a Member
		private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (TreeView.SelectedItem != null && TreeView.SelectedItem is Manifestation)
			{
				Manifestation manifestation = TreeView.SelectedItem as Manifestation;
				// DataObject object is used for transfering drag and drop data from source to destination
				DataObject dragData = new DataObject(typeof(Manifestation), manifestation);
				// DoDragDrop starts the Drag and Drop process, we define the source, drag and drop data and DragDropEffects
				DragDrop.DoDragDrop(TreeView, dragData, DragDropEffects.Copy);
			}
		}

		// This event handler is invoked each time a mouse button is down on the StackPanel
		// this Stackpanel is a part of DataTemplate for each TreeViewItem child
		// Using this because if an item in TreeView is already selected, the "SelectedItemChanged" event won't fire
		// so we're calling it explicitly
		private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				// Invoke the TreeView_SelectedItemChanged event handler manually
				Dispatcher.BeginInvoke(new EventHandler<RoutedPropertyChangedEventArgs<object>>(TreeView_SelectedItemChanged), sender, null);
			}
		}

		// This event handler is invoked when an item is dragged on the map
		// used for adding a manifestation image to Map
		private void Mapa_Drop(object sender, DragEventArgs e)
		{
			// Extract the needed data from DataObject
			Manifestation manifestacija = e.Data.GetData(typeof(Manifestation)) as Manifestation;

			// Check if manifestation is already dragged on the map
			foreach (MapItem point in Points)
			{
				if (point.Manifestation.JedinstvenaOznaka == manifestacija.JedinstvenaOznaka)
				{
					return;
				}
			}

			// If not, check if manifestation image collides with others
			double xPos = e.GetPosition(Mapa).X;
			double yPos = e.GetPosition(Mapa).Y;

			// Iterate over all the items on the Map
			foreach (UIElement child in Mapa.Children)
			{
				// Get items x and y coordinates
				double childTop = (double)child.GetValue(Canvas.TopProperty);
				double childLeft = (double)child.GetValue(Canvas.LeftProperty);
				// Cast the child to Image so we can get the width and height
				Image childImg = child as Image;
				// Rect class contains methods for intersection checking so we use that
				// We're using the ActualWidth and ActualHight because the width and height showed on the view
				// will probably won't be de defined Width and Height for that image because the image
				// probably won't be a perfect square but something different so it won't fit perfectly in the defined
				// Widtha and Height sizes
				// Rect for each image on the map
				Rect childRect = new Rect(childLeft, childTop, childImg.ActualWidth, childImg.ActualHeight);
				// Rect for a potential image to put on the map
				Rect currentPointRect = new Rect(xPos, yPos, childImg.ActualWidth, childImg.ActualHeight);

				// Check IsIntersectingInside
				Rect intersectionArea = Rect.Intersect(childRect, currentPointRect);
				if (intersectionArea.Width > 0 && intersectionArea.Height > 0)
				{
					// Check if childRect intersects with current
					if (childRect.IntersectsWith(currentPointRect))
					{
						return;
					}
				}
			}

			// Set up the image, tooltip and add to Map
			BitmapImage bitmap = new BitmapImage();
			string relativePath = $@"..\..\Resources\{manifestacija.Ikona}";
			string absolutePath = System.IO.Path.GetFullPath(relativePath);
			string localPath = new Uri(absolutePath).LocalPath;
			if (File.Exists(localPath))
			{
				bitmap = new BitmapImage(new Uri(localPath));
			}
			Image img = new Image();
			img.Source = bitmap;
			img.Width = 20;
			img.Height = 20;
			img.ToolTip = $"Oznaka: [{manifestacija.JedinstvenaOznaka}] Ime: [{manifestacija.Ime}]{Environment.NewLine}(Right click on icon to remove it.)";

			Mapa.Children.Add(img);

			Canvas.SetLeft(img, xPos);
			Canvas.SetTop(img, yPos);

			// Create new MapItem
			MapItem p = new MapItem { Manifestation = manifestacija, X = xPos, Y = yPos };
			// Serialize it
			mapItemCtrl.Save(p);
			// Update the list that contains all the points
			List<MapItem> listaPointova = mapItemCtrl.FindAll();
			if (listaPointova == null)
			{
				Points = new List<MapItem>();
			}
			else
			{
				Points = listaPointova;
			}
		}

		// This event handler is invoked when the Canvas is loaded
		private void Mapa_Loaded(object sender, RoutedEventArgs e)
		{
			// Get all the points from file
			List<MapItem> listaPointova = mapItemCtrl.FindAll();
			if (listaPointova == null)
			{
				Points = new List<MapItem>();
			}
			else
			{
				Points = listaPointova;

				// For each points in list of points get all the needed data
				foreach (var pointItem in Points)
				{
					// Set up the images and tooltips and add to Map
					BitmapImage bitmap = new BitmapImage();
					string relativePath = $@"..\..\Resources\{pointItem.Manifestation.Ikona}";
					string absolutePath = System.IO.Path.GetFullPath(relativePath);
					string localPath = new Uri(absolutePath).LocalPath;
					if (File.Exists(localPath))
					{
						bitmap = new BitmapImage(new Uri(localPath));
					}
					Image img = new Image();
					img.Source = bitmap;
					img.Width = 20;
					img.Height = 20;
					img.ToolTip = $"Oznaka: [{pointItem.Manifestation.JedinstvenaOznaka}] Ime: [{pointItem.Manifestation.Ime}]{Environment.NewLine}(Right click on icon to remove it.)";

					Mapa.Children.Add(img);

					Canvas.SetLeft(img, pointItem.X);
					Canvas.SetTop(img, pointItem.Y);
				}
			}
		}

		// This event handler is invoked each time a mouse button is down on the Map
		// and is used to remove an image from the map
		private void Mapa_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if(e.RightButton == MouseButtonState.Pressed)
			{
				// Checks if the item over which the button is pressed is an image
				if(e.Source is Image)
				{
					// This holds the result image that will be removed from the map
					Image resultImg = null;

					// Extract the manifestation JedinstvenaOznaka property from image ToolTip
					Image img = e.Source as Image;
					string tooltip = img.ToolTip.ToString();
					int indexOfFirstLeftBracket = tooltip.IndexOf('[');
					int indexOfFirstRightBracket = tooltip.IndexOf(']');
					string manifestationJedinstvenaOznaka = tooltip.Substring(indexOfFirstLeftBracket + 1, indexOfFirstRightBracket - indexOfFirstLeftBracket - 1);

					// Compare each image on Map with selected image
					foreach(UIElement child in Mapa.Children)
					{
						Image imgg = child as Image;
						string tt = imgg.ToolTip.ToString();
						int idxOfFirstLeftBracket = tt.IndexOf('[');
						int idxOfFirstRightBracket = tt.IndexOf(']');
						string childJedinstvenaOznaka = tt.Substring(idxOfFirstLeftBracket + 1, idxOfFirstRightBracket - idxOfFirstLeftBracket - 1);
						
						// If an image is found, set the resultImg and stop the loop
						if(manifestationJedinstvenaOznaka == childJedinstvenaOznaka)
						{
							resultImg = imgg;
							break;
						}
					}

					// If a result is found
					if (resultImg != null)
					{
						// Remove an image from Map remove the coresponding item from Points list
						Mapa.Children.Remove(resultImg as UIElement);
						// And serialize the new data
						mapItemCtrl.Delete(manifestationJedinstvenaOznaka);
						// After that reset the list of points
						List<MapItem> listaPointova = mapItemCtrl.FindAll();
						if (listaPointova == null)
						{
							Points = new List<MapItem>();
						}
						else
						{
							Points = listaPointova;
						}
					}
				}
			}
		}
	}
}
