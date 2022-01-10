using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PZ3_NetworkService.Model;

namespace PZ3_NetworkService.ViewModel
{
    public class NetworkDataViewModel : BindableBase
    {
        public static ObservableCollection<Valve> Valves { get; set; }
        public static ObservableCollection<Valve> FilteredValves { get; set; }

        public static ObservableCollection<ValveType> VTypes { get; set; }

        public ObservableCollection<Valve> networkDataValves;
        public ObservableCollection<Valve> NetworkDataValves
        {
            get { return networkDataValves; }

            set
            {

                if (networkDataValves != value)
                {
                    networkDataValves = value;
                    OnPropertyChanged("NetworkDataValves");
                }
            }

        }

        private Valve selectedValve = new Valve();

        public MyICommand DeleteCommand { get; set; }
        public MyICommand AddCommand { get; set; }
        public MyICommand FilterCommand { get; set; }
        public MyICommand ResetCommand { get; set; }

        private string idText;
        private string nameText;

        private int filterId;

        private bool radiobuttonGas;
        private bool radiobuttonWater;
		private bool radiobuttonLessThan;
		private bool radiobuttonGreaterOrEqual;


        public NetworkDataViewModel()
        {

            Valves = GlobalValves.AllObjects;

            FilteredValves = new ObservableCollection<Valve>();
            NetworkDataValves = new ObservableCollection<Valve>();
         
            NetworkDataValves = Valves;

         

            ObservableCollection<ValveType> mTypes =
                new ObservableCollection<ValveType>();

            mTypes.Add(GlobalValves.ValveTypes.Values.ToList()[0]);
            mTypes.Add(GlobalValves.ValveTypes.Values.ToList()[1]);

            VTypes = mTypes;

                    
            AddCommand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            FilterCommand = new MyICommand(OnFilter);
            ResetCommand = new MyICommand(OnReset);

            RadiobuttonWater = false;
            RadiobuttonGas = true;
			RadiobuttonLessThan = true;
			RadiobuttonGreaterOrEqual = false;

        }

        public bool RadiobuttonGas
        {
            get { return radiobuttonGas; }
            set
            {
                if (radiobuttonGas != value)
                {
                    radiobuttonGas = value;
                    OnPropertyChanged("RadiobuttonGas");
                }
            }
        }
        public bool RadiobuttonWater
        {
            get { return radiobuttonWater; }
            set
            {
                if (radiobuttonWater != value)
                {
                    radiobuttonWater = value;
                    OnPropertyChanged("RadiobuttonWater");
                }
            }
        }
		public bool RadiobuttonLessThan
		{
			get
			{
				return radiobuttonLessThan;
			}
			set
			{
				if(radiobuttonLessThan != value)
				{
					radiobuttonLessThan = value;
					OnPropertyChanged("RadiobuttonLessThan");
				}
			}
		}
		public bool RadiobuttonGreaterOrEqual
		{
			get
			{
				return radiobuttonGreaterOrEqual;
			}
			set
			{
				if(radiobuttonGreaterOrEqual != value)
				{
					radiobuttonGreaterOrEqual = value;
					OnPropertyChanged("RadiobuttonGreaterOrEqual");
				}
			}
		}
        public string IDText
        {
            get { return idText; }
            set
            {
                if (idText != value)
                {
                    idText = value;
                    OnPropertyChanged("IDText");
                }
            }
        }
        public string NameText
        {
            get { return nameText; }
            set
            {
                if (nameText != value)
                {
                    nameText = value;
                    OnPropertyChanged("NameText");
                }
            }
        }
        public int FilterId
        {
            get { return filterId; }
            set
            {
                if (filterId != value)
                {
                    filterId = value;
                    OnPropertyChanged("FilterId");
                }
            }
        }
        private ValveType selectedVType;
        public ValveType SelectedVType
        {
            get { return selectedVType; }

            set
            {
                if (selectedVType != value)
                {
                    selectedVType = value;
                    OnPropertyChanged("SelectedVType");
                }
            }
        }
		public Valve SelectedValve
		{
			get { return selectedValve; }
			set
			{
				selectedValve = value;
				DeleteCommand.RaiseCanExecuteChanged();
			}
		}

		private void OnAdd()
        {
            bool doit = true;
            int nj;

            if (Int32.TryParse(IDText, out nj) == false)
            {
                doit = false;
                MessageBox.Show("Please enter valid ID for your valve. ID must be integer!", "Invalid ID Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
				if(nj <= 0)
				{
					MessageBox.Show("Id must be greater than 0.", "Invalid ID Error!.", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				else
				{
					foreach (var a in Valves)
					{
						if (a.ID == nj)
						{
							doit = false;
							MessageBox.Show("Valve with selected ID alredy exists in database!", "Meter alredy exists.", MessageBoxButton.OK, MessageBoxImage.Error);
						}
					}
				}
            }

            if (NameText == null || NameText == "")
            {
                doit = false;
                MessageBox.Show("Enter a name for your valve!", "No Name Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (SelectedVType == null)
            {
                doit = false;
                MessageBox.Show("You must select valid type!", "Invalid Type Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (doit == true)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to add this object to the database?", "Confirm adding.",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

                    GlobalValves.AllObjects.Add(new Valve { ID = nj, Name = NameText, ValveType = SelectedVType, Val = 0});
					OnFilter();
                }

            }
        }        
        private bool CanDelete()
        {
            return SelectedValve != null;
        }
        private void OnDelete()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to remove selected object from the database?", "Confirm deletion.",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                foreach (var item in GlobalValves.DDbase.Values)
                {
                    if (item.ID == SelectedValve.ID)
                    {
                        MessageBox.Show("Unable to remove valve that is being monitored.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }


                int id = SelectedValve.ID;
                FilteredValves.Remove(SelectedValve); 
                foreach (Valve r in GlobalValves.AllObjects)
                if (r.ID == id)
                {
                       
                    GlobalValves.AllObjects.Remove(r);
                    break;
                }
            }
        }
        private void OnReset()
        {
            NetworkDataValves = Valves;
			RadiobuttonWater = false;
			RadiobuttonGas = true;
			RadiobuttonLessThan = true;
			RadiobuttonGreaterOrEqual = false;
		}
        private void OnFilter()
        {
            if (FilterId <= 0)
            {
				MessageBox.Show("Id values must be greater than 0.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
            }

			FilteredValves.Clear();
			if (RadiobuttonGas)
			{
				if (RadiobuttonLessThan)
				{
					foreach(var v in Valves)
					{
						if(v.ValveType.Name == "Gas" && v.ID < FilterId)
						{
							FilteredValves.Add(v);
						}
					}
				}
				else
				{
					foreach (var v in Valves)
					{
						if (v.ValveType.Name == "Water" && v.ID < FilterId)
						{
							FilteredValves.Add(v);
						}
					}
				}
			}
			else
			{
				if (RadiobuttonLessThan)
				{
					foreach (var v in Valves)
					{
						if (v.ValveType.Name == "Gas" && v.ID >= FilterId)
						{
							FilteredValves.Add(v);
						}
					}
				}
				else
				{
					foreach (var v in Valves)
					{
						if (v.ValveType.Name == "Water" && v.ID >= FilterId)
						{
							FilteredValves.Add(v);
						}
					}
				}
			}

            NetworkDataValves = FilteredValves;
        }
    }
}

