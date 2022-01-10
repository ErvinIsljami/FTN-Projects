using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PZ3_NetworkService.ViewModel
{
    public class MeriloViewModel : BindableBase
    {


        private string networkTerminal;
        private string networkTerminal2;


        bool dragDropActive = false;
        int minValue = 670;
        int maxValue = 735;
        private Merilo selectedObject = null;
        private Merilo dragDropObject = null;


        public MyICommand<string> NetworkCommand { get; private set; }
        public MyICommand MouseLeftButtonUpCommand { get; set; }
        public MyICommand<Canvas> DropCommand { get; set; }
        public MyICommand<ListView> SelectionChangedCommand { get; set; }

        public MyICommand<Canvas> FreeCanv1Command { get; set; }
        public MyICommand<Canvas> FreeCanv2Command { get; set; }
        public MyICommand<Canvas> FreeCanv3Command { get; set; }
        public MyICommand<Canvas> FreeCanv4Command { get; set; }
        public MyICommand<Canvas> FreeCanv5Command { get; set; }
        public MyICommand<Canvas> FreeCanv6Command { get; set; }




        private static ObservableCollection<Merilo> objects = new ObservableCollection<Merilo>();
        public static ObservableCollection<SolidColorBrush> borderBrushes = new ObservableCollection<SolidColorBrush>();
        public Dictionary<string, int> canvas = new Dictionary<string, int>();
        public static ObservableCollection<Merilo> DroppedObjects { get; set; }



        public ObservableCollection<Merilo> Objects
        {

            get { return objects; }

            set
            {

                if (objects != value)
                {
                    objects = value;
                    OnPropertyChanged("Objects");
                }
            }

        }

        public string NetworkTerminal
        {

            get { return networkTerminal; }
            set
            {
                if (networkTerminal != value)
                {
                    networkTerminal = value;
                    OnPropertyChanged("NetworkTerminal");
                }
            }

        }

        public string NetworkTerminal2
        {
            get { return networkTerminal2; }
            set
            {
                if (networkTerminal2 != value)
                {
                    networkTerminal2 = value;
                    OnPropertyChanged("NetworkTerminal2");
                }
            }
        }


        public ObservableCollection<SolidColorBrush> BorderBrushes
        {

            get { return borderBrushes; }

            set
            {

                if (borderBrushes != value)
                {
                    borderBrushes = value;
                    OnPropertyChanged("BorderBrushes");
                }
            }

        }

        public Merilo SelectedObject
        {

            get { return selectedObject; }

            set
            {
                if (selectedObject != value)
                {
                    selectedObject = value;
                    OnPropertyChanged("SelectedObject");
                }

            }
        }

        public Merilo DragDropObject
        {

            get { return dragDropObject; }

            set
            {
                if (dragDropObject != value)
                {
                    dragDropObject = value;
                    OnPropertyChanged("DragDropObject");
                }

            }
        }


        public MeriloViewModel()
        {
            
            
            networkTerminal2 = ">>";
            Objects.Clear();
            BorderBrushes.Clear();

            for (int i = 0; i < 7; i++)
            {
                borderBrushes.Add(Brushes.Black);

            }

            foreach (Merilo r in Kolekcija.AllObjects)
            {
                Objects.Add(new Merilo(r.Id, r.Name, r.Tip, r.Valuee));
            }


            DroppedObjects = new ObservableCollection<Merilo>();
            selectedObject = new Merilo();
            dragDropObject = new Merilo();


            NetworkCommand = new MyICommand<string>(OnNav);
            DropCommand = new MyICommand<Canvas>(OnDrop);
            SelectionChangedCommand = new MyICommand<ListView>(OnSelectionChanged);
            MouseLeftButtonUpCommand = new MyICommand(OnMouseLeftButtonUp);

            FreeCanv1Command = new MyICommand<Canvas>(OnFreeCanv1);
            FreeCanv2Command = new MyICommand<Canvas>(OnFreeCanv2);
            FreeCanv3Command = new MyICommand<Canvas>(OnFreeCanv3);
            FreeCanv4Command = new MyICommand<Canvas>(OnFreeCanv4);
            FreeCanv5Command = new MyICommand<Canvas>(OnFreeCanv5);
            FreeCanv6Command = new MyICommand<Canvas>(OnFreeCanv6);




            Task.Run(() =>
            {
                while (true)
                {
                    provjera();
                    System.Threading.Thread.Sleep(1000);
                }

            });





        }

   

        private void OnNav(string destination)
        {
            if (destination == "enter")
            {
                switch (NetworkTerminal)
                {
                    case "networkData":
                        MainWindowViewModel.currentViewModel = MainWindowViewModel.networkDataViewModel;
                        MainWindowViewModel.RisePropChanged();
                        break;
                    case "chart":
                        MainWindowViewModel.currentViewModel = MainWindowViewModel.dataChartViewModel;
                        MainWindowViewModel.RisePropChanged();
                        break;
                    case "report":
                        MainWindowViewModel.currentViewModel = MainWindowViewModel.reportViewModel;
                        MainWindowViewModel.RisePropChanged();
                        break;
                    default:
                        NetworkTerminal = "";
                        NetworkTerminal2 = "Wrong Command!";
                        break;


                }

            }
        }

        public void provjera()
        {

            string canvasToChange = "";  // ime canvasa koji se mijenja
            int idToChange = -1;  // redni br canvasa koji treba da se promijeni

            if (DroppedObjects.Count > 0)
            {

                foreach (Merilo r in DroppedObjects)
                {
                    foreach (Merilo mjeriloObject in Kolekcija.AllObjects)
                    {
                        if (mjeriloObject.Id == r.Id)
                        {

                            foreach (KeyValuePair<string, int> kvp in canvas)
                            {
                                if (kvp.Value == r.Id)
                                {

                                    canvasToChange = kvp.Key;

                                    idToChange = Int32.Parse(canvasToChange.Split('v')[1]);


                                    if (mjeriloObject.Valuee > maxValue || mjeriloObject.Valuee < minValue)
                                    {

                                        borderBrushes[idToChange] = Brushes.Red;
                                        OnPropertyChanged("BorderBrushes");
                                        break;
                                    }
                                    else
                                    {
                                        borderBrushes[idToChange] = Brushes.LawnGreen;
                                        OnPropertyChanged("BorderBrushes");
                                        break;


                                    }
                                }
                            }
                        }

                    }
                }
            }
        }





        public void OnDrop(Canvas canv)
        {

            if (DragDropObject != null)
            {

                if ((canv).Resources["taken"] == null)

                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(DragDropObject.Tip.Img_Src);
                    img.EndInit();

                    (canv).Background = new ImageBrush(img);

                    ((TextBlock)(canv).Children[1]).Text = " ID[" + DragDropObject.Id.ToString() + "]  Name[" + DragDropObject.Name + "]";
                    (canv).Resources.Add("taken", true);




                    for (int i = 0; i < Objects.Count; i++)
                    {
                        if (Objects[i].Id == DragDropObject.Id)
                        {

                            DroppedObjects.Add(Objects[i]);
                            Objects.RemoveAt(i);
                            canvas[canv.Name] = DragDropObject.Id;
                            break;
                        }
                    }
                    OnPropertyChanged("Objects");
                }
                DragDropObject = null;
                dragDropActive = false;

            }


        }

        private void OnSelectionChanged(ListView lvObjects)
        {

            if (!dragDropActive)
            {
                DragDropObject = new Merilo(SelectedObject.Id, SelectedObject.Name, SelectedObject.Tip, SelectedObject.Valuee);
                dragDropActive = true;
                DragDrop.DoDragDrop(lvObjects, DragDropObject, DragDropEffects.Copy | DragDropEffects.Move);

            }



        }

        private void OnMouseLeftButtonUp()
        {
            DragDropObject = null;
            selectedObject = null;
            dragDropActive = false;
        }


        private void OnFreeCanv1(Canvas canv)
        {
            if (canv.Resources["taken"] != null)
            {
                string[] lineParts = ((TextBlock)(canv).Children[1]).Text.Split('[', ']');  //ID[1] Name[1]
                int id = Int32.Parse(lineParts[1]);

                foreach (Merilo r in DroppedObjects.ToList())
                {
                    if (r.Id == id)
                    {
                        Objects.Add(r);
                        DroppedObjects.Remove(r);
                    }
                }

                canvas.Remove(canv.Name);
                borderBrushes[1] = Brushes.Black;

                canv.Background = Brushes.White;
                ((TextBlock)(canv).Children[1]).Text = "Available";
                ((TextBlock)(canv).Children[1]).Foreground = Brushes.Black;
                ((TextBlock)(canv).Children[1]).Background = Brushes.AliceBlue;
                (canv).Resources.Remove("taken");
            }
        }


        private void OnFreeCanv2(Canvas canv)
        {
            if (canv.Resources["taken"] != null)
            {
                string[] lineParts = ((TextBlock)(canv).Children[1]).Text.Split('[', ']');
                int id = Int32.Parse(lineParts[1]);

                foreach (Merilo r in DroppedObjects.ToList())
                {
                    if (r.Id == id)
                    {
                        Objects.Add(r);
                        DroppedObjects.Remove(r);
                    }
                }

                canvas.Remove(canv.Name);
                borderBrushes[2] = Brushes.Black;

                canv.Background = Brushes.White;
                ((TextBlock)(canv).Children[1]).Text = "Available";
                ((TextBlock)(canv).Children[1]).Foreground = Brushes.Black;
                ((TextBlock)(canv).Children[1]).Background = Brushes.AliceBlue;
                (canv).Resources.Remove("taken");
            }
        }
        private void OnFreeCanv3(Canvas canv)
        {
            if (canv.Resources["taken"] != null)
            {
                string[] lineParts = ((TextBlock)(canv).Children[1]).Text.Split('[', ']');
                int id = Int32.Parse(lineParts[1]);

                foreach (Merilo r in DroppedObjects.ToList())
                {
                    if (r.Id == id)
                    {
                        Objects.Add(r);
                        DroppedObjects.Remove(r);
                    }
                }

                canvas.Remove(canv.Name);
                borderBrushes[3] = Brushes.Black;

                canv.Background = Brushes.White;
                ((TextBlock)(canv).Children[1]).Text = "Available";
                ((TextBlock)(canv).Children[1]).Foreground = Brushes.Black;
                ((TextBlock)(canv).Children[1]).Background = Brushes.AliceBlue;
                (canv).Resources.Remove("taken");
            }
        }
        private void OnFreeCanv4(Canvas canv)
        {
            if (canv.Resources["taken"] != null)
            {
                string[] lineParts = ((TextBlock)(canv).Children[1]).Text.Split('[', ']');
                int id = Int32.Parse(lineParts[1]);

                foreach (Merilo r in DroppedObjects.ToList())
                {
                    if (r.Id == id)
                    {
                        Objects.Add(r);
                        DroppedObjects.Remove(r);
                    }
                }

                canvas.Remove(canv.Name);
                borderBrushes[4] = Brushes.Black;

                canv.Background = Brushes.White;
                ((TextBlock)(canv).Children[1]).Text = "Available";
                ((TextBlock)(canv).Children[1]).Foreground = Brushes.Black;
                ((TextBlock)(canv).Children[1]).Background = Brushes.AliceBlue;
                (canv).Resources.Remove("taken");
            }
        }

        private void OnFreeCanv5(Canvas canv)
        {
            if (canv.Resources["taken"] != null)
            {
                string[] lineParts = ((TextBlock)(canv).Children[1]).Text.Split('[', ']');
                int id = Int32.Parse(lineParts[1]);

                foreach (Merilo r in DroppedObjects.ToList())
                {
                    if (r.Id == id)
                    {
                        Objects.Add(r);
                        DroppedObjects.Remove(r);
                    }
                }

                canvas.Remove(canv.Name);
                borderBrushes[5] = Brushes.Black;

                canv.Background = Brushes.White;
                ((TextBlock)(canv).Children[1]).Text = "Available";
                ((TextBlock)(canv).Children[1]).Foreground = Brushes.Black;
                ((TextBlock)(canv).Children[1]).Background = Brushes.AliceBlue;
                (canv).Resources.Remove("taken");
            }
        }
        private void OnFreeCanv6(Canvas canv)
        {
            if (canv.Resources["taken"] != null)
            {
                string[] lineParts = ((TextBlock)(canv).Children[1]).Text.Split('[', ']');
                int id = Int32.Parse(lineParts[1]);

                foreach (Merilo r in DroppedObjects.ToList())
                {
                    if (r.Id == id)
                    {
                        Objects.Add(r);
                        DroppedObjects.Remove(r);
                    }
                }

                canvas.Remove(canv.Name);
                borderBrushes[6] = Brushes.Black;

                canv.Background = Brushes.White;
                ((TextBlock)(canv).Children[1]).Text = "Available";
                ((TextBlock)(canv).Children[1]).Foreground = Brushes.Black;
                ((TextBlock)(canv).Children[1]).Background = Brushes.AliceBlue;
                (canv).Resources.Remove("taken");
            }
        }
    }
}
