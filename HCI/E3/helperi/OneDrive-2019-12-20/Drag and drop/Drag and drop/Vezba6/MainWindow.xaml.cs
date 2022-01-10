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

namespace Vezba6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Slika draggedItem = null;
        private bool dragging = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dragging)
            {
                dragging = true;
                ListViewItem o = (ListViewItem)listView1.SelectedItem;
                Slika draggedData = new Slika(((ImageBrush)o.Background).ImageSource.ToString());
                DragDrop.DoDragDrop(this, draggedData, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void listView1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //draggedItem = null;
            listView1.SelectedItem = null;
            dragging = false;
        }

        private void Canvas1_DragEnter(object sender, DragEventArgs e)
        {
            //This one is called before DragOver
        }

        private void dragOver(object sender, DragEventArgs e)
        {
            //base.OnDragOver(e);
            if (((Canvas)sender).Resources["taken"] != null)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Copy;
            }
            //e.Handled = true; //marking the event handled
        }

        private void drop(object sender, DragEventArgs e)
        {
            //base.OnDrop(e);
            Slika dataString = null;

            if (e.Data.GetDataPresent(typeof(Slika))){
                dataString  = (Slika)e.Data.GetData(typeof(Slika));
            }

            if (dataString != null)
            {
                if (((Canvas)sender).Resources["taken"] == null)
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(dataString.imageUri);
                    logo.EndInit();
                    ((Canvas)sender).Background = new ImageBrush(logo);
                    ((TextBlock)((Canvas)sender).Children[0]).Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDAFF00"));
                    ((TextBlock)((Canvas)sender).Children[0]).Text = "Mesto zauzeto";
                    ((Canvas)sender).Resources.Add("taken", true);
                }
                listView1.SelectedItem = null;
                dragging = false;
            }

            //e.Handled = true;
        }

        private void oslobodi1(object sender, RoutedEventArgs e)
        {
            if (canvas1.Resources["taken"] != null)
            {
                canvas1.Background = Brushes.GhostWhite;
                ((TextBlock)canvas1.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas1.Children[0]).Text = "Slobodno mesto";
                canvas1.Resources.Remove("taken");
            }
        }

        private void oslobodi2(object sender, RoutedEventArgs e)
        {
            if (canvas2.Resources["taken"] != null)
            {
                canvas2.Background = Brushes.GhostWhite;
                ((TextBlock)canvas2.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas2.Children[0]).Text = "Slobodno mesto";
                canvas2.Resources.Remove("taken");
            }
        }

        private void oslobodi3(object sender, RoutedEventArgs e)
        {
            if (canvas3.Resources["taken"] != null)
            {
                canvas3.Background = Brushes.GhostWhite;
                ((TextBlock)canvas3.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas3.Children[0]).Text = "Slobodno mesto";
                canvas3.Resources.Remove("taken");
            }
        }

        private void oslobodi4(object sender, RoutedEventArgs e)
        {
            if (canvas4.Resources["taken"] != null)
            {
                canvas4.Background = Brushes.GhostWhite;
                ((TextBlock)canvas4.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas4.Children[0]).Text = "Slobodno mesto";
                canvas4.Resources.Remove("taken");
            }
        }

        
    }
}
