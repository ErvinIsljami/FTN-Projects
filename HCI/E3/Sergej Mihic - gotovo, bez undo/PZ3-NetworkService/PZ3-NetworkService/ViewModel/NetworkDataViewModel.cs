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
        private string idToBeDeleted = "";
        private string newID;
        private string newName;
        private  Tip type1;
        private Tip type2;
        private string networkDataTerminal;
        private string networkDataTerminal2;
        private static ObservableCollection<Mjerilo> mjerila;
        private static ObservableCollection<Mjerilo> pretrazenaMjerila;

        public MyICommand<string> NetworkDataCommand { get; private set; }
        private ObservableCollection<Mjerilo> networkDataMjerila;
        public ObservableCollection<Mjerilo> NetworkDataMjerila {

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
        public  ObservableCollection<Mjerilo> Mjerila
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
        public  ObservableCollection<Mjerilo> PretrazenaMjerila
        {
            get { return pretrazenaMjerila; }

            set
            {
                if (pretrazenaMjerila != value)
                {
					pretrazenaMjerila = value;
                    OnPropertyChanged("PretrazenaMjerila");
                }
            }
        }

        public NetworkDataViewModel()
        {
            Mjerila = Kolekcija.AllObjects;
            Type1 = new Tip("Turbo965", Environment.CurrentDirectory + @"\" + "Turbo965.jpg");
            Type2 = new Tip("ElektroMer71", Environment.CurrentDirectory + @"\" + "ElektroMer71.jpg");
            networkDataTerminal2 = ">>";            

            NetworkDataCommand = new MyICommand<string>(HandleTerminalInput);

			PretrazenaMjerila = new ObservableCollection<Mjerilo>();
            NetworkDataMjerila = new ObservableCollection<Mjerilo>();

            NetworkDataMjerila = Mjerila;
        }

        private void HandleTerminalInput(string destination)
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
                    NetworkDataTerminal2 = "Choose Type (1-Turbo965, 2-ElektroMer71):";
                    break;
                case "Choose Type (1-Turbo965, 2-ElektroMer71):":
                    if (NetworkDataTerminal == "1")
                    {
                        Kolekcija.AllObjects.Add(new Mjerilo(Int32.Parse(NewID), newName, Type1, 0));
                        NetworkDataTerminal = "";
                        NetworkDataTerminal2 = ">>";
                        NewID = "";
                        NewName = "";
                    }
                    else if (NetworkDataTerminal == "2")
                    {
                        Kolekcija.AllObjects.Add(new Mjerilo(Int32.Parse(NewID), newName, Type2, 0));
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
                        foreach (Mjerilo r in Kolekcija.AllObjects)
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
				string[] action = NetworkDataTerminal.Split('.');
				if (action.Length > 1)
				{
					NetworkDataTerminal2 = "";
					if (action[0] == "search")
					{
						string argument = action[2];
						PretrazenaMjerila.Clear();

						if(action[1] == "name")
						{
							foreach (Mjerilo m in Kolekcija.AllObjects)
							{
								if (m.Name.IndexOf(argument, StringComparison.CurrentCultureIgnoreCase) != -1)
								{
									PretrazenaMjerila.Add(new Mjerilo(m.Id, m.Name, m.Tip, m.Valuee));
								}
							}
							NetworkDataMjerila = PretrazenaMjerila;
							NetworkDataTerminal = "";
						}
						else if(action[1] == "type")
						{
							foreach (Mjerilo m in Kolekcija.AllObjects)
							{
								if (m.Tip.Name.IndexOf(argument, StringComparison.CurrentCultureIgnoreCase) != -1)
								{
									PretrazenaMjerila.Add(new Mjerilo(m.Id, m.Name, m.Tip, m.Valuee));
								}
							}
							NetworkDataMjerila = PretrazenaMjerila;
							NetworkDataTerminal = "";
						}
						else
						{
							NetworkDataTerminal2 = "Nepoznati tip za pretragu.";
						}
					}
					else
					{
						NetworkDataTerminal2 = "Nevalidna akcija sa vise parametara.";
					}
				}
				else
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
							MainWindowViewModel.dataChartViewModel.ShowChart();
							break;
						case "reset search":
							ResetSearch();
							NetworkDataTerminal = "";
							break;
						default:
							NetworkDataTerminal = "";
							break;
					}
				}
            }
        }
        private void ResetSearch()
        {
            NetworkDataMjerila = Kolekcija.AllObjects;
        }
    }
}
