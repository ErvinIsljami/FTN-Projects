using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PZ3_NetworkService.ViewModel
{
    public class NetworkDataViewModel : BindableBase
    {

        private string textBoxNetworkData;
        private string idToBeDeleted = "";
        private string newID;
        private string newName;

        private bool filterName = false;
        private bool filterType = false;

        private  Tip type1;
        private Tip type2;

        private string networkDataTerminal;
        private string networkDataTerminal2;

        private System.Windows.Media.Brush filterTextBoxBrush;
        private System.Windows.Media.Brush radioButton1Brash;
        private System.Windows.Media.Brush radioButton2Brash;


        public MyICommand<string> NetworkDataCommand { get; private set; }
        public MyICommand NameCommand { get; set; }
        public MyICommand TypeCommand { get; set; }
        public MyICommand FilterCommand { get; set; }
        public MyICommand ResetFilterCommand { get; set; }


        private static ObservableCollection<Merilo> mjerila;
        private static ObservableCollection<Merilo> filtriranaMjerila;
        

        private ObservableCollection<Merilo> networkDataMjerila;
        public ObservableCollection<Merilo> NetworkDataMjerila {

            get { return networkDataMjerila; }

            set
            {

                if (networkDataMjerila != value)
                {
                    networkDataMjerila = value;
                    OnPropertyChanged("NetworkDataMjerila");
                }
            }
        }


        public string NetworkDataTerminal
        {
            

            get { return networkDataTerminal; }
            set
            {
                if (networkDataTerminal != value)
                {
                    networkDataTerminal = value;
                    OnPropertyChanged("NetworkDataTerminal");
                }
            }

        }

        public string NetworkDataTerminal2
        {
            get { return networkDataTerminal2; }
            set
            {
                if (networkDataTerminal2 != value)
                {
                    networkDataTerminal2 = value;
                    OnPropertyChanged("NetworkDataTerminal2");
                }
            }
        }

        public string NewID { get => newID; set => newID = value; }
        public string NewName { get => newName; set => newName = value; }
        public Tip Type1 { get => type1; set => type1 = value; }
        public Tip Type2 { get => type2; set => type2 = value; }
        public bool FilterName
        {
            get { return filterName; }
            set
            {
                if (filterName != value)
                {
                    filterName = value;
                    OnPropertyChanged("FilterName");
                }
            }
        }
        public bool FilterType
        {
            get { return filterType; }
            set
            {
                if (filterType != value)
                {
                    filterType = value;
                    OnPropertyChanged("FilterType");
                }
            }
        }

        public string TextBoxNetworkData
        {
            get { return textBoxNetworkData; }
            set
            {
                if (textBoxNetworkData != value)
                {
                    textBoxNetworkData = value;
                    OnPropertyChanged("TextBoxNetworkData");
                }
            }
        }

        public Brush FilterTextBoxBrush
        {
            get { return filterTextBoxBrush; }
            set
            {
                if (filterTextBoxBrush != value)
                {
                    filterTextBoxBrush = value;
                    OnPropertyChanged("FilterTextBoxBrush");
                }
            }
        }

        public Brush RadioButton1Brash
        {
            get { return radioButton1Brash; }
            set
            {
                if (radioButton1Brash != value)
                {
                    radioButton1Brash = value;
                    OnPropertyChanged("RadioButton1Brash");
                }
            }
        }
        public Brush RadioButton2Brash
        {
            get { return radioButton2Brash; }
            set
            {
                if (radioButton2Brash != value)
                {
                    radioButton2Brash = value;
                    OnPropertyChanged("RadioButton2Brash");
                }
            }
        }

        public  ObservableCollection<Merilo> Mjerila
        {
            get { return mjerila; }

            set
            {

                if (mjerila != value)
                {
                    mjerila = value;
                    OnPropertyChanged("Mjerila");
                }
            }
        }
        public  ObservableCollection<Merilo> FiltriranaMjerila
        {
            get { return filtriranaMjerila; }

            set
            {

                if (filtriranaMjerila != value)
                {
                    filtriranaMjerila = value;
                    OnPropertyChanged("FiltriranaMjerila");
                }
            }
        }



        public NetworkDataViewModel()
        {
            Mjerila = Kolekcija.AllObjects;
            Type1 = new Tip("Turbo975", Environment.CurrentDirectory + @"\" + "Turbo975.jpg");
            Type2 = new Tip("HidroMer71", Environment.CurrentDirectory + @"\" + "HidroMer71.jpg");
            networkDataTerminal2 = ">>";

            FilterTextBoxBrush = Brushes.Black;
            RadioButton1Brash = Brushes.Black;
            RadioButton2Brash = Brushes.Black;
            

            NetworkDataCommand = new MyICommand<string>(OnNav);
            NameCommand = new MyICommand(OnName);
            TypeCommand = new MyICommand(OnType);
            FilterCommand = new MyICommand(OnFilter);
            ResetFilterCommand = new MyICommand(OnReset);



            FiltriranaMjerila = new ObservableCollection<Merilo>();
            NetworkDataMjerila = new ObservableCollection<Merilo>();

            NetworkDataMjerila = Mjerila;
            

        }


        private void OnNav(string destination)
        {
            switch (NetworkDataTerminal2)
            {
                case "Input Id(must be int):":
                    NewID = NetworkDataTerminal;
                    int pom;
                    if(int.TryParse(NewID, out pom))
                    {
                        NetworkDataTerminal = "";
                        NetworkDataTerminal2 = "Input Name:";
                        

                    }
                    
                    break;
                case "Input Name:":
                    NewName = NetworkDataTerminal;
                    NetworkDataTerminal = "";
                    NetworkDataTerminal2 = "Choose Type (1-Turbo975, 2-HidroMer71):";
                    break;
                case "Choose Type (1-Turbo975, 2-HidroMer71):":
                    if (NetworkDataTerminal == "1")
                    {

                        Kolekcija.AllObjects.Add(new Merilo(Int32.Parse(NewID), newName, Type1, 0));
                        NetworkDataTerminal = "";
                        NetworkDataTerminal2 = ">>";
                        NewID = "";
                        NewName = "";
                        
                    }
                    else if (NetworkDataTerminal == "2")
                    {
                        Kolekcija.AllObjects.Add(new Merilo(Int32.Parse(NewID), newName, Type2, 0));
                        NetworkDataTerminal = "";
                        NetworkDataTerminal2 = ">>";
                        NewID = "";
                        NewName = "";
                        


                    }
                    
                    break;
                case "Input Id to delete(must be int):":
                    int pom2;
                    idToBeDeleted = NetworkDataTerminal;
                    if (int.TryParse(idToBeDeleted, out pom2))
                    {
                        
                        NetworkDataTerminal = "";
                        NetworkDataTerminal2 = "Are you sure?(yes/no):";
                    }
                    break;
                case "Are you sure?(yes/no):":
                    if (NetworkDataTerminal == "yes")
                    {
                        
                        
                        foreach (Merilo r in Kolekcija.AllObjects)
                            if (r.Id == Int32.Parse(idToBeDeleted))
                            {
                                Kolekcija.AllObjects.Remove(r);
                                break;
                            }
                        NetworkDataTerminal = "";
                        NetworkDataTerminal2 = ">>";
                        
                        NetworkDataTerminal = "";
                      
                        break;
                    }
                    else if(NetworkDataTerminal == "no")
                    {
                        NetworkDataTerminal = "";
                        NetworkDataTerminal2 = ">>";
                       
                    }
                    

                    break;



            }


            if (destination == "enter")
            {
                switch (NetworkDataTerminal)
                {
                    case "addNew":
                        NetworkDataTerminal2 = "Input Id(must be int):";
                        NetworkDataTerminal = "";
                        break;
                    case "delete":
                        NetworkDataTerminal2 = "Input Id to delete(must be int):";
                        NetworkDataTerminal = "";
                        break;
                    case "network":
                        MainWindowViewModel.currentViewModel = MainWindowViewModel.homeViewModel;
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
                    case "reset filter":
                        OnReset();
                        break;
                    default:
                        NetworkDataTerminal = "";
                        break;




                }

            }
        }

        private void OnName()
        {

            FilterName = true;
            FilterType = false;
            
            
        }
        private void OnType()
        {


            FilterName = false;
            FilterType = true;
        }

        private void OnReset()
        {


            NetworkDataMjerila = Kolekcija.AllObjects;
            FilterTextBoxBrush = Brushes.Black;

            TextBoxNetworkData = "";
        }


        private void OnFilter()
        {

            if (string.IsNullOrEmpty(TextBoxNetworkData) && FilterName == false && FilterType == false)
            {
                FilterTextBoxBrush = Brushes.Red;
                RadioButton1Brash = Brushes.Red;
                RadioButton2Brash = Brushes.Red;
                return;
            }
            else if (string.IsNullOrEmpty(TextBoxNetworkData))
            {
                FilterTextBoxBrush = Brushes.Red;
                RadioButton1Brash = Brushes.Black;
                RadioButton2Brash = Brushes.Black;
                return;
            }
            else if (FilterName == false && FilterType == false)
            {
                FilterTextBoxBrush = Brushes.Black;
                RadioButton1Brash = Brushes.Red;
                RadioButton2Brash = Brushes.Red;
                return;
            }
            else
            {
                FilterTextBoxBrush = Brushes.Black;
                RadioButton1Brash = Brushes.Black;
                RadioButton2Brash = Brushes.Black;
            }
                





            FiltriranaMjerila.Clear();

            foreach (Merilo m in Kolekcija.AllObjects)
            {

                if (filterName)
                {
                    if (m.Name == TextBoxNetworkData)
                    {

                        FiltriranaMjerila.Add(new Merilo(m.Id, m.Name, m.Tip, m.Valuee));
                        
                    }
                }
                else
                {
                    if (m.Tip.Name == TextBoxNetworkData)
                    {

                        FiltriranaMjerila.Add(new Merilo(m.Id, m.Name, m.Tip, m.Valuee));
                        
                    }


                }
            }
            TextBoxNetworkData = "";

            NetworkDataMjerila = FiltriranaMjerila;
            
            


        }



    }
}
