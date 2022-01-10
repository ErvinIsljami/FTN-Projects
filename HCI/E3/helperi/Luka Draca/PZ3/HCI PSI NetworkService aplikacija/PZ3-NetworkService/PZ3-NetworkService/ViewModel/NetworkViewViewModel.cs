using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static PZ3_NetworkService.MyCommand;
using Type = PZ3_NetworkService.Model.Type;

namespace PZ3_NetworkService.ViewModel
{
    public class NetworkViewViewModel : BindableBase
    {
        public static ValveModel draggedItem = null;
        private bool dragging = false;
        private static bool exists = false;
        private int selectedIndex=0;
        private ListView lv;

        public BindingList<ValveModel> Items { get; set; }
        public MyICommand<ListView> MLBUCommand { get; set; }
        public MyICommand<ValveModel> SCCommand { get; set; }
        public MyICommand<Canvas> DCommand { get; set; }
        public MyICommand<Canvas> FreeCommand { get; set; }
        public MyICommand<Canvas> DOCommand { get; set; }
        public MyICommand<Canvas> DLCommand { get; set; }
        public MyICommand<Canvas> LCommand { get; set; }
        public MyICommand<ListView> LLWCommand { get; set; }
        public int SelectedIndex { get => selectedIndex; set { selectedIndex = value; OnPropertyChanged("SelectedIndex"); } }
        
        public NetworkViewViewModel()
        {
            Items = new BindingList<ValveModel>();
            foreach (var item in DataBase.Valve_MainStorage.Values)
            {
                exists = false;
                foreach (var ex in DataBase.ValveCanvas_Storage.Values)
                {
                    if(ex.Id==item.Id)
                    {
                        exists = true;
                        break;
                    }
                }

                if(exists==false)
                    Items.Add(new ValveModel(item));
            }
            MLBUCommand = new MyICommand<ListView>(OnMLBU);
            SCCommand = new MyICommand<ValveModel>(SelectionChange);
            DCommand = new MyICommand<Canvas>(OnDrop);
            FreeCommand = new MyICommand<Canvas>(OnFree);
            DOCommand = new MyICommand<Canvas>(OnDragOver);
            DLCommand = new MyICommand<Canvas>(OnDragLeave);
            LCommand = new MyICommand<Canvas>(OnLoad);
            LLWCommand = new MyICommand<ListView>(OnLLW);
        }

        public void OnLLW(ListView listview)
        {
            lv = listview;
        }

        public void OnLoad(Canvas c)
        {
            if (DataBase.ValveCanvas_Storage.ContainsKey(c.Name))
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(DataBase.ValveCanvas_Storage[c.Name].Type.ImgUri);
                logo.EndInit();
                c.Background = new ImageBrush(logo);
                ((TextBlock)(c).Children[1]).Text = "";
                c.Resources.Add("taken", true);
                CheckValue(c);
            }
        }

        public void OnDragLeave(Canvas c)
        {
            if (((TextBlock)(c).Children[1]).Text == "Taken!")
            {
                ((TextBlock)(c).Children[1]).Text = "";
                ((TextBlock)(c).Children[1]).Foreground = Brushes.Gray;
            }
        }

        public void OnDragOver(Canvas c)
        {
            if (c.Resources["taken"] != null)
            {
                ((TextBlock)(c).Children[1]).Text = "Taken!";
                ((TextBlock)(c).Children[1]).Foreground = Brushes.Red;
            }
        }

        public void OnFree(Canvas c)
        {
            try
            {
                if (c.Resources["taken"] != null)
                {
                    c.Background = Brushes.Gray;
                    ((TextBlock)c.Children[1]).Foreground = Brushes.Gray;
                    ((Border)c.Children[0]).BorderBrush = Brushes.Transparent;
                   
                    foreach (var item in DataBase.Valve_MainStorage.Values)
                    {
                        if (!Items.Contains(item) && DataBase.ValveCanvas_Storage[c.Name].Id == item.Id)
                        {
                            Items.Add(new ValveModel(item));
                        }
                    }
                    c.Resources.Remove("taken");
                    DataBase.ValveCanvas_Storage.Remove(c.Name);
                }

            }
            catch (Exception) {}
        }


        public void OnDrop(Canvas c)
        {
            if (((TextBlock)(c).Children[1]).Text == "Taken!")
            {
                ((TextBlock)(c).Children[1]).Text = "";
                ((TextBlock)(c).Children[1]).Foreground = Brushes.White;
            }
            if (draggedItem != null)
            {
                if (c.Resources["taken"] == null)
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(draggedItem.Type.ImgUri);
                    logo.EndInit();
                    c.Background = new ImageBrush(logo);
                    DataBase.ValveCanvas_Storage[c.Name] = draggedItem;
                    c.Resources.Add("taken", true);
                    Items.Remove(Items.Single(x=> x.Name==draggedItem.Name));
                    SelectedIndex = 0;
                    CheckValue(c);
                }
                dragging = false;
            }
        }

        public void OnMLBU(ListView lw)
        {
            draggedItem = null;
            lw.SelectedItem = null;
            dragging = false;
        }

        public void SelectionChange(ValveModel o)
        {
            if (!dragging)
            {
                dragging = true;
                draggedItem = new ValveModel(o);
                DragDrop.DoDragDrop(lv, draggedItem, DragDropEffects.Move);
            }
        }

        private void CheckValue(Canvas c)
        {
            Task.Delay(2000).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (DataBase.ValveCanvas_Storage.Count != 0)
                    {
                        if (DataBase.ValveCanvas_Storage.ContainsKey(c.Name))
                        {
                            if (DataBase.Valve_MainStorage[DataBase.ValveCanvas_Storage[c.Name].Id].Value < 5.0 || DataBase.Valve_MainStorage[DataBase.ValveCanvas_Storage[c.Name].Id].Value > 16.0)
                            {
                                ((Border)(c).Children[0]).BorderBrush = Brushes.Red;
                            }
                            else
                            {
                                ((Border)(c).Children[0]).BorderBrush = Brushes.Transparent;
                            }
                        }
                        else
                        {
                            ((Border)(c).Children[0]).BorderBrush = Brushes.Transparent;
                        }
                    }


                });
                CheckValue(c);
            });

        }
    }
}
