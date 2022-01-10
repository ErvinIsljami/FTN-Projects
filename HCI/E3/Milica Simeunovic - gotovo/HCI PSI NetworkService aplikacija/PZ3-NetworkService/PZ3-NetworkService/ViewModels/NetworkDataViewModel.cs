using PZ3_NetworkService.Commands;
using PZ3_NetworkService.Containers;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PZ3_NetworkService.ViewModels
{
	public class NetworkDataViewModel : BindableBase
	{
		#region Fields
		private string addIdErrorMessage;
		private string addNameErrorMessage;
		private Valve selectedValve;
		private int? newValveId;
		private string newValveName;
		private ValveType newValveValveType;
		private string searchText;
		private string selectedSearchBy;
		#endregion

		#region Properties
		public string AddIdErrorMessage
		{
			get { return addIdErrorMessage; }
			set
			{
				SetField(ref addIdErrorMessage, value);
			}
		}
		public string AddNameErrorMessage
		{
			get { return addNameErrorMessage; }
			set
			{
				SetField(ref addNameErrorMessage, value);
			}
		}
		public Valve SelectedValve
		{
			get { return selectedValve; }
			set
			{
				SetField(ref selectedValve, value);
				DeleteValveCommand.RaiseCanExecuteChanged();
			}
		}
		public int? NewValveId
		{
			get { return newValveId; }
			set
			{
				SetField(ref newValveId, value);
			}
		}
		public string NewValveName
		{
			get { return newValveName; }
			set
			{
				SetField(ref newValveName, value);
			}
		}
		public ValveType NewValveValveType
		{
			get { return newValveValveType; }
			set
			{
				SetField(ref newValveValveType, value);
			}
		}
		public string SearchText
		{
			get { return searchText; }
			set
			{
				SetField(ref searchText, value);
				Search();
			}
		}
		public string SelectedSearchBy
		{
			get { return selectedSearchBy; }
			set
			{
				SetField(ref selectedSearchBy, value);
				Search();
			}
		}
		public ObservableCollection<string> SearchByList { get; set; }
		#endregion

		public MyICommand AddValveCommand { get; set; }
		public MyICommand DeleteValveCommand { get; set; }

		public NetworkDataViewModel()
		{
			Container.SearchCollection = new CollectionViewSource();
			Container.SearchCollection.Source = Container.Valves;
			SearchByList = new ObservableCollection<string>
			{
				"Name",
				"ValveType"
			};
			SelectedSearchBy = SearchByList[0];
			AddValveCommand = new MyICommand(OnAddValve);
			DeleteValveCommand = new MyICommand(OnDeleteValve, CanDeleteValve);
			ResetErrorMessages();
		}

		#region Command methods
		private void ClearSearch()
		{
			Container.SearchCollection.Source = Container.Valves;
			SearchText = null;
			SelectedSearchBy = null;
		}
		private void Search()
		{
			if (SelectedSearchBy == "Name")
			{
				List<Valve> valves = new List<Valve>();
				if (String.IsNullOrWhiteSpace(SearchText))
				{
					valves = Container.Valves.ToList();
				}
				else
				{
					foreach (var v in Container.Valves)
					{
						if (v.Name.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) != -1)
						{
							valves.Add(v);
						}
					}
				}
				Container.SearchCollection.Source = valves;
			}
			else if(SelectedSearchBy == "ValveType")
			{
				List<Valve> valves = new List<Valve>();
				if (String.IsNullOrWhiteSpace(SearchText))
				{
					valves = Container.Valves.ToList();
				}
				else
				{
					foreach (var v in Container.Valves)
					{
						if (v.ValveType.ToString().IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) != -1)
						{
							valves.Add(v);
						}
					}
				}
				Container.SearchCollection.Source = valves;
			}
			else
			{
				Container.SearchCollection.Source = Container.Valves;
			}
		}
		private bool CanDeleteValve()
		{
			return SelectedValve != null;
		}
		private void OnDeleteValve()
		{
			DeleteValveCommand deleteCommand = new DeleteValveCommand(SelectedValve);
			Container.CommandInvoker.AddAndExecute(deleteCommand);
			//ResetForm();
		}
		private bool CanAddValve()
		{
			// Before each check, reset error message
			ResetErrorMessages();

			bool valveIdAlreadyExists = false;

			// Id check
			if (NewValveId == null)
			{
				AddIdErrorMessage = "Id cannot be empty";
			}
			else
			{
				foreach (var valve in Container.Valves)
				{
					if (valve.Id == NewValveId)
					{
						valveIdAlreadyExists = true;
						AddIdErrorMessage = $"Valve with ID = {newValveId} already exists.";
						break;
					}
				}
			}

			// Name check
			if (String.IsNullOrWhiteSpace(NewValveName))
			{
				AddNameErrorMessage = "Name cannot be empty.";
			}

			return !(NewValveId == null || NewValveId < 0 || valveIdAlreadyExists || String.IsNullOrWhiteSpace(NewValveName));
		}
		private void OnAddValve()
		{
			if (CanAddValve())
			{
				Valve newValve = new Valve(
				NewValveId == null ? default(int) : NewValveId.Value,
				NewValveValveType,
				NewValveName,
				Container.ValveTypesAndImagesMapper[NewValveValveType]);

				AddValveCommand addCommand = new AddValveCommand(newValve);
				Container.CommandInvoker.AddAndExecute(addCommand);
				ResetForm();
			}
		}
		#endregion

		private void ResetForm()
		{
			NewValveId = null;
			NewValveName = "";
			ResetErrorMessages();
		}
		private void ResetErrorMessages()
		{
			AddIdErrorMessage = "";
			AddNameErrorMessage = "";
		}
	}
}
