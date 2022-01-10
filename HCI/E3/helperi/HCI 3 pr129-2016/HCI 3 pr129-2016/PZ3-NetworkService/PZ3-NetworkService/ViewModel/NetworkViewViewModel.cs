using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PZ3_NetworkService.ViewModel
{
    public class NetworkViewViewModel : BindableBase
    {
        private static ObservableCollection<Valve> valvesList;
        public ObservableCollection<Valve> ValvesList { get => valvesList; set { valvesList = value; OnPropertyChanged("ValvesList"); } }

        public static Valve draggedItem = null;
        private bool dragging = false;
        private ListView listView;

        bool elementIn = false;

        public MyICommand<ListView> MLBUCommand { get; set; }
        public MyICommand<Valve> SCCommand { get; set; }
        public MyICommand<ListView> LLWCommand { get; set; }
        public MyICommand<Canvas> DCommand { get; set; }
        public MyICommand<Canvas> LCommand { get; set; }
        public MyICommand<Canvas> FreeCommand { get; set; }

        public List<Canvas> CanvasList { get; set; } = new List<Canvas>();

        public NetworkViewViewModel()
        {
            
            valvesList = new ObservableCollection<Valve>();
            
   

            
            foreach (var item in GlobalValves.AllObjects)
            {
                elementIn = false;
                foreach (var i in GlobalValves.DDbase.Values)
                {
                    if (i.ID == item.ID)
                    {
                        elementIn = true;
                        break;
                    }
                }
                if (!elementIn)
                    valvesList.Add(new Valve { ID = item.ID, Name = item.Name, ValveType = new ValveType(item.ValveType.Name, item.ValveType.Img_src), Val = item.Val });
            }
            

            MLBUCommand = new MyICommand<ListView>(MouseLeftButtonUp);
            SCCommand = new MyICommand<Valve>(SelectionChange);
            LLWCommand = new MyICommand<ListView>(OnLLW);
            DCommand = new MyICommand<Canvas>(OnDrop);
            LCommand = new MyICommand<Canvas>(OnLoad);
            FreeCommand = new MyICommand<Canvas>(free);
        }

        public void MouseLeftButtonUp(ListView lw)
        {
            draggedItem = null;
            lw.SelectedItem = null;
            dragging = false;
        }
        public void SelectionChange(Valve m)
        {
            if (!dragging)
            {
                dragging = true;
                draggedItem = new Valve { ID = m.ID, Name = m.Name, ValveType = new ValveType(m.ValveType.Name, m.ValveType.Img_src), Val = m.Val };

                DragDrop.DoDragDrop(listView, draggedItem, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        public void OnLLW(ListView listview) 
        {
            listView = listview;
        }

        public void OnLoad(Canvas c)    
        {
            Border b = c.Parent as Border;
            TextBlock tb = c.Children[1] as TextBlock;
            if (!GlobalValves.NVBorder.ContainsKey(c.Name))
            {   
                GlobalValves.NVBorder.Add(c.Name, b);

                GlobalValves.NVState.Add(c.Name, tb);

            }

            if (GlobalValves.DDbase.ContainsKey(c.Name))
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(GlobalValves.DDbase[c.Name].ValveType.Img_src);
                logo.EndInit();
                c.Background = new ImageBrush(logo);

                c.Resources["taken"] = true;
            }

        }

        public void OnDrop(Canvas c)    
        {
            if (draggedItem != null)
            {
                if (c.Resources["taken"] == null)
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(draggedItem.ValveType.Img_src);
                    logo.EndInit();
                    c.Background = new ImageBrush(logo);
                    GlobalValves.DDbase[c.Name] = draggedItem;
                    int index = GetIndex(draggedItem);
                    GlobalValves.AllObjects[index].PropertyChanged += DraggedItem_PropertyChanged;
                    Border b = c.Parent as Border;
                    TextBlock tb = c.Children[1] as TextBlock;
                    b?.Dispatcher.Invoke(() =>
                    {
                        if (draggedItem.Val < 5 || draggedItem.Val > 16)
                        { 
                            tb.Foreground = Brushes.Red;
                            tb.Text = "Value:" + draggedItem.Val + "MP";
                        }
                        else
                        {

                            tb.Foreground = Brushes.Green;
                            tb.Text = "Value:" + draggedItem.Val + "MP";
                        }
                    });
                    c.Resources.Add("taken", true);

                    ((TextBlock)c.Children[0]).Text = "  " + draggedItem.Name;
                    ((TextBlock)c.Children[0]).Background = Brushes.Black;
                    ((TextBlock)c.Children[0]).Opacity = 0.8;

                    ((TextBlock)c.Children[1]).Background = Brushes.Black;
                    ((TextBlock)c.Children[1]).Opacity = 0.8;

                    foreach (var item in ValvesList)
                    {
                        if (item.ID == draggedItem.ID)
                        {
                            ValvesList.Remove(item);
                            break;
                        }
                    }
                    OnPropertyChanged("ValvesList");
                }
                dragging = false;
            }
        }
        private int GetIndex(Valve obj)
        {
            int index = -1;
            foreach (Valve currObj in GlobalValves.AllObjects)
            {
                ++index;
                if (currObj.ID == obj.ID)
                {
                    return index;
                }
            }
            return -1;
        }
        private void DraggedItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Valve obj = sender as Valve;
            Debug.WriteLine($"{obj.Val},{obj.Name}");
            foreach (KeyValuePair<string, Valve> pair in GlobalValves.DDbase)
            {
                if (pair.Value.ID == obj.ID)
                {
                    string canvasName = pair.Key;
                    GlobalValves.NVBorder[canvasName].Dispatcher.Invoke(() =>
                    {
                        if (obj.Val < 5 || obj.Val > 16)
                        {
                            GlobalValves.NVState[canvasName].Foreground = Brushes.Red;
                            GlobalValves.NVState[canvasName].Text = "Value:" + obj.Val + "MP";
                        }
                        else
                        {
                            GlobalValves.NVState[canvasName].Foreground = Brushes.Green;
                            GlobalValves.NVState[canvasName].Text = "Value:" + obj.Val + "MP";
                        }
                    });
                }
            }


        }

        public void free(Canvas c)
        {
            if (c.Resources["taken"] != null)
            {
                c.Background = Brushes.White;
                c.Resources.Remove("taken");

                ValvesList.Add(GlobalValves.DDbase[c.Name]);
                OnPropertyChanged("ValvesList");

                int index = GetIndex(GlobalValves.DDbase[c.Name]);
                GlobalValves.AllObjects[index].PropertyChanged -= DraggedItem_PropertyChanged;
                Border b = c.Parent as Border;
                TextBlock tb = c.Children[1] as TextBlock;
                b?.Dispatcher.Invoke(() =>
                {
                    tb.Foreground = Brushes.Transparent;
                });
                GlobalValves.DDbase.Remove(c.Name);

                ((TextBlock)c.Children[0]).Text = "";
                ((TextBlock)c.Children[0]).Background = Brushes.Transparent;
                ((TextBlock)c.Children[0]).Opacity = 1;

                ((TextBlock)c.Children[1]).Text = "";
                ((TextBlock)c.Children[1]).Background = Brushes.Transparent;
                ((TextBlock)c.Children[1]).Opacity = 1;
            }

        }


    }
}
