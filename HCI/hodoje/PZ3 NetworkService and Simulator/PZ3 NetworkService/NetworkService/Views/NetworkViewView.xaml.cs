using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NetworkService.Models;
using NetworkService.ViewModel;

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for NetworkViewView.xaml
    /// </summary>
    public partial class NetworkViewView : UserControl
    {
        private DataIO serializer = new DataIO();

        private bool dragging;
        private Road currentDraggedItem;

        // Double-check the elements on grid, OnGridCollection.Contains doesn't work properly
        public static List<int> OnGridIds = new List<int>();

        public static Dictionary<string, Road> OnGridCollection = new Dictionary<string, Road>();

        public List<Canvas> ListOfCanvases = new List<Canvas>();

        // List of disabled grids from listview
        public static Dictionary<string, Grid> ListOfDisabledGrids = new Dictionary<string, Grid>();

        public Road CurrentRoad
        {
            get { return NotifiedVms.Instance.CurrentRoad; }
        }

        public NetworkViewViewModel vm { get; set; }

        public NetworkViewView()
        {
            InitializeComponent();

            // Add all canvases to the list so it can be iterated later
            ListOfCanvases.Add(Can1);
            ListOfCanvases.Add(Can2);
            ListOfCanvases.Add(Can3);
            ListOfCanvases.Add(Can4);
            ListOfCanvases.Add(Can5);
            ListOfCanvases.Add(Can6);
            ListOfCanvases.Add(Can7);
            ListOfCanvases.Add(Can8);
            ListOfCanvases.Add(Can9);
            ListOfCanvases.Add(Can10);
            ListOfCanvases.Add(Can11);
            ListOfCanvases.Add(Can12);
            ListOfCanvases.Add(Can13);
            ListOfCanvases.Add(Can14);
            ListOfCanvases.Add(Can15);
            ListOfCanvases.Add(Can16);

            // Set the grid with last known positions of objects on the grid

            this.DataContext = new NetworkService.ViewModel.NetworkViewViewModel();
            vm = (NetworkViewViewModel) (this.DataContext);
            SetGrid();
            //SetListView();
            CheckColor();
        }

        private void CheckColor()                                       // Check new value and update specific UI element every 100ms
        {
            Task.Delay(100).ContinueWith(_ =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    if (NotifiedVms.Instance.CurrentRoad != null)
                    {
                        if (OnGridCollection.ContainsValue(NotifiedVms.Instance.CurrentRoad))
                        {
                            foreach (var gridEl in OnGridCollection)
                            {
                                if (gridEl.Value.Id == NotifiedVms.Instance.CurrentRoad.Id)
                                {
                                    foreach (var can in ListOfCanvases)
                                    {
                                        if (can.Name == gridEl.Key)
                                        {
                                            if (gridEl.Value.Type.NAME == "IA")
                                            {
                                                if (gridEl.Value.Value > 15000)
                                                {
                                                    ((Canvas)can.Children[2]).Background = Brushes.Red;
                                                    break;
                                                }
                                                else
                                                {
                                                    ((Canvas)can.Children[2]).Background = Brushes.Transparent;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                if (gridEl.Value.Value > 7000)
                                                {
                                                    ((Canvas)can.Children[2]).Background = Brushes.Red;
                                                    break;
                                                }
                                                else
                                                {
                                                    ((Canvas)can.Children[2]).Background = Brushes.Transparent;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    CheckColor();
                });
            });
        }

        private void ListViewName_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentDraggedItem = null;
            ListViewName.SelectedItem = null;
            dragging = false;
        }

        private void ListViewName_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dragging)
            {
                dragging = true;
                currentDraggedItem = (Road)ListViewName.SelectedItem;
                DragDrop.DoDragDrop(this, currentDraggedItem, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            // Check if the currently dragged item is already on the grid
            if (!OnGridCollection.ContainsValue(currentDraggedItem) && !OnGridIds.Contains(currentDraggedItem.Id))
            {
                base.OnDrop(e);
                if (currentDraggedItem != null)
                {
                    //if (((Canvas)sender).Resources["taken"] == null)
                    if(((Canvas)sender).Background.Equals(Brushes.White))
                    {

                        // Create image from currently dragged item
                        BitmapImage logo = new BitmapImage();
                        logo.BeginInit();
                        logo.UriSource = new Uri(currentDraggedItem.Type.IMG_URL, UriKind.RelativeOrAbsolute);
                        logo.EndInit();

                        // Set the target canvas background
                        ((Canvas) sender).Background = new ImageBrush(logo);
                        if (currentDraggedItem.Type.NAME == "IA")
                        {
                            if (currentDraggedItem.Value > 15000)
                            {
                                ((Canvas)(((Canvas)sender).Children[2])).Background = Brushes.Red;
                            }
                            else
                            {
                                ((Canvas)(((Canvas)sender).Children[2])).Background = Brushes.Transparent;
                            }
                        }
                        else
                        {
                            if (currentDraggedItem.Value > 7000)
                            {
                                ((Canvas)(((Canvas)sender).Children[2])).Background = Brushes.Red;
                            }
                            else
                            {
                                ((Canvas)(((Canvas)sender).Children[2])).Background = Brushes.Transparent;
                            }
                        }

                        // Set partial data for the correct TextBlock under the Canvas
                        ((TextBox)((Canvas)sender).Children[1]).Text = currentDraggedItem.ToString();

                        ((Canvas)sender).Resources.Add("taken", true);

                        OnGridIds.Add(currentDraggedItem.Id);
                        OnGridCollection.Add(((Canvas)sender).Name,currentDraggedItem);
                    }

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // This code (with help of "GetFrameworkElementByName") gets the current selected item in the listview,
                    // extracts the data template of the particular listview item and then gets the control that is the main element of the data template
                    ListViewItem item = ListViewName.ItemContainerGenerator.ContainerFromIndex(ListViewName.SelectedIndex) as ListViewItem;

                    Grid g = null;

                    if (item != null)
                    {
                        ContentPresenter templateParent = GetFrameworkElementByName<ContentPresenter>(item);

                        DataTemplate dataTemplate = ListViewName.ItemTemplate;

                        if (dataTemplate != null && templateParent != null)
                        {
                            g = dataTemplate.FindName("ListViewItemGrid", templateParent) as Grid;
                        }

                        g.Cursor = Cursors.No;
                        ((Canvas)g.Children[1]).Background = Brushes.LightGray;
                        if (!ListOfDisabledGrids.ContainsValue(g))
                        {
                            ListOfDisabledGrids.Remove(((Canvas) sender).Name);     // Since we care only about the grid, we need to delete the grid item with current key
                            ListOfDisabledGrids.Add(((Canvas)sender).Name, g);      // And add it back again with a different key
                        }
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    ListViewName.SelectedItem = null;
                    dragging = false;
                }
                e.Handled = true;
            }
        }

        private void UIElement_OnDragOver(object sender, DragEventArgs e)
        {
            base.OnDragOver(e);
            if (((Canvas) sender).Resources["taken"] != null)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Copy;
            }
            e.Handled = true;
        }

        // Since we can't save the latest state of the grid because the view always recreates whenever we navigate to it,
        // we manually refresh it's state
        private void SetGrid()
        {
            Dictionary<string, Road> Temp = new Dictionary<string, Road>();

            foreach (var el in OnGridCollection)
            {
                foreach (var r in RoadsObs.Instance.Roads)
                {
                    if (el.Value.Id == r.Id)
                    {
                        Temp.Add(el.Key, el.Value);
                        break;
                    }
                }
            }
            OnGridCollection = Temp;


            foreach (var el in OnGridCollection)
            {
                foreach (Canvas can in ListOfCanvases)
                {
                    if (el.Key == can.Name)
                    {
                        BitmapImage logo = new BitmapImage();
                        logo.BeginInit();
                        logo.UriSource = new Uri(el.Value.Type.IMG_URL, UriKind.RelativeOrAbsolute);
                        logo.EndInit();

                        // Set the target canvas background
                        can.Background = new ImageBrush(logo);

                        if (el.Value.Type.NAME == "IA")
                        {
                            if (el.Value.Value > 15000)
                            {
                                ((Canvas)(can.Children[2])).Background = Brushes.Red;
                            }
                            else
                            {
                                ((Canvas)(can.Children[2])).Background = Brushes.Transparent;
                            }
                        }
                        else
                        {
                            if (el.Value.Value > 7000)
                            {
                                ((Canvas)(can.Children[2])).Background = Brushes.Red;
                            }
                            else
                            {
                                ((Canvas)(can.Children[2])).Background = Brushes.Transparent;
                            }
                        }

                            ((TextBox)can.Children[1]).Text = el.Value.ToString();

                        if (!OnGridIds.Contains(el.Value.Id))
                        {
                            OnGridIds.Add(el.Value.Id);
                        }
                    }
                }
            }
        }

        // Not sure why this code doesn't work
        private void SetListView()
        {
            //foreach (KeyValuePair<string, Grid> el in ListOfDisabledGrids)
            //{
            //    el.Value.Cursor = Cursors.No;
            //    ((Canvas) el.Value.Children[1]).Background = Brushes.LightGray;
            //}

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Used for getting data template elements
        private static T GetFrameworkElementByName<T>(FrameworkElement referenceElement) where T : FrameworkElement
        {
            FrameworkElement child = null;

            for (Int32 i = 0; i < VisualTreeHelper.GetChildrenCount(referenceElement); i++)
            {
                child = VisualTreeHelper.GetChild(referenceElement, i) as FrameworkElement;

                System.Diagnostics.Debug.WriteLine(child);

                if (child != null && child.GetType() == typeof(T))
                {
                    break;
                }
                else if (child != null)
                {
                    child = GetFrameworkElementByName<T>(child);
                    if (child != null && child.GetType() == typeof(T))
                    {
                        break;
                    }
                }
            }
            return child as T;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void RemoveGridItemOnClick(object sender, RoutedEventArgs e)
        {
            Canvas clearCanvas = (Canvas)((Button) sender).Parent;

            //if (clearCanvas.Resources["taken"] != null)
            if(!clearCanvas.Background.Equals(Brushes.White))
            {
                foreach (Canvas can in ListOfCanvases)
                {
                    if(clearCanvas.Equals(can))
                    {
                        clearCanvas.Background = Brushes.White;
                        ((TextBox) (clearCanvas.Children[1])).Text = "";
                        clearCanvas.Resources.Remove("taken");

                        ((Canvas) (clearCanvas.Children[2])).Background = Brushes.Transparent;

                        OnGridIds.Remove(OnGridCollection[clearCanvas.Name].Id);
                        OnGridCollection.Remove(clearCanvas.Name);

                        ((Canvas)(ListOfDisabledGrids[clearCanvas.Name].Children[1])).Background = Brushes.Transparent;
                        ((Canvas)(ListOfDisabledGrids[clearCanvas.Name].Children[1])).Cursor = Cursors.Arrow;
                        ListOfDisabledGrids.Remove(clearCanvas.Name);
                        break;
                    }
                }   
            }
        }
    }
}
