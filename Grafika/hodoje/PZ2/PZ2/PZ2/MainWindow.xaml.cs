using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PZ2.Models;
using PZ2.Xml;

namespace PZ2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NetworkModel NetworkModel { get; set; }        
        private CustomXmlRW CustomXmlRW = new CustomXmlRW();
        private bool IsDataLoaded { get; set; }
        private GMapOverlay Markers { get; set; }
        private GMapOverlay Routes { get; set; }

        public MainWindow()
        {
            Markers = new GMapOverlay("markers");
            Routes = new GMapOverlay("routes");
            IsDataLoaded = false;
            ReadXmlAndConvertTemplate();
            InitializeComponent();
        }

        private void Gmap_Load(object sender, EventArgs e)
        {
            // If there is some problem with map loading, use this code
            // if it's not shown even with this, that means we messed something up
            //GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            //GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            gmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.MinZoom = 1;
            gmap.MaxZoom = 18;
            // Whole world zoom
            gmap.Zoom = 12;
            // Lets the map use the mousewheel to zoom
            gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // Lets the user drag the map
            gmap.CanDragMap = true;
            // Lets the user drag the map with the left mouse button
            gmap.DragButton = System.Windows.Forms.MouseButtons.Left;
            // Centers the map on Novi Sad
            double blX = 45.2325;
            double blY = 19.793909;
            double trX = 45.277031;
            double trY = 19.894459;
            gmap.Position = new PointLatLng((blX + trX) / 2, (blY + trY) / 2);
        }

        public async Task ReadXmlAndConvertTemplate()
        {
            Task readXmlTask = Task.Run(() =>
            {
                ReadXml();
            });

            await readXmlTask.ContinueWith(antecedent =>
            {
                ConvertUTMToDecimalAndAddMarkers();
                IsDataLoaded = true;
            });
        }

        public void ReadXml()
        {
            NetworkModel = CustomXmlRW.DeSerializeObject<NetworkModel>(@"../../Data/Geographic.xml");
        }

        public void ConvertUTMToDecimalAndAddMarkers()
        {
            foreach (var substation in NetworkModel.Substations)
            {
                UTMToLatLonConverter.ToLatLon(substation.X, substation.Y, 34, out double latitude, out double longitude);
                substation.X = longitude;
                substation.Y = latitude;
                GMapMarker marker = new GMarkerGoogle(new PointLatLng(substation.Y, substation.X), GMarkerGoogleType.green);
                marker.ToolTipText = $"Substation Id:{substation.Id}, Substation Name:{substation.Name}\nXCoord:{substation.X}, YCoord:{substation.Y}";
                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                Markers.Markers.Add(marker);
            }

            foreach (var node in NetworkModel.Nodes)
            {
                UTMToLatLonConverter.ToLatLon(node.X, node.Y, 34, out double latitude, out double longitude);
                node.X = longitude;
                node.Y = latitude;
                GMapMarker marker = new GMarkerGoogle(new PointLatLng(node.Y, node.X), GMarkerGoogleType.pink);
                marker.ToolTipText = $"Node Id:{node.Id}, Node Name:{node.Name}\nXCoord:{node.X}, YCoord:{node.Y}";
                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                Markers.Markers.Add(marker);
            }

            foreach (var sw in NetworkModel.Switches)
            {
                UTMToLatLonConverter.ToLatLon(sw.X, sw.Y, 34, out double latitude, out double longitude);
                sw.X = longitude;
                sw.Y = latitude;
                GMapMarker marker = new GMarkerGoogle(new PointLatLng(sw.Y, sw.X), GMarkerGoogleType.black_small);
                marker.ToolTipText = $"Switch Id:{sw.Id}, Switch Name:{sw.Name}\nXCoord:{sw.X}, YCoord:{sw.Y}";
                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                Markers.Markers.Add(marker);
            }

            foreach (var line in NetworkModel.Lines)
            {
                List<PointLatLng> points = new List<PointLatLng>();
                foreach (var point in line.Vertices)
                {
                    UTMToLatLonConverter.ToLatLon(point.X, point.Y, 34, out double latitude, out double longitude);
                    point.X = longitude;
                    point.Y = latitude;
                    points.Add(new PointLatLng(point.Y, point.X));
                }
                GMapRoute route = new GMapRoute(points, "");
                route.Stroke = new System.Drawing.Pen(System.Drawing.Color.Red, 3);
                Routes.Routes.Add(route);
            }
        }

        private void LoadMarkers_Click(object sender, RoutedEventArgs e)
        {
            if (IsDataLoaded)
            {
                if (gmap.Overlays.Count > 0)
                {
                    gmap.Overlays.Clear();
                }
                gmap.Overlays.Add(Markers);
                gmap.Overlays.Add(Routes);
            }
        }
    }
}
