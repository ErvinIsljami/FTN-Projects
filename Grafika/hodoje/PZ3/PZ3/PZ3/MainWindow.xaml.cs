using PZ3.Models;
using PZ3.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PZ3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NetworkModel NetworkModel { get; set; }
        public CustomXmlRW CustomXmlRW { get; set; }
        public Dictionary<Tuple<int, int>, GridPoint> GridPoints { get; set; }
        // These three lists will contain elements that are added to the grid
        // so we can draw their lines
        public List<SubstationEntity> Substations { get; set; }
        public List<NodeEntity> Nodes { get; set; }
        public List<SwitchEntity> Switches { get; set; }
        public List<Tuple<Line, Line>> DrawnLines { get; set; }
        // LineEntity is the line in NetworkModel.Lines
        // First Line is the first part of the line - horizontal
        // Second Line is the second part of the line - vertical
        public List<Tuple<LineEntity, Line, Line>> NonModifiedLines { get; set; }
        public bool IsDataLoaded { get; set; }
        private int _singleTileSize = 1;
        private double _ellipseWidth = 1.0;
        private double _ellipseHeight = 1.0;
        private double _minimalPointX = 0.0;
        private double _minimalPointY = 0.0;
        private double _maximalPointX = 0.0;
        private double _maximalPointY = 0.0;
        private double _intervalX = 0.0;
        private double _intervalY = 0.0;

        public MainWindow()
        {
            CustomXmlRW = new CustomXmlRW();
            GridPoints = new Dictionary<Tuple<int, int>, GridPoint>();
            Substations = new List<SubstationEntity>();
            Nodes = new List<NodeEntity>();
            Switches = new List<SwitchEntity>();
            DrawnLines = new List<Tuple<Line, Line>>();
            NonModifiedLines = new List<Tuple<LineEntity, Line, Line>>();
            IsDataLoaded = false;
            ReadXmlAndConvertAndFindMinimalsAndMaximalsTemplate();
            InitializeComponent();
            InitializeGrid();
        }

        public void InitializeGrid()
        {
            for(int i = 0; i <= GridCanvas.Width; i+= _singleTileSize)
            {
                for(int j = 0; j <= GridCanvas.Height; j+= _singleTileSize)
                {
                    GridPoint gridPoint = new GridPoint(i, j);
                    GridPoints.Add(gridPoint.Key, gridPoint);
                }
            }
        }

        public void FindMinimalAndMaximalPoints()
        {
            List<double> minimalXList = new List<double>(3);
            List<double> minimalYList = new List<double>(3);
            List<double> maximalXList = new List<double>(3);
            List<double> maximalYList = new List<double>(3);

            // Find minimal x's and y's for each type of grid element
            minimalXList.Add(NetworkModel.Substations.Min(s => s.X));
            minimalXList.Add(NetworkModel.Nodes.Min(n => n.X));
            minimalXList.Add(NetworkModel.Switches.Min(sw => sw.X));
            minimalYList.Add(NetworkModel.Substations.Min(s => s.Y));
            minimalYList.Add(NetworkModel.Nodes.Min(n => n.Y));
            minimalYList.Add(NetworkModel.Switches.Min(sw => sw.Y));

            // Find maximal x's and y's for each type of grid element
            maximalXList.Add(NetworkModel.Substations.Max(s => s.X));
            maximalXList.Add(NetworkModel.Nodes.Max(n => n.X));
            maximalXList.Add(NetworkModel.Switches.Max(sw => sw.X));
            maximalYList.Add(NetworkModel.Substations.Max(s => s.Y));
            maximalYList.Add(NetworkModel.Nodes.Max(n => n.Y));
            maximalYList.Add(NetworkModel.Switches.Max(sw => sw.Y));

            // Set up the referential minimal point
            _minimalPointX = minimalXList.Min();
            _minimalPointY = minimalYList.Min();

            // Set up the referential maximal point
            _maximalPointX = maximalXList.Max();
            _maximalPointY = maximalYList.Max();

            // Save intervals between minimal and maximal point on both axes
            _intervalX = _maximalPointX - _minimalPointX;
            _intervalY = _maximalPointY - _minimalPointY;
        }

        public async Task ReadXmlAndConvertAndFindMinimalsAndMaximalsTemplate()
        {
            Task readXmlTask = Task.Run(() =>
            {
                ReadXml();
            }).ContinueWith(antecedent =>
            {
                //ConvertUTMToDecimal();
                IsDataLoaded = true;
            }).ContinueWith(antecedent =>
            {
                FindMinimalAndMaximalPoints();
            });
        }

        public void ReadXml()
        {
            NetworkModel = CustomXmlRW.DeSerializeObject<NetworkModel>(@"../../DataFiles/Geographic.xml");
        }

        //public void ConvertUTMToDecimal()
        //{
        //    foreach (var substation in NetworkModel.Substations)
        //    {
        //        UTMToLatLonConverter.ToLatLon(substation.X, substation.Y, 34, out double latitude, out double longitude);
        //        substation.X = longitude;
        //        substation.Y = latitude;
        //    }

        //    foreach (var node in NetworkModel.Nodes)
        //    {
        //        UTMToLatLonConverter.ToLatLon(node.X, node.Y, 34, out double latitude, out double longitude);
        //        node.X = longitude;
        //        node.Y = latitude;
        //    }

        //    foreach (var sw in NetworkModel.Switches)
        //    {
        //        UTMToLatLonConverter.ToLatLon(sw.X, sw.Y, 34, out double latitude, out double longitude);
        //        sw.X = longitude;
        //        sw.Y = latitude;
        //    }

        //    // Lines are omitted because we are using FirstEnd and SecondEnd properties that give us
        //    // beginning and the end of the lines
        //}

        public void ConvertLatLongToLocalCoordinates(double longX, double latY, out double x, out double y)
        {
            // Move will store the percentage in decimal number that will with multiplication with the grid
            // give us the coordinates on the grid
            double moveX = (longX - _minimalPointX) / (_intervalX);
            double moveY = (latY - _minimalPointY) / (_intervalY);
            x = GridCanvas.Width * moveX;
            y = GridCanvas.Height * moveY;
        }

        public void DrawSubstations()
        {
            foreach (var substation in NetworkModel.Substations)
            {
                Ellipse el = new Ellipse();
                el.Width = _ellipseWidth;
                el.Height = _ellipseHeight;
                el.StrokeThickness = 0;
                el.Fill = Brushes.Green;
                el.ToolTip = $"Substation Id:{substation.Id}, Substation Name:{substation.Name}\nXCoord:{substation.X}, YCoord:{substation.Y}";
                ConvertLatLongToLocalCoordinates(substation.X, substation.Y, out double x, out double y);

                double roundedX = Math.Round(x);
                double roundedY = Math.Round(y);

                var point = GridPoints[Tuple.Create<int, int>((int)roundedX, (int)roundedY)];

                if (point.Taken)
                {
                    int pixelDistance = 1;
                    GridPoint newPoint;
                    while ((newPoint = CheckIfCanPlace(point, pixelDistance)) == null)
                    {
                        pixelDistance++;
                    }
                    substation.X = newPoint.X;
                    substation.Y = newPoint.Y;
                    newPoint.Taken = true;
                    Canvas.SetLeft(el, newPoint.X);
                    Canvas.SetBottom(el, newPoint.Y);
                    GridCanvas.Children.Add(el);
                    Substations.Add(substation);
                }
                else
                {
                    substation.X = point.X;
                    substation.Y = point.Y;
                    point.Taken = true;
                    Canvas.SetLeft(el, point.X);
                    Canvas.SetBottom(el, point.Y);
                    GridCanvas.Children.Add(el);
                    Substations.Add(substation);
                }
            }
        }

        public void DrawNodes()
        {
            foreach (var node in NetworkModel.Nodes)
            {
                Ellipse el = new Ellipse();
                el.Width = _ellipseWidth;
                el.Height = _ellipseHeight;
                el.StrokeThickness = 0;
                el.Fill = Brushes.Red;
                el.ToolTip = $"Node Id:{node.Id}, Node Name:{node.Name}\nXCoord:{node.X}, YCoord:{node.Y}";
                ConvertLatLongToLocalCoordinates(node.X, node.Y, out double x, out double y);

                double roundedX = Math.Round(x);
                double roundedY = Math.Round(y);

                var point = GridPoints[Tuple.Create<int, int>((int)roundedX, (int)roundedY)];

                if (point.Taken)
                {
                    int pixelDistance = 1;
                    GridPoint newPoint;
                    while ((newPoint = CheckIfCanPlace(point, pixelDistance)) == null)
                    {
                        pixelDistance++;
                    }
                    node.X = newPoint.X;
                    node.Y = newPoint.Y;
                    newPoint.Taken = true;
                    Canvas.SetLeft(el, newPoint.X);
                    Canvas.SetBottom(el, newPoint.Y);
                    GridCanvas.Children.Add(el);
                    Nodes.Add(node);
                }
                else
                {
                    node.X = point.X;
                    node.Y = point.Y;
                    point.Taken = true;
                    Canvas.SetLeft(el, point.X);
                    Canvas.SetBottom(el, point.Y);
                    GridCanvas.Children.Add(el);
                    Nodes.Add(node);
                }
            }
        }

        public void DrawSwitches()
        {
            foreach (var sw in NetworkModel.Switches)
            {
                Ellipse el = new Ellipse();
                el.Width = _ellipseWidth;
                el.Height = _ellipseHeight;
                el.StrokeThickness = 0;
                el.Fill = Brushes.Black;
                el.ToolTip = $"Switch Id:{sw.Id}, Switch Name:{sw.Name}\nXCoord:{sw.X}, YCoord:{sw.Y}";
                ConvertLatLongToLocalCoordinates(sw.X, sw.Y, out double x, out double y);

                double roundedX = Math.Round(x);
                double roundedY = Math.Round(y);

                var point = GridPoints[Tuple.Create<int, int>((int)roundedX, (int)roundedY)];

                if (point.Taken)
                {
                    int pixelDistance = 1;
                    GridPoint newPoint;
                    while ((newPoint = CheckIfCanPlace(point, pixelDistance)) == null)
                    {
                        pixelDistance++;
                    }
                    sw.X = newPoint.X;
                    sw.Y = newPoint.Y;
                    newPoint.Taken = true;
                    Canvas.SetLeft(el, newPoint.X);
                    Canvas.SetBottom(el, newPoint.Y);
                    GridCanvas.Children.Add(el);
                    Switches.Add(sw);
                }
                else
                {
                    sw.X = point.X;
                    sw.Y = point.Y;
                    point.Taken = true;
                    Canvas.SetLeft(el, point.X);
                    Canvas.SetBottom(el, point.Y);
                    GridCanvas.Children.Add(el);
                    Switches.Add(sw);
                }
            }
        }

        public void DrawLines()
        {
            // BFS variation, instead of using graphs, we use lists
            GridPoint startNode = new GridPoint();
            GridPoint endNode = new GridPoint();

            // Go through each line and find the start and end node
            // and then draw the line
            foreach (LineEntity line in NetworkModel.Lines)
            {
                foreach (SubstationEntity sub in Substations)
                {
                    double roundedX = Math.Round(sub.X);
                    double roundedY = Math.Round(sub.Y);
                    if (sub.Id == line.FirstEnd)
                    {
                        startNode.X = (int)roundedX;
                        startNode.Y = (int)roundedY;
                    }
                    if (sub.Id == line.SecondEnd)
                    {
                        endNode.X = (int)roundedX;
                        endNode.Y = (int)roundedY;
                    }
                }

                foreach (NodeEntity node in Nodes)
                {
                    double roundedX = Math.Round(node.X);
                    double roundedY = Math.Round(node.Y);
                    if (node.Id == line.FirstEnd)
                    {
                        startNode.X = (int)roundedX;
                        startNode.Y = (int)roundedY;
                    }
                    if (node.Id == line.SecondEnd)
                    {
                        endNode.X = (int)roundedX;
                        endNode.Y = (int)roundedY;
                    }
                }

                foreach (SwitchEntity sw in Switches)
                {
                    double roundedX = Math.Round(sw.X);
                    double roundedY = Math.Round(sw.Y);
                    if (sw.Id == line.FirstEnd)
                    {
                        startNode.X = (int)roundedX;
                        startNode.Y = (int)roundedY;
                    }
                    if (sw.Id == line.SecondEnd)
                    {
                        endNode.X = (int)roundedX;
                        endNode.Y = (int)roundedY;
                    }
                }

                // When both nodes are found, draw a line in right angle
                if (startNode != null && endNode != null)
                {
                    Line l1 = new Line();
                    l1.Stroke = new SolidColorBrush(Colors.HotPink);
                    l1.StrokeThickness = 0.5;
                    l1.X1 = startNode.X;
                    l1.Y1 = GridCanvas.Height - startNode.Y;
                    l1.X2 = endNode.X;
                    l1.Y2 = GridCanvas.Height - startNode.Y;
                    GridCanvas.Children.Add(l1);

                    Line l2 = new Line();
                    l2.Stroke = new SolidColorBrush(Colors.HotPink);
                    l2.StrokeThickness = 0.5;
                    l2.X1 = endNode.X;
                    l2.Y1 = GridCanvas.Height - startNode.Y;
                    l2.X2 = endNode.X;
                    l2.Y2 = GridCanvas.Height - endNode.Y; 
                    GridCanvas.Children.Add(l2);
                }
            }
        }

        public void LoadGrid_Handler(object source, EventArgs e)
        {
            if (IsDataLoaded)
            {
                if (GridCanvas.Children.Count > 0)
                {
                    GridCanvas.Children.Clear();

                }
                DrawSubstations();
                DrawNodes();
                DrawSwitches();
                DrawLines();
                
            }
        }

        public GridPoint CheckIfCanPlace(GridPoint point, int pixelDistance)
        {
            GridPoint newPoint = null;
            if ((newPoint = CheckIfCanPlaceRight(point, pixelDistance)) != null)
            {
                return newPoint;
            }
            else if ((newPoint = CheckIfCanPlaceLeft(point, pixelDistance)) != null)
            {
                return newPoint;
            }
            else if ((newPoint = CheckIfCanPlaceUp(point, pixelDistance)) != null)
            {
                return newPoint;
            }
            else if ((newPoint = CheckIfCanPlaceDown(point, pixelDistance)) != null)
            {
                return newPoint;
            }
            else if ((newPoint = CheckIfCanPlaceUpperLeft(point, pixelDistance)) != null)
            {
                return newPoint;
            }
            else if ((newPoint = CheckIfCanPlaceUpperRight(point, pixelDistance)) != null)
            {
                return newPoint;
            }
            else if ((newPoint = CheckIfCanPlaceBottomLeft(point, pixelDistance)) != null)
            {
                return newPoint;
            }
            else if ((newPoint = CheckIfCanPlaceBottomRight(point, pixelDistance)) != null)
            {
                return newPoint;
            }
            return newPoint;
        }

        #region CheckPossibleMovement
        public GridPoint CheckIfCanPlaceRight(GridPoint point, int pixelDistance)
        {
            if(point.X + pixelDistance <= GridCanvas.Width)
            {
                GridPoint rightPoint = null;
                if (GridPoints.TryGetValue(Tuple.Create<int, int>(point.X + pixelDistance, point.Y), out rightPoint))
                {
                    if (!rightPoint.Taken)
                    {
                        return rightPoint;
                    }
                }
            }
            return null;
        }

        public GridPoint CheckIfCanPlaceLeft(GridPoint point, int pixelDistance)
        {
            if(point.X - pixelDistance >= 0)
            {
                GridPoint leftPoint = null;
                if(GridPoints.TryGetValue(Tuple.Create<int, int>(point.X - pixelDistance, point.Y), out leftPoint))
                {
                    if (!leftPoint.Taken)
                    {
                        return leftPoint;
                    }
                }
            }
            return null;
        }

        public GridPoint CheckIfCanPlaceUp(GridPoint point, int pixelDistance)
        {
            if (point.Y - pixelDistance >= 0)
            {
                GridPoint upPoint = null;
                if (GridPoints.TryGetValue(Tuple.Create<int, int>(point.X, point.Y - pixelDistance), out upPoint))
                {
                    if (!upPoint.Taken)
                    {
                        return upPoint;
                    }
                }
            }
            return null;
        }

        public GridPoint CheckIfCanPlaceDown(GridPoint point, int pixelDistance)
        {
            if (point.Y + pixelDistance <= GridCanvas.Height)
            {
                GridPoint downPoint = null;
                if (GridPoints.TryGetValue(Tuple.Create<int, int>(point.X, point.Y + pixelDistance), out downPoint))
                {
                    if (!downPoint.Taken)
                    {
                        return downPoint;
                    }
                }
            }
            return null;
        }
        #endregion

        #region CheckPossibleMovementDiagonals

        public GridPoint CheckIfCanPlaceUpperLeft(GridPoint point, int pixelDistance)
        {
            if(point.X - pixelDistance >= 0 && point.Y - pixelDistance >= 0)
            {
                GridPoint upperLeftPoint = null;
                if(GridPoints.TryGetValue(Tuple.Create<int, int>(point.X - pixelDistance, point.Y - pixelDistance), out upperLeftPoint))
                {
                    if (!upperLeftPoint.Taken)
                    {
                        return upperLeftPoint;
                    }
                }
            }
            return null;
        }

        public GridPoint CheckIfCanPlaceUpperRight(GridPoint point, int pixelDistance)
        {
            if(point.X + pixelDistance <= GridCanvas.Width && point.Y - pixelDistance >= 0)
            {
                GridPoint upperRightPoint = null;
                if(GridPoints.TryGetValue(Tuple.Create<int, int>(point.X + pixelDistance, point.Y - pixelDistance), out upperRightPoint))
                {
                    if (!upperRightPoint.Taken)
                    {
                        return upperRightPoint;
                    }
                }
            }
            return null;
        }

        public GridPoint CheckIfCanPlaceBottomLeft(GridPoint point, int pixelDistance)
        {
            if(point.X - pixelDistance >= 0 && point.Y + pixelDistance <= GridCanvas.Height)
            {
                GridPoint bottomLeftPoint = null;
                if(GridPoints.TryGetValue(Tuple.Create<int, int>(point.X - pixelDistance, point.Y + pixelDistance), out bottomLeftPoint))
                {
                    if (!bottomLeftPoint.Taken)
                    {
                        return bottomLeftPoint;
                    }
                }
            }
            return null;
        }

        public GridPoint CheckIfCanPlaceBottomRight(GridPoint point, int pixelDistance)
        {
            if (point.X + pixelDistance >= 0 && point.Y + pixelDistance <= GridCanvas.Height)
            {
                GridPoint bottomRightPoint = null;
                if (GridPoints.TryGetValue(Tuple.Create<int, int>(point.X + pixelDistance, point.Y + pixelDistance), out bottomRightPoint))
                {
                    if (!bottomRightPoint.Taken)
                    {
                        return bottomRightPoint;
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
