using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static PZ3_NetworkService.MyCommand;

namespace PZ3_NetworkService.ViewModel
{
    public class NetworkDataViewModel : BindableBase
    {
        public int MaxValue = 16;
        public int MinValue = 5;
        public bool searched = false;
        public bool filtered = false;

        public List<string> ListTypes { get; set; } = Types.TypesNames;

        private ValveModel selectedItem;
        public ValveModel SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        private int selectedIndex=0;
        public int SelectedIndex { get => selectedIndex; set { selectedIndex = value; OnPropertyChanged("SelectedIndex"); } }

        public List<ValveModel> Items { get; set; } = DataBase.Valve_MainStorage.Values.ToList();
        public List<ValveModel> TempSearchList { get; set; }
        public List<ValveModel> SearchResults { get; set; }

        public List<ValveModel> ExResults { get; set; }
        public List<ValveModel> AllResults { get; set; }
        public List<ValveModel> OutResults { get; set; }
        public List<ValveModel> Temp { get; set; }

        public bool ById = false;
        public bool ByName = true;
        public bool All = true;
        public bool Out = false;
        public bool Ex = false;

        public MyICommand<TextBlock> SearchCommand { get; set; }
        public MyICommand<Grid> ResetCommand { get; set; }
        public MyICommand ByNameCheckedCommand { get; set; }
        public MyICommand ByIdCheckedCommand { get; set; }
        public MyICommand AllCommand { get; set; }
        public MyICommand OutCommand { get; set; }
        public MyICommand ExpectedCommand { get; set; }
        public MyICommand DeleteCommand { get; set; }
        public MyICommand AddCommand { get; set; }
        public MyICommand<Image> SelectionChangedCommand { get; set; }
        public MyICommand<TextBox> GotFocusSearchCommand { get; set; }
        public MyICommand<TextBox> LostFocusSearchCommand { get; set; }
        public MyICommand<Grid> LoadedCommand { get; set; }

        private ValveModel item;
        public ValveModel Item
        {
            get => item;
            set { item = value; OnPropertyChanged("Item"); }
        }


        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged("SearchText");
            }
        }

        

        public NetworkDataViewModel()
        {
            ByNameCheckedCommand = new MyICommand(OnNameChecked);
            ByIdCheckedCommand = new MyICommand(OnIdChecked);
            SearchCommand = new MyICommand<TextBlock>(OnSearch);
            ResetCommand = new MyICommand<Grid>(OnReset);
            AllCommand = new MyICommand(OnAll);
            ExpectedCommand = new MyICommand(OnExpected);
            OutCommand = new MyICommand(OnOut);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            AddCommand = new MyICommand(OnAdd);
            SelectionChangedCommand = new MyICommand<Image>(OnSelectionChanged);
            GotFocusSearchCommand = new MyICommand<TextBox>(OnGotFocusSearch);
            LostFocusSearchCommand = new MyICommand<TextBox>(OnLostFocusSearch);
            LoadedCommand = new MyICommand<Grid>(OnLoad);
            Load();
            Item = new ValveModel();
            AllResults = Items;
        }

        

        private void OnLoad(Grid g)
        {
            TextBox tb = g.FindName("SearchTextBox") as TextBox;
            tb.Text = "Search Objects";
            tb.Foreground = Brushes.Gray;
        }
        
        private void OnGotFocusSearch(TextBox tb)
        {
            if (tb.Text.Trim() == "Search Objects")
                tb.Text = "";
            tb.Foreground = Brushes.White;
        }

        private void OnLostFocusSearch(TextBox tb)
        {
            if (tb.Text.Trim() == String.Empty)
            {
                tb.Text = "Search Objects";
                tb.Foreground = Brushes.Gray;
            }
            else
            {
                tb.Foreground = Brushes.White;
            }
        }



        private void OnSelectionChanged(Image img)
        {
            if(selectedIndex==1)
            {
                img.Source = new BitmapImage(new Uri(Types.TypesList[1].ImgUri));
                Item.Type.Name = "IA";
                Item.Type.ImgUri = Types.TypesList[1].ImgUri;
            }
            else if(selectedIndex==2)
            {
                img.Source = new BitmapImage(new Uri(Types.TypesList[2].ImgUri));
                Item.Type.Name = "IB";
                Item.Type.ImgUri = Types.TypesList[2].ImgUri;
            }
            else
            {
                img.Source = null;
                Item.Type.Name = "Select type";
                Item.Type.ImgUri = null;
            }
        }

        private bool CanDelete()
        {
            if(SelectedItem != null)
            {
                bool inUse = false;
                foreach (var item in DataBase.ValveCanvas_Storage.Values)
                {
                    if (item.Id == SelectedItem.Id)
                    {
                        inUse = true;
                        break;
                    }
                }

                if (inUse)
                    return false;
                return true;
            }
            return false;
        }

        public void OnAdd()
        {
            Item.Validate();
            if (Item.IsValid)
            {
                DataBase.Valve_MainStorage.Add(Item.Id, new ValveModel()
                { 
                    Id = Item.Id,
                    Type = new Model.Type(Item.Type.Name, Item.Type.ImgUri),
                    Name = Item.Name
                });


                Loger.Log($"{DateTime.Now.ToString()},Object,{DataBase.ItemsCount},{item.Name},{item.Value},ADD{Environment.NewLine}");
                DataBase.logtext += $"{DateTime.Now.ToString()},Object,{DataBase.ItemsCount},{Item.Name},{item.Value},ADD {Environment.NewLine}";
                ++DataBase.ItemsCount;

                if (Item.Type.Name == "IA")
                    ++DataBase.IACount;
                else
                    ++DataBase.IBCount;
                Items = DataBase.Valve_MainStorage.Values.ToList();
                SelectedIndex = 0;
                OnPropertyChanged("Items");
                Item = new ValveModel();
            }
           

        }

        private void OnDelete()
        {
            if (SelectedItem.Type.Name == "IA")
                --DataBase.IACount;
            else
                --DataBase.IBCount;

            int i = 0;
            foreach (var item in DataBase.Valve_MainStorage.Values)
            {
                if(item.Id==SelectedItem.Id)
                {
                    Loger.Log($"{DateTime.Now.ToString()},Object,{i},{SelectedItem.Name},{SelectedItem.Value},DELETE{Environment.NewLine}");
                    DataBase.logtext += $"{DateTime.Now.ToString()},Object,{i},{SelectedItem.Name},{item.Value},DELETE {Environment.NewLine}";
                    break;
                }
                ++i;
            }
            

            if (searched)
            {
                if (filtered)
                {
                    SearchResults.Remove(SelectedItem);
                    TempSearchList.Remove(SelectedItem);
                    
                    if (Ex)
                    {
                        AllResults.Remove(SelectedItem);
                        ExResults.Remove(SelectedItem);
                        DataBase.Valve_MainStorage.Remove(SelectedItem.Id);
                        Items = DataBase.Valve_MainStorage.Values.ToList();
                        OnPropertyChanged("Items");
                        Items = ExResults;
                        OnPropertyChanged("Items");
                    }
                    else if (Out)
                    {
                        AllResults.Remove(SelectedItem);
                        OutResults.Remove(SelectedItem);
                        DataBase.Valve_MainStorage.Remove(SelectedItem.Id);
                        Items = DataBase.Valve_MainStorage.Values.ToList();
                        OnPropertyChanged("Items");
                        Items = OutResults;
                        OnPropertyChanged("Items");
                    }
                    else
                    {
                        AllResults.Remove(SelectedItem);
                        DataBase.Valve_MainStorage.Remove(SelectedItem.Id);
                        Items = DataBase.Valve_MainStorage.Values.ToList();
                        OnPropertyChanged("Items");
                        Items = AllResults;
                        OnPropertyChanged("Items");
                    }

                }
                else
                {
                    TempSearchList.Remove(SelectedItem);
                    SearchResults.Remove(SelectedItem);
                    AllResults.Remove(SelectedItem);
                    DataBase.Valve_MainStorage.Remove(SelectedItem.Id);
                    Items=DataBase.Valve_MainStorage.Values.ToList();
                    OnPropertyChanged("Items");
                    Items = SearchResults;
                    OnPropertyChanged("Items");
                }
            }
            else
            {
                if (filtered)
                {
                    if(Ex)
                    {
                        AllResults.Remove(SelectedItem);
                        ExResults.Remove(SelectedItem);
                        DataBase.Valve_MainStorage.Remove(SelectedItem.Id);
                        Items = DataBase.Valve_MainStorage.Values.ToList();
                        OnPropertyChanged("Items");
                        Items = ExResults;
                        OnPropertyChanged("Items");
                    }
                    else if(Out)
                    {
                        AllResults.Remove(SelectedItem);
                        OutResults.Remove(SelectedItem);
                        DataBase.Valve_MainStorage.Remove(SelectedItem.Id);
                        Items = DataBase.Valve_MainStorage.Values.ToList();
                        OnPropertyChanged("Items");
                        Items = OutResults;
                        OnPropertyChanged("Items");
                    }
                    else
                    {
                        AllResults.Remove(SelectedItem);
                        DataBase.Valve_MainStorage.Remove(SelectedItem.Id);
                        Items = DataBase.Valve_MainStorage.Values.ToList();
                        OnPropertyChanged("Items");
                        Items = AllResults;
                        OnPropertyChanged("Items");
                    }

                    
                }
                else
                {
                    DataBase.Valve_MainStorage.Remove(SelectedItem.Id);
                    Items = DataBase.Valve_MainStorage.Values.ToList();
                    OnPropertyChanged("Items");
                    
                }
            }

        }


        private void OnOut()
        {
            filtered = true;
            if (!searched)
            {
                Temp = new List<ValveModel>();
                Temp = AllResults;
                if (All)
                {
                    AllResults = Items;
                    OutResults = new List<ValveModel>();
                    foreach (var item in AllResults)
                    {
                        if (item.Value > MaxValue || item.Value < MinValue)
                            OutResults.Add(item);
                    }
                    Items = OutResults;
                    base.OnPropertyChanged("Items");
                }
                else
                {
                    OutResults = new List<ValveModel>();
                    foreach (var item in Temp)
                    {
                        if (item.Value > MaxValue || item.Value < MinValue)
                            OutResults.Add(item);
                    }
                    Items = OutResults;
                    base.OnPropertyChanged("Items");

                }

            }
            else
            {
                Temp = new List<ValveModel>();
                Temp = SearchResults;
                if (All)
                {
                    OutResults = new List<ValveModel>();
                    foreach (var item in SearchResults)
                    {
                        if (item.Value > MaxValue || item.Value < MinValue)
                            OutResults.Add(item);
                    }
                    Items = OutResults;
                    base.OnPropertyChanged("Items");
                }
                else
                {
                    OutResults = new List<ValveModel>();
                    foreach (var item in Temp)
                    {
                        if (item.Value > MaxValue || item.Value < MinValue)
                            OutResults.Add(item);
                    }
                    Items = OutResults;
                    base.OnPropertyChanged("Items");

                }
            }


            All = false;
            Out = true;
            Ex = false;
        }

        private void OnExpected()
        {
            filtered = true;
            if (!searched)
            {
                Temp = new List<ValveModel>();
                Temp = AllResults;
                if (All)
                {
                    AllResults = Items;
                    ExResults = new List<ValveModel>();
                    foreach (var item in AllResults)
                    {
                        if (item.Value <= MaxValue && item.Value >= MinValue)
                            ExResults.Add(item);
                    }
                    Items = ExResults;
                    base.OnPropertyChanged("Items");
                }
                else
                {
                    ExResults = new List<ValveModel>();
                    foreach (var item in Temp)
                    {
                        if (item.Value <= MaxValue && item.Value >= MinValue)
                            ExResults.Add(item);
                    }
                    Items = ExResults;
                    base.OnPropertyChanged("Items");

                }

            }
            else
            {
                Temp = new List<ValveModel>();
                Temp = SearchResults;
                if (All)
                {
                    ExResults = new List<ValveModel>();
                    foreach (var item in SearchResults)
                    {
                        if (item.Value <= MaxValue && item.Value >= MinValue)
                            ExResults.Add(item);
                    }
                    Items = ExResults;
                    base.OnPropertyChanged("Items");
                }
                else
                {
                    ExResults = new List<ValveModel>();
                    foreach (var item in Temp)
                    {
                        if (item.Value <= MaxValue && item.Value >= MinValue)
                            ExResults.Add(item);
                    }
                    Items = ExResults;
                    base.OnPropertyChanged("Items");

                }
            }

            All = false;
            Out = false;
            Ex = true;
        }

        private void OnAll()
        {
            filtered = false;
            if (!searched)
            {
                Items = AllResults;
                base.OnPropertyChanged("Items");
            }
            else
            {
                Items = SearchResults;
                base.OnPropertyChanged("Items");
            }


            All = true;
            Out = false;
            Ex = false;
        }

        private void OnReset(Grid grid)
        {
            RadioButton rb1 = grid.FindName("NameRB") as RadioButton;
            RadioButton rb2 = grid.FindName("IdRB") as RadioButton;
            RadioButton rb3 = grid.FindName("OutRB") as RadioButton;
            RadioButton rb4 = grid.FindName("AllRB") as RadioButton;
            RadioButton rb5 = grid.FindName("ExRB") as RadioButton;
            TextBox tb = grid.FindName("SearchTextBox") as TextBox;
            tb.Text = "Search Objects";
            tb.Foreground = Brushes.Gray;
            rb1.IsChecked = true;
            rb2.IsChecked = false;
            rb3.IsChecked = false;
            rb4.IsChecked = true;
            rb5.IsChecked = false;
            ById = false;
            ByName = true;
            All = true;
            Out = false;
            Ex = false;
            if (searched || filtered)
            {
                Items = DataBase.Valve_MainStorage.Values.ToList();
                OnPropertyChanged("Items");
                searched = false;
                filtered = false;
            }
        }

        private void OnSearch(TextBlock block)
        {
            if (String.IsNullOrEmpty(searchText))
            {
                block.Text = "Cannot be empty!";
                block.Foreground = Brushes.Red;
                return;
            }
            else
            {
                block.Text = "";
                if (ById == false && ByName == false)
                {
                    return;
                }
                else if (ById)
                {
                    TempSearchList = Items;
                    SearchResults = new List<ValveModel>();
                    foreach (var item in Items)
                    {
                        if (item.Id.ToString() == SearchText)
                            SearchResults.Add(item);
                    }
                    searched = true;
                    Items = SearchResults;
                    base.OnPropertyChanged("Items");
                }
                else if (ByName)
                {
                    TempSearchList = Items;
                    SearchResults = new List<ValveModel>();
                    foreach (var item in Items)
                    {
                        if (item.Name.Contains(SearchText))
                            SearchResults.Add(item);
                    }
                    searched = true;
                    Items = SearchResults;
                    base.OnPropertyChanged("Items");
                }
            }

        }

        private void OnNameChecked()
        {
            ByName = true;
            ById = false;
        }

        private void OnIdChecked()
        {
            ByName = false;
            ById = true;
        }

        private void Load()
        {
            Items = DataBase.Valve_MainStorage.Values.ToList();
        }

    }
}
