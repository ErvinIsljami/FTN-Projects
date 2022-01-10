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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PZ4.Enums;
using PZ4.Models;
using PZ4.Xml;

namespace PZ4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NetworkModel NetworkModel { get; set; }
        public CustomXmlRW CustomXmlRW { get; set; }
        private bool _isDataLoaded = false;
        // Assignment mins and maxs
        private double _mapLeft = 19.793909;
        private double _mapBottom = 45.2325;
        private double _mapTop = 45.277031;
        private double _mapRight = 19.894459;
        private double _intervalXLong = 0.0;
        private double _intervalYLat = 0.0;
        // Map size
        private double _mapEdgeSize = 2;
        // Map square bottom left
        private double _mapSquareLeftCoord = -1;
        private double _mapSquareBottomCoord = -1;
        // Zoom and pan
        private System.Windows.Point _start = new System.Windows.Point();
        private System.Windows.Point _diffOffset = new System.Windows.Point();
        private int _zoomMax = 40;
        private int _zoomCurrent = 1;
        // Used for rotations
        private System.Windows.Point _startPosition = new System.Windows.Point(0,0);
        // Used for hit testing
        private System.Windows.Point _currentMouseHit = new System.Windows.Point();
        private GeometryModel3D _hitGeoModel;
        // Used for hit testing, holds the tooltip
        private Dictionary<GeometryModel3D, string> _models;
        // Used for drawing lines (draw lines only for drawn entitites)
        private List<SubstationEntity> _drawnSubstations;
        private List<NodeEntity> _drawnNodes;
        private List<SwitchEntity> _drawnSwitches;

        public MainWindow()
        {
            CustomXmlRW = new CustomXmlRW();
            _models = new Dictionary<GeometryModel3D, string>();
            _drawnSubstations = new List<SubstationEntity>();
            _drawnNodes = new List<NodeEntity>();
            _drawnSwitches = new List<SwitchEntity>();
            ReadXmlAndConvertAndFindMinimalsAndMaximalsTemplate();
            InitializeComponent();
        }

        private void LoadMap_Click(object sender, RoutedEventArgs e)
        {
            // If models are already loaded, clear all models, but save the map front and map back and reset
            if(AllModelsGroup.Children.Count > 2)
            {
                var mapFront = MapFront;
                var mapBack = MapBack;
                AllModelsGroup.Children.Clear();
                AllModelsGroup.Children.Add(mapFront);
                AllModelsGroup.Children.Add(mapBack);
            }

            if (_isDataLoaded)
            {
                DrawSubstations();
                DrawNodes();
                DrawSwitches();
                DrawLines();
            }
        }

        private void CenterMapAndResetRotations_Click(object sender, RoutedEventArgs e)
        {
            XRotationSlider.Value = -50;
            YRotationSlider.Value = 0;
            ZRotationSlider.Value = 0;
            xAxisAngleRotation.Angle = 0;
            yAxisAngleRotation.Angle = 0;
            translation.OffsetX = 0;
            translation.OffsetY = 0;
            translation.OffsetZ = 0;
            _zoomCurrent = 1;
            Camera.FieldOfView = 45;
        }

        #region DataSetupTemplate
        public async Task ReadXmlAndConvertAndFindMinimalsAndMaximalsTemplate()
        {
            Task readXmlTask = Task.Run(() =>
            {
                ReadXml();
            }).ContinueWith(antecedent =>
            {
                ConvertUTMToDecimal();
                _isDataLoaded = true;
            }).ContinueWith(antecedent =>
            {
                CalculateLatLonIntervals();
            });
        }

        public void ReadXml()
        {
            NetworkModel = CustomXmlRW.DeSerializeObject<NetworkModel>(@"../../GridData/Geographic.xml");
        }

        public void ConvertUTMToDecimal()
        {
            foreach (var substation in NetworkModel.Substations)
            {
                UTMToLatLonConverter.ToLatLon(substation.X, substation.Y, 34, out double latitude, out double longitude);
                substation.X = longitude;
                substation.Y = latitude;
            }

            foreach (var node in NetworkModel.Nodes)
            {
                UTMToLatLonConverter.ToLatLon(node.X, node.Y, 34, out double latitude, out double longitude);
                node.X = longitude;
                node.Y = latitude;
            }

            foreach (var sw in NetworkModel.Switches)
            {
                UTMToLatLonConverter.ToLatLon(sw.X, sw.Y, 34, out double latitude, out double longitude);
                sw.X = longitude;
                sw.Y = latitude;
            }

            foreach (var line in NetworkModel.Lines)
            {
                List<Models.Point> points = new List<Models.Point>();
                foreach (var point in line.Vertices)
                {
                    UTMToLatLonConverter.ToLatLon(point.X, point.Y, 34, out double latitude, out double longitude);
                    point.X = longitude;
                    point.Y = latitude;
                }
            }
        }

        public void CalculateLatLonIntervals()
        {
            _intervalXLong = _mapRight - _mapLeft;
            _intervalYLat = _mapTop - _mapBottom;
        }
        #endregion

        #region MouseEvents
        private void Viewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToolTipTextBlock.Text = "";

            Viewport.CaptureMouse();
            _start = e.GetPosition(this);
            _diffOffset.X = translation.OffsetX;
            _diffOffset.Y = translation.OffsetY;

            System.Windows.Point mousePosition = e.GetPosition(Viewport);
            _currentMouseHit = mousePosition;
            Point3D testpoint3D = new Point3D(mousePosition.X, mousePosition.Y, 0);
            Vector3D testdirection = new Vector3D(mousePosition.X, mousePosition.Y, 10);

            PointHitTestParameters pointparams = new PointHitTestParameters(_currentMouseHit);
            RayHitTestParameters rayparams = new RayHitTestParameters(testpoint3D, testdirection);

            //test for a result in the Viewport3D     
            _hitGeoModel = null;
            VisualTreeHelper.HitTest(Viewport, null, HTResult, pointparams);
        }

        private void Viewport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Viewport.ReleaseMouseCapture();
        }

        private void Viewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                _startPosition = e.GetPosition(this);
            }
        }

        private void Viewport_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point currentPosition = e.GetPosition(this);
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                // Offset that tells us the direction of movement by X and Y axes
                double offsetX = currentPosition.X - _startPosition.X;
                double offsetY = currentPosition.Y - _startPosition.Y;
                double step = 0.2;

                xAxisAngleRotation.Angle += step * offsetY;
                yAxisAngleRotation.Angle += step * offsetX;
            }

            _startPosition = currentPosition;

            if (Viewport.IsMouseCaptured)
            {
                System.Windows.Point end = e.GetPosition(this);
                double offsetX = end.X - _start.X;
                double offsetY = end.Y - _start.Y;
                double w = this.Width;
                double h = this.Height;
                double translateX = (offsetX * 100) / w;
                double translateY = -(offsetY * 100) / h;
                translation.OffsetX = _diffOffset.X + (translateX / (100 * scaling.ScaleX));
                translation.OffsetY = _diffOffset.Y + (translateY / (100 * scaling.ScaleX));
            }
        }

        private void Viewport_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && _zoomCurrent < _zoomMax)
            {
                _zoomCurrent++;
                Camera.FieldOfView--;
            }
            else if (e.Delta <= 0 && _zoomCurrent > -_zoomMax)
            {
                _zoomCurrent--;
                Camera.FieldOfView++;
            }
        }
        #endregion

        #region DrawEntities
        private void DrawSubstations()
        {
            double offsetXLong = 0.0;
            double offsetYLat = 0.0;

            foreach(var sub in NetworkModel.Substations)
            {
                if((sub.Y > _mapBottom && sub.Y < _mapTop) && (sub.X > _mapLeft && sub.X < _mapRight))
                {
                    offsetXLong = _mapSquareLeftCoord + (((sub.X - _mapLeft) / _intervalXLong) * _mapEdgeSize);
                    offsetYLat = _mapSquareBottomCoord + (((sub.Y - _mapBottom) / _intervalYLat) * _mapEdgeSize);

                    var cube = Helpers.GenerateCubeGeometryModel(EntityType.Substation, Colors.Green);
                    cube.Transform = new TranslateTransform3D(offsetXLong, offsetYLat, 0);
                    _models.Add(cube, $"Substation:\nID: {sub.Id}\nNAME: {sub.Name}");
                    AllModelsGroup.Children.Add(cube);
                    _drawnSubstations.Add(sub);
                }
            }
        }

        private void DrawNodes()
        {
            double offsetXLong = 0.0;
            double offsetYLat = 0.0;

            foreach (var node in NetworkModel.Nodes)
            {
                if ((node.Y > _mapBottom && node.Y < _mapTop) && (node.X > _mapLeft && node.X < _mapRight))
                {
                    offsetXLong = _mapSquareLeftCoord + (((node.X - _mapLeft) / _intervalXLong) * _mapEdgeSize);
                    offsetYLat = _mapSquareBottomCoord + (((node.Y - _mapBottom) / _intervalYLat) * _mapEdgeSize);

                    var cube = Helpers.GenerateCubeGeometryModel(EntityType.Node, Colors.Red);
                    cube.Transform = new TranslateTransform3D(offsetXLong, offsetYLat, 0);
                    _models.Add(cube, $"Node:\nID: {node.Id}\nNAME: {node.Name}");
                    AllModelsGroup.Children.Add(cube);
                    _drawnNodes.Add(node);
                }
            }
        }

        private void DrawSwitches()
        {
            double offsetXLong = 0.0;
            double offsetYLat = 0.0;

            foreach (var sw in NetworkModel.Switches)
            {
                if ((sw.Y > _mapBottom && sw.Y < _mapTop) && (sw.X > _mapLeft && sw.X < _mapRight))
                {
                    offsetXLong = _mapSquareLeftCoord + (((sw.X - _mapLeft) / _intervalXLong) * _mapEdgeSize);
                    offsetYLat = _mapSquareBottomCoord + (((sw.Y - _mapBottom) / _intervalYLat) * _mapEdgeSize);

                    var cube = Helpers.GenerateCubeGeometryModel(EntityType.Switch, Colors.Black);
                    cube.Transform = new TranslateTransform3D(offsetXLong, offsetYLat, 0);
                    _models.Add(cube, $"Switch:\nID: {sw.Id}\nNAME: {sw.Name}");
                    AllModelsGroup.Children.Add(cube);
                    _drawnSwitches.Add(sw);
                }
            }
        }

        private void DrawLines()
        {
            double firstEndX1 = 0;
            double firstEndY1 = 0;
            double secondEndX2 = 0;
            double secondEndY2 = 0;

            bool isFirstEndFound = false;
            bool isSecondEndFound = false;

            int[] triangleIndices = new int[] { 0, 1, 2, 0, 2, 3 };

            // Go through each line and check if there are both nodes in drawn entities
            foreach (LineEntity line in NetworkModel.Lines)
            {
                foreach (SubstationEntity sub in NetworkModel.Substations)
                {
                    if (sub.Id == line.FirstEnd)
                    {
                        isFirstEndFound = true;
                    }
                    if (sub.Id == line.SecondEnd)
                    {
                        isSecondEndFound = true;
                    }
                }

                foreach (NodeEntity node in NetworkModel.Nodes)
                {
                    if (node.Id == line.FirstEnd)
                    {
                        isFirstEndFound = true;
                    }
                    if (node.Id == line.SecondEnd)
                    {
                        isSecondEndFound = true;
                    }
                }

                foreach (SwitchEntity sw in NetworkModel.Switches)
                {
                    if (sw.Id == line.FirstEnd)
                    {
                        isFirstEndFound = true;
                    }
                    if (sw.Id == line.SecondEnd)
                    {
                        isSecondEndFound = true;
                    }
                }

                // When both nodes are found, draw a line
                if (isFirstEndFound && isSecondEndFound)
                {
                    for(int i = 0; i < line.Vertices.Count - 1; i++)
                    {
                        double firstVertexLong = line.Vertices[i].X;
                        double firstVertexLat = line.Vertices[i].Y;

                        if ((firstVertexLat > _mapBottom && firstVertexLat < _mapTop) && (firstVertexLong > _mapLeft && firstVertexLong < _mapRight))
                        {
                            firstEndX1 = _mapSquareLeftCoord + (((firstVertexLong - _mapLeft) / _intervalXLong) * _mapEdgeSize);
                            firstEndY1 = _mapSquareBottomCoord + (((firstVertexLat - _mapBottom) / _intervalYLat) * _mapEdgeSize);
                        }

                        double secondVertexLong = line.Vertices[i + 1].X;
                        double secondVertexLat = line.Vertices[i + 1].Y;

                        if ((secondVertexLat > _mapBottom && secondVertexLat < _mapTop) && (secondVertexLong > _mapLeft && secondVertexLong < _mapRight))
                        {
                            secondEndX2 = _mapSquareLeftCoord + (((secondVertexLong - _mapLeft) / _intervalXLong) * _mapEdgeSize);
                            secondEndY2 = _mapSquareBottomCoord + (((secondVertexLat - _mapBottom) / _intervalYLat) * _mapEdgeSize);
                        }

                        // Primitive solution
                        //Point3D firstPoint = new Point3D(firstEndX1, firstEndY1, 0.0005);
                        //Point3D secondPoint = new Point3D(secondEndX2, secondEndY2, 0.0005);
                        //Point3D thirdPoint = new Point3D(firstEndX1 + 0.003, firstEndY1, 0.0005);
                        //Point3D fourthPoint = new Point3D(secondEndX2 + 0.003, secondEndY2, 0.0005);
                        //Point3D[] positions = new Point3D[] { thirdPoint, firstPoint, secondPoint, fourthPoint };

                        // Found algorithm on google for finding all 4 points of a rectangle
                        Point3D a1 = new Point3D(firstEndX1, firstEndY1, 0.0005);
                        Point3D b1 = new Point3D(secondEndX2, secondEndY2, 0.0005);

                        Vector3D diffVector = b1 - a1;
                        Vector3D nVector = Vector3D.CrossProduct(diffVector, new Vector3D(0, 0, 1));
                        nVector = Vector3D.Divide(nVector, nVector.Length);
                        nVector = Vector3D.Multiply(nVector, 0.001);

                        Point3D firstPoint = a1 + nVector;
                        Point3D secondPoint = a1 - nVector;
                        Point3D thirdPoint = b1 + nVector;
                        Point3D fourthPoint = b1 - nVector;

                        Point3D[] positions = new Point3D[] { secondPoint, firstPoint, thirdPoint, fourthPoint };

                        GeometryModel3D lineGeometry = new GeometryModel3D();
                        MeshGeometry3D meshGeometry = new MeshGeometry3D();
                        DiffuseMaterial material = new DiffuseMaterial();
                        switch (line.ConductorMaterial)
                        {
                            case PowerLineConductingMaterial.Steel:
                                material.Brush = Brushes.DarkGray;
                                break;

                            case PowerLineConductingMaterial.Acsr:
                                material.Brush = Brushes.Green;
                                break;

                            case PowerLineConductingMaterial.Copper:
                                material.Brush = Brushes.Orange;
                                break;

                            case PowerLineConductingMaterial.Other:
                                material.Brush = Brushes.Purple;
                                break;
                            default:
                                material.Brush = Brushes.Black;
                                break;
                        }
                        lineGeometry.Material = material;
                        meshGeometry.Positions = new Point3DCollection(positions);
                        meshGeometry.TriangleIndices = new Int32Collection(triangleIndices);
                        lineGeometry.Geometry = meshGeometry;
                        AllModelsGroup.Children.Add(lineGeometry);
                    }
                }
            }
        }
        #endregion

        #region HitTest
        private HitTestResultBehavior HTResult(HitTestResult result)
        {
            RayHitTestResult rayResult = result as RayHitTestResult;

            if (rayResult != null)
            {
                bool gasit = false;

                foreach (KeyValuePair<GeometryModel3D, string> pair in _models)
                {
                    if (pair.Key == rayResult.ModelHit)
                    {
                        _hitGeoModel = (GeometryModel3D)rayResult.ModelHit;
                        gasit = true;
                        ToolTipTextBlock.Text = pair.Value;
                    }
                }
                if (!gasit)
                {
                    _hitGeoModel = null;
                }
            }

            return HitTestResultBehavior.Stop;
        }
        #endregion
    }
}
