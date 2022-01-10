using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using NetworkService.Views;
using NetworkService.Models;

namespace NetworkService.ViewModel
{
    public class NetworkDataViewModel : BindableBase, INotify
    {
        private DataIO serializer = new DataIO();

        static AddWindowView addWindow;

        public ObservableCollection<Road> Roads
        {
            get
            {
                return RoadsObs.Instance.Roads;
            }

            set
            {
                if (RoadsObs.Instance.Roads != value)
                {
                    RoadsObs.Instance.Roads = value;
                    OnPropertyChanged("Roads");
                }
            }
        }

        // Used for sorting, filtering, grouping
        // Represents a wrapper around a particular collection
        public CollectionViewSource FilterCollection { get; set; }

        public MyICommand AddOpenCommand { get; set; }
        public MyICommand AddCommand { get; set; }
        public MyICommand DeleteCommand { get; set; }
        public MyICommand CancelCommand { get; set; }
        public MyICommand FilterCommand { get; set; }

        public MyICommand ClearCommand { get; set; }

        private Road _selectedRoad;
        private string _id;
        private string _label;
        private NetworkService.Models.Type _type;
        private double _value;
        private string _filterType;
        private string _searchName;
        private string _idWarningText;
        private string _labelWarningText;
        private string _typeWarningText;
        private string _idBorder = "Black";
        private string _labelBorder = "Black";
        private string _typeBorder = "Black";

        // When we create a NetworkDataView view instance, a viewmodel of this type will be created, we will call it VM-1
        // When we instantiate the AddWindow window a new viewmodel of this type will be created, we will call it VM-2
        // Obviously, VM-1 and VM-2 will be different instances, so because of that there will be 2 different properties "Roads"
        // Although there are 2 properties, there is only one ROADS LIST we are using, and that's the "RoadsObs.Instance.Road" observable collection
        // Because of that, when we instantiate the NetworkDataView again, we will have the right elements in it
        // When we add an element via AddWindow, we change the "Roads" property of VM-2, not the VM-1, and because of that 
        // NetworkDataView view will not be updated until we instantiate it again and read the data again from the deserialization
        public NetworkDataViewModel()
        {
            Roads.Clear();                                                                                          // We clear the list of the old elements
            if (serializer.DeSerializeObject<ObservableCollection<Road>>("roads.xml") != null)
            {
                serializer.DeSerializeObject<ObservableCollection<Road>>("roads.xml").ToList().ForEach(Roads.Add);  // And then we deserialize the file holding a list of Roads
            }                                                                                                       // and add each element to the "Roads" property
                                                                                                                    // This LINQ expression is the same as if we manually did the foreach loop

            FilterCollection = new CollectionViewSource();
            FilterCollection.Source = Roads;                                                                        // Sets the collection from which to create a view
            
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            AddOpenCommand = new MyICommand(OnAddOpen);
            AddCommand = new MyICommand(OnAdd);
            CancelCommand = new MyICommand(OnCancel);
            FilterCommand = new MyICommand(OnFilter);
            ClearCommand = new MyICommand(OnClear);

            NotifiedVms.Instance.Register(this);
            //ovde registrujemo ovaj VM da bude u listi u singletonu i prosledimo this, a taj this implementira neki nas interfejs
        }

        // Properties

        public Road SelectedRoad
        {
            get { return _selectedRoad; }
            set
            {
                _selectedRoad = value;
                DeleteCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedRoad");
            }
        }

        public string Id
        {
            get { return _id; }
            set
            {
                if(_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Label
        {
            get { return _label; }
            set
            {
                if(_label != value)
                {
                    _label = value;
                    OnPropertyChanged("Label");
                }
            }
        }

        public List<NetworkService.Models.Type> TpsList
        {
            get { return Types.Instance.ListOfTypes; }
        }

        public NetworkService.Models.Type Type
        {
            get { return _type; }
            set
            {
                if(_type != value)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public double Value
        {
            get { return _value; }
            set
            {
                if(_value != value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        public string FilterType
        {
            get { return _filterType; }
            set
            {
                if (_filterType != value)
                {
                    _filterType = value;
                    OnPropertyChanged("FilterType");
                }
            }
        }

        public string SearchName
        {
            get { return _searchName; }
            set
            {
                if (_searchName != value)
                {
                    _searchName = value;
                    OnPropertyChanged("SearchName");
                }
            }
        }

        public string IdWarningText
        {
            get { return _idWarningText; }
            set
            {
                if (_idWarningText != value)
                {
                    _idWarningText = value;
                    OnPropertyChanged("IdWarningText");
                }
            }
        }

        public string LabelWarningText
        {
            get { return _labelWarningText; }
            set
            {
                if (_labelWarningText != value)
                {
                    _labelWarningText = value;
                    OnPropertyChanged("LabelWarningText");
                }
            }
        }

        public string TypeWarningText
        {
            get { return _typeWarningText; }
            set
            {
                if (_typeWarningText != value)
                {
                    _typeWarningText = value;
                    OnPropertyChanged("TypeWarningText");
                }
            }
        }

        public string IdBorder
        {
            get { return _idBorder; }
            set
            {
                if (_idBorder != value)
                {
                    _idBorder = value;
                    OnPropertyChanged("IdBorder");
                }
            }
        }

        public string LabelBorder
        {
            get { return _labelBorder; }
            set
            {
                if (_labelBorder != value)
                {
                    _labelBorder = value;
                    OnPropertyChanged("LabelBorder");
                }
            }
        }

        public string TypeBorder
        {
            get { return _typeBorder; }
            set
            {
                if (_typeBorder != value)
                {
                    _typeBorder = value;
                    OnPropertyChanged("TypeBorder");
                }
            }
        }

        // Methods

        private void OnAddOpen()
        {
            /////// THIS BRAKES THE MVVM PATTERN ///////
            addWindow = new AddWindowView();
            addWindow.ShowDialog();
            ///////      BREAKS TESTABILITY      ///////
        }

        private void OnCancel()
        {
            /////// THIS BRAKES THE MVVM PATTERN ///////
            addWindow.Close();
            ///////      BREAKS TESTABILITY      ///////
        }

        private void OnAdd()
        {
            if (Validate())
            {
                Road nr = new Road(Int32.Parse(this.Id), this.Label, this.Type);

                // If we manage to delete the added road from deleted roads, that means that now we want to display data in report for the added element
                if (RoadsObs.Instance.DeletedRoads.Contains(nr))
                {
                    Roads.Add(nr);
                    RoadsObs.Instance.DeletedRoads.Remove(nr);
                    addWindow.Close();
                    serializer.SerializeObject<ObservableCollection<Road>>(Roads, "roads.xml");
                }
                else
                {
                    Roads.Add(nr);
                    addWindow.Close();
                    serializer.SerializeObject<ObservableCollection<Road>>(Roads, "roads.xml");
                }
            }
        }

        private void OnClear()
        {
            FilterCollection.Source = Roads;
            SearchName = "";
            FilterType = null;
        }

        private bool CanDelete()
        {
            return SelectedRoad != null;
        }

        private void OnDelete()
        {
            // We first add selected road to the deleted roads because when we do Remove() on "Roads", the next selected road will be the one after previous selected one
            RoadsObs.Instance.DeletedRoads.Add(SelectedRoad);
            Roads.Remove(SelectedRoad);
            serializer.SerializeObject<ObservableCollection<Road>>(Roads, "roads.xml");
        }

        private void OnFilter()
        {
            if ((FilterType == "IA" || FilterType == "IB") && !string.IsNullOrEmpty(SearchName))
            {
                if (FilterType == "IA")
                {
                    List<Road> roads = new List<Road>();
                    foreach (Road road in Roads)
                    {
                        if (road.Type.NAME == FilterType && road.Label.Contains(SearchName))
                        {
                            roads.Add(road);
                        }
                    }
                    FilterCollection.Source = roads;
                }
                else
                {
                    List<Road> roads = new List<Road>();
                    foreach (Road road in Roads)
                    {
                        if (road.Type.NAME == FilterType && road.Label.Contains(SearchName))
                        {
                            roads.Add(road);
                        }
                    }
                    FilterCollection.Source = roads;
                }
            }
            else if ((FilterType == "IA" || FilterType == "IB") && string.IsNullOrEmpty(SearchName))
            {
                if (FilterType == "IA")
                {
                    List<Road> roads = new List<Road>();
                    foreach (Road road in Roads)
                    {
                        if (road.Type.NAME == FilterType)
                        {
                            roads.Add(road);
                        }
                    }
                    FilterCollection.Source = roads;
                }
                else
                {
                    List<Road> roads = new List<Road>();
                    foreach (Road road in Roads)
                    {
                        if (road.Type.NAME == FilterType)
                        {
                            roads.Add(road);
                        }
                    }
                    FilterCollection.Source = roads;
                }
            }
            else if (FilterType != "IA" && FilterType != "IB")
            {
                if (!string.IsNullOrEmpty(SearchName))
                {
                    List<Road> roads = new List<Road>();
                    foreach (Road road in Roads)
                    {
                        if (road.Label.Contains(SearchName))
                        {
                            roads.Add(road);
                        }
                    }
                    FilterCollection.Source = roads;
                }
                else
                {
                    FilterCollection.Source = Roads;
                }
            }
            else
            {
                FilterCollection.Source = Roads;
            }
        }

        private bool Validate()
        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(Id))
            {
                result = false;
                IdBorder = "Red";
                IdWarningText = "This field cannot be empty!";
            }
            else if (!Int32.TryParse(Id, out int n))
            {
                result = false;
                IdBorder = "Red";
                IdWarningText = "Only an integer number is allowed for ID!";
            }
            else if (IsEqualRoad(Int32.Parse(Id)))
            {
                result = false;
                IdBorder = "Red";
                IdWarningText = "Road with this ID already exists!";
            }
            else
            {
                IdBorder = "Black";
                IdWarningText = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(Label))
            {
                result = false;
                LabelBorder = "Red";
                LabelWarningText = "This field cannot be empty!";
            }
            else if (Label.Length > 20)
            {
                result = false;
                LabelBorder = "Red";
                LabelWarningText = "Label has to be a maximum of 20 characters long!";
            }
            else
            {
                LabelBorder = "Black";
                LabelWarningText = string.Empty;
            }
            if (Type == null)
            {
                result = false;
                TypeBorder = "Red";
                TypeWarningText = "You have to choose an option!";
            }
            else if (string.IsNullOrWhiteSpace(Type.NAME))
            {
                result = false;
                TypeBorder = "Red";
                TypeWarningText = "You have to choose an option!";
            }
            else if (Type.NAME != "IA" && Type.NAME != "IB")
            {
                result = false;
                TypeBorder = "Red";
                TypeWarningText = "You have to choose an option!";
            }
            else
            {
                TypeBorder = "Black";
                TypeWarningText = string.Empty;
            }
            return result;
        }

        private bool IsEqualRoad(int id)
        {
            bool result = false;

            foreach (var road in Roads)
            {
                if (road.Id == id)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void Notify(Road changedRoad)
        {
            foreach (Road r in Roads)
            {
                if (r.Equals(changedRoad))
                {
                    r.Value = changedRoad.Value;
                }
            }
        }
    }
}