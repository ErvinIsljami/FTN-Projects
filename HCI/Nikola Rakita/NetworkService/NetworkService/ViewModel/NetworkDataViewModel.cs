using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NetworkService.ViewModel
{
    public class NetworkDataViewModel : BindableBase
    {

        private string textBoxNetworkData;
        private string idToBeDeleted = "";
        private string newID;
        private string newName;

        private bool searchName = false;
        private bool searchType = false;

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
        public MyICommand SearchCommand { get; set; }
        public MyICommand ResetFilterCommand { get; set; }
        private static ObservableCollection<Generator> generatori;
        private static ObservableCollection<Generator> filtriraniGeneratori;
        private ObservableCollection<Generator> networkDataGeneratori;
        public ObservableCollection<Generator> NetworkDataGeneratori {

            get { return networkDataGeneratori; }

            set
            {

                if (networkDataGeneratori != value)
                {
                    networkDataGeneratori = value;
                    OnPropertyChanged("NetworkDataGeneratori");
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
        public bool SearchName
        {
            get { return searchName; }
            set
            {
                if (searchName != value)
                {
                    searchName = value;
                    OnPropertyChanged("SearchName");
                }
            }
        }
        public bool SearchType
        {
            get { return searchType; }
            set
            {
                if (searchType != value)
                {
                    searchType = value;
                    OnPropertyChanged("SearchType");
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
        public  ObservableCollection<Generator> Generatori
        {
            get { return generatori; }

            set
            {

                if (generatori != value)
                {
                    generatori = value;
                    OnPropertyChanged("Generatori");
                }
            }
        }
        public  ObservableCollection<Generator> FilitriraniGeneratori
        {
            get { return filtriraniGeneratori; }

            set
            {

                if (filtriraniGeneratori != value)
                {
                    filtriraniGeneratori = value;
                    OnPropertyChanged("FilitriraniGeneratori");
                }
            }
        }
        public NetworkDataViewModel()
        {
            Generatori = Kolekcija.AllObjects;
            Type1 = new Tip("WindGen", Environment.CurrentDirectory + @"\" + "windGen.jpg");
            Type2 = new Tip("Solar", Environment.CurrentDirectory + @"\" + "solarGen.jpg");
            networkDataTerminal2 = ">>";

            FilterTextBoxBrush = Brushes.Black;
            RadioButton1Brash = Brushes.Black;
            RadioButton2Brash = Brushes.Black;
            

            NetworkDataCommand = new MyICommand<string>(OnNav);
            NameCommand = new MyICommand(OnName);
            TypeCommand = new MyICommand(OnType);
            SearchCommand = new MyICommand(OnSearch);

            FilitriraniGeneratori = new ObservableCollection<Generator>();
            NetworkDataGeneratori = new ObservableCollection<Generator>();

            NetworkDataGeneratori = Generatori;
            

        }
        private void OnNav(string destination)
        {
            //imamo dva mini terminala, levi i desni.. ovaj je levi, i u odnosu na ono sto pise trenutno prelazi na sledece
            //nesto slicno kao state masina, ako znas kako radi state masina
            //u sustini od jednog stanja prelazi na drugo, sa id prelazi na name, pa na type
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
                    NetworkDataTerminal2 = "Choose Type (1-WindGen, 2-Solar):";
                    break;
                case "Choose Type (1-WindGen, 2-Solar):":
                    if (NetworkDataTerminal == "1")
                    {
                        Kolekcija.AllObjects.Add(new Generator(Int32.Parse(NewID), newName, Type1, 0));
                        NetworkDataTerminal = "";
                        NetworkDataTerminal2 = ">>";
                        NewID = "";
                        NewName = "";    
                    }
                    else if (NetworkDataTerminal == "2")
                    {
                        Kolekcija.AllObjects.Add(new Generator(Int32.Parse(NewID), newName, Type2, 0));
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
                        foreach (Generator r in Kolekcija.AllObjects)
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
                //navigacija unutar terminala gde korisnik pise komandu
                //moze jedan od ovih case-ova da pise

                switch (NetworkDataTerminal)
                {
                    case "addNew":
                        NetworkDataTerminal2 = "Input Id(must be int):";    //ovako setuje levi terminal, i onda onaj gore switch case radi na osnovu ovog sto upises
                        NetworkDataTerminal = "";
                        break;
                    case "delete":
                        NetworkDataTerminal2 = "Input Id to delete(must be int):";  
                        NetworkDataTerminal = "";
                        break;
                    case "network": 
                        MainWindowViewModel.currentViewModel = MainWindowViewModel.homeViewModel;
                        MainWindowViewModel.RaisePropertyChanged();
                        break;
                    case "chart":
                        MainWindowViewModel.currentViewModel = MainWindowViewModel.dataChartViewModel;
                        MainWindowViewModel.RaisePropertyChanged();
                        break;
                    default:
                        NetworkDataTerminal = "";
                        break;
                }
            }
        }
        private void OnName()
        {

            SearchName = true;
            SearchType = false;
            
            
        }
        private void OnType()
        {


            SearchName = false;
            SearchType = true;
        }
        
        //komanda koja se izvrsava kada se klikne search
        private void OnSearch()
        {
            //proverava da li je prazan text za pretragu i da li su radio buttoni neselektovani
            if (string.IsNullOrEmpty(TextBoxNetworkData) && SearchName == false && SearchType == false)
            {
                //zacrveni ih sve ako jesu
                FilterTextBoxBrush = Brushes.Red;
                RadioButton1Brash = Brushes.Red;
                RadioButton2Brash = Brushes.Red;
                return;
            }
            //proverava da li je text prazan, ako jeste zacrveni ga a radio buttone ne
            else if (string.IsNullOrEmpty(TextBoxNetworkData))
            {
                FilterTextBoxBrush = Brushes.Red;
                RadioButton1Brash = Brushes.Black;
                RadioButton2Brash = Brushes.Black;
                return;
            }
            //proverava da li su radio buttoni neselektovani, i crveni ih
            else if (SearchName == false && SearchType == false)
            {
                FilterTextBoxBrush = Brushes.Black;
                RadioButton1Brash = Brushes.Red;
                RadioButton2Brash = Brushes.Red;
                return;
            }
            else
            {
                //ako je selektovan bar jedan radio button i ako ima teksta u polju za search
                FilterTextBoxBrush = Brushes.Black;
                RadioButton1Brash = Brushes.Black;
                RadioButton2Brash = Brushes.Black;
            }
            
            //ocisti listu za prikaz pretrage
            FilitriraniGeneratori.Clear();
            //prolazi kroz sve generatore
            foreach (Generator m in Kolekcija.AllObjects)
            {
                if (searchName) //ako je selektovana pretraga po imenu
                {
                    if (m.Name == TextBoxNetworkData)
                    {
                        FilitriraniGeneratori.Add(new Generator(m.Id, m.Name, m.Tip, m.Valuee));
                    }
                }
                else    //ako je selektovana pretraga po tipu
                {
                    if (m.Tip.Name == TextBoxNetworkData)
                    {
                        FilitriraniGeneratori.Add(new Generator(m.Id, m.Name, m.Tip, m.Valuee));
                    }
                }
            }
            TextBoxNetworkData = "";
            NetworkDataGeneratori = FilitriraniGeneratori;
        }
    }
}
