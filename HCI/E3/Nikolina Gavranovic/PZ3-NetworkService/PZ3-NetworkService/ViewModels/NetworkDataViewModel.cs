using PZ3_NetworkService.Commands;
using PZ3_NetworkService;
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
		private Road selectedRoad;
		private int? newRoadId;
		private string newRoadName;
		private RoadType newRoadRoadType;
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
		public Road SelectedRoad
		{
			get { return selectedRoad; }
			set
			{
				SetField(ref selectedRoad, value);
				DeleteRoadCommand.RaiseCanExecuteChanged();
			}
		}
		public int? NewRoadId
		{
			get { return newRoadId; }
			set
			{
				SetField(ref newRoadId, value);
			}
		}
		public string NewRoadName
		{
			get { return newRoadName; }
			set
			{
				SetField(ref newRoadName, value);
			}
		}
		public RoadType NewRoadRoadType
		{
			get { return newRoadRoadType; }
			set
			{
				SetField(ref newRoadRoadType, value);
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

		public MyICommand AddRoadCommand { get; set; }
		public MyICommand DeleteRoadCommand { get; set; }

		public NetworkDataViewModel()
		{
			Container.SearchCollection = new CollectionViewSource();
			Container.SearchCollection.Source = Container.Roads;
			SearchByList = new ObservableCollection<string>
			{
				"Name",
				"RoadType"
			};
			SelectedSearchBy = SearchByList[0];
			AddRoadCommand = new MyICommand(OnAddRoad);
			DeleteRoadCommand = new MyICommand(OnDeleteRoad, CanDeleteRoad);
			ResetErrorMessages();
		}

		#region Command methods
		//private void ClearSearch()
		//{
		//	Container.SearchCollection.Source = Container.Roads;
		//	SearchText = null;
		//	SelectedSearchBy = null;
		//}
		private void Search()
		{
			if (SelectedSearchBy == "Name")
			{
				List<Road> roads = new List<Road>();
				if (String.IsNullOrWhiteSpace(SearchText))
				{
					roads = Container.Roads.ToList();
				}
				else
				{
					foreach (var v in Container.Roads)
					{
						if (v.Name.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) != -1)
						{
							roads.Add(v);
						}
					}
				}
				Container.SearchCollection.Source = roads;
			}
			else if(SelectedSearchBy == "RoadType")
			{
				List<Road> roads = new List<Road>();
				if (String.IsNullOrWhiteSpace(SearchText))
				{
					roads = Container.Roads.ToList();
				}
				else
				{
					foreach (var v in Container.Roads)
					{
						if (v.RoadType.ToString().IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) != -1)
						{
							roads.Add(v);
						}
					}
				}
				Container.SearchCollection.Source = roads;
			}
			else
			{
				Container.SearchCollection.Source = Container.Roads;
			}
		}
		private bool CanDeleteRoad()
		{
			return SelectedRoad != null;
		}
		private void OnDeleteRoad()
		{
			DeleteRoadCommand deleteCommand = new DeleteRoadCommand(SelectedRoad);
			Container.CommandInvoker.AddAndExecute(deleteCommand);
		}
		private bool CanAddRoad()
		{
			// Before each check, reset error message
			ResetErrorMessages();

			bool roadIdAlreadyExists = false;

			// Id check
			if (NewRoadId == null)
			{
				AddIdErrorMessage = "Id cannot be empty";
			}
			else
			{
				foreach (var road in Container.Roads)
				{
					if (road.Id == NewRoadId)
					{
						roadIdAlreadyExists = true;
						AddIdErrorMessage = $"Road with ID = {newRoadId} already exists.";
						break;
					}
				}
			}

			// Name check
			if (String.IsNullOrWhiteSpace(NewRoadName))
			{
				AddNameErrorMessage = "Name cannot be empty.";
			}

			return !(NewRoadId == null || NewRoadId < 0 || roadIdAlreadyExists || String.IsNullOrWhiteSpace(NewRoadName));
		}
		private void OnAddRoad()
		{
			if (CanAddRoad())
			{
				Road newRoad = new Road(
				NewRoadId == null ? default(int) : NewRoadId.Value,
				NewRoadName,
				NewRoadRoadType,
				Container.RoadTypesAndImagesMapper[NewRoadRoadType]);

				AddRoadCommand addCommand = new AddRoadCommand(newRoad);
				Container.CommandInvoker.AddAndExecute(addCommand);
				ResetForm();
			}
		}
		#endregion

		private void ResetForm()
		{
			NewRoadId = null;
			NewRoadName = "";
			ResetErrorMessages();
		}
		private void ResetErrorMessages()
		{
			AddIdErrorMessage = "";
			AddNameErrorMessage = "";
		}
	}
}
