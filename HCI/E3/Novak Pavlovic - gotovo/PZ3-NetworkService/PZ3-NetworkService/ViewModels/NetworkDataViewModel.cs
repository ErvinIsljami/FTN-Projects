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
		private Reactor selectedReactor;
		private int? newReactorId;
		private string newReactorName;
		private ReactorType newReactorReactorType;
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
		public Reactor SelectedReactor
		{
			get { return selectedReactor; }
			set
			{
				SetField(ref selectedReactor, value);
				DeleteReactorCommand.RaiseCanExecuteChanged();
			}
		}
		public int? NewReactorId
		{
			get { return newReactorId; }
			set
			{
				SetField(ref newReactorId, value);
			}
		}
		public string NewReactorName
		{
			get { return newReactorName; }
			set
			{
				SetField(ref newReactorName, value);
			}
		}
		public ReactorType NewReactorReactorType
		{
			get { return newReactorReactorType; }
			set
			{
				SetField(ref newReactorReactorType, value);
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

		public MyICommand AddReactorCommand { get; set; }
		public MyICommand DeleteReactorCommand { get; set; }

		public NetworkDataViewModel()
		{
			Container.SearchCollection = new CollectionViewSource();
			Container.SearchCollection.Source = Container.Reactors;
			SearchByList = new ObservableCollection<string>
			{
				"Name",
				"ReactorType"
			};
			SelectedSearchBy = SearchByList[0];
			AddReactorCommand = new MyICommand(OnAddReactor);
			DeleteReactorCommand = new MyICommand(OnDeleteReactor, CanDeleteReactor);
			ResetErrorMessages();
		}

		#region Command methods
		//private void ClearSearch()
		//{
		//	Container.SearchCollection.Source = Container.Reactors;
		//	SearchText = null;
		//	SelectedSearchBy = null;
		//}
		private void Search()
		{
			if (SelectedSearchBy == "Name")
			{
				List<Reactor> reactors = new List<Reactor>();
				if (String.IsNullOrWhiteSpace(SearchText))
				{
					reactors = Container.Reactors.ToList();
				}
				else
				{
					foreach (var v in Container.Reactors)
					{
						if (v.Name.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) != -1)
						{
							reactors.Add(v);
						}
					}
				}
				Container.SearchCollection.Source = reactors;
			}
			else if(SelectedSearchBy == "ReactorType")
			{
				List<Reactor> reactors = new List<Reactor>();
				if (String.IsNullOrWhiteSpace(SearchText))
				{
					reactors = Container.Reactors.ToList();
				}
				else
				{
					foreach (var v in Container.Reactors)
					{
						if (v.ReactorType.ToString().IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) != -1)
						{
							reactors.Add(v);
						}
					}
				}
				Container.SearchCollection.Source = reactors;
			}
			else
			{
				Container.SearchCollection.Source = Container.Reactors;
			}
		}
		private bool CanDeleteReactor()
		{
			return SelectedReactor != null;
		}
		private void OnDeleteReactor()
		{
			DeleteReactorCommand deleteCommand = new DeleteReactorCommand(SelectedReactor);
			Container.CommandInvoker.AddAndExecute(deleteCommand);
		}
		private bool CanAddReactor()
		{
			// Before each check, reset error message
			ResetErrorMessages();

			bool reactorIdAlreadyExists = false;

			// Id check
			if (NewReactorId == null)
			{
				AddIdErrorMessage = "Id cannot be empty";
			}
			else
			{
				foreach (var reactor in Container.Reactors)
				{
					if (reactor.Id == NewReactorId)
					{
						reactorIdAlreadyExists = true;
						AddIdErrorMessage = $"Reactor with ID = {newReactorId} already exists.";
						break;
					}
				}
			}

			// Name check
			if (String.IsNullOrWhiteSpace(NewReactorName))
			{
				AddNameErrorMessage = "Name cannot be empty.";
			}

			return !(NewReactorId == null || NewReactorId < 0 || reactorIdAlreadyExists || String.IsNullOrWhiteSpace(NewReactorName));
		}
		private void OnAddReactor()
		{
			if (CanAddReactor())
			{
				Reactor newReactor = new Reactor(
				NewReactorId == null ? default(int) : NewReactorId.Value,
				NewReactorName,
				NewReactorReactorType,
				Container.ReactorTypesAndImagesMapper[NewReactorReactorType]);

				AddReactorCommand addCommand = new AddReactorCommand(newReactor);
				Container.CommandInvoker.AddAndExecute(addCommand);
				ResetForm();
			}
		}
		#endregion

		private void ResetForm()
		{
			NewReactorId = null;
			NewReactorName = "";
			ResetErrorMessages();
		}
		private void ResetErrorMessages()
		{
			AddIdErrorMessage = "";
			AddNameErrorMessage = "";
		}
	}
}
