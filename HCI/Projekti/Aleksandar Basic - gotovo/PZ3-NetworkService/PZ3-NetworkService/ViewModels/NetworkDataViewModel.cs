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
		private int filterId;
		private ReactorType selectedFilterBy;
		private bool filterLessThan;
		private bool filterGreaterOrEqual;
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
		public int FilterId
		{
			get { return filterId; }
			set
			{
				SetField(ref filterId, value);
			}
		}
		public ReactorType SelectedFilterBy
		{
			get { return selectedFilterBy; }
			set
			{
				SetField(ref selectedFilterBy, value);
			}
		}
		public bool FilterLessThan
		{
			get { return filterLessThan; }
			set
			{
				SetField(ref filterLessThan, value);
			}
		}
		public bool FilterGreaterOrEqual
		{
			get { return filterGreaterOrEqual; }
			set
			{
				SetField(ref filterGreaterOrEqual, value);
			}
		}
		public ObservableCollection<ReactorType> FilterByList { get; set; }
		#endregion

		public MyICommand AddReactorCommand { get; set; }
		public MyICommand DeleteReactorCommand { get; set; }
		public MyICommand FilterCommand { get; set; }
		public MyICommand ClearFilterCommand { get; set; }

		public NetworkDataViewModel()
		{
			Container.FilterCollection = new CollectionViewSource();
			Container.FilterCollection.Source = Container.Reactors;
			FilterByList = new ObservableCollection<ReactorType>(Container.ReactorTypes.ToList());
			SelectedFilterBy = FilterByList[0];
			AddReactorCommand = new MyICommand(OnAddReactor);
			DeleteReactorCommand = new MyICommand(OnDeleteReactor, CanDeleteReactor);
			FilterCommand = new MyICommand(OnFilter);
			ClearFilterCommand = new MyICommand(OnClearFilter);
			FilterLessThan = true;
			FilterGreaterOrEqual = false;
			ResetErrorMessages();
		}

		#region Command methods
		private void OnClearFilter()
		{
			ClearFilterCommand clearFilterCommand = new ClearFilterCommand();
			Container.CommandInvoker.AddAndExecute(clearFilterCommand);
			FilterId = 0;
			SelectedFilterBy = FilterByList[0];
			FilterLessThan = true;
			FilterGreaterOrEqual = false;
		}
		private void OnFilter()
		{
			FilterCommand filterCommand = new FilterCommand(SelectedFilterBy, FilterLessThan, FilterId);
			Container.CommandInvoker.AddAndExecute(filterCommand);
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
