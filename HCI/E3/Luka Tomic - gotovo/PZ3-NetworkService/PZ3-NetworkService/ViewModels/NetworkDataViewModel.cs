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
		private WaterMeter selectedWaterMeter;
		private int? newWaterMeterId;
		private string newWaterMeterName;
		private WaterMeterType newWaterMeterWaterMeterType;
		private int filterId;
		private WaterMeterType selectedFilterBy;
		private FilterConditionType selectedFilterConditionType;
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
		public WaterMeter SelectedWaterMeter
		{
			get { return selectedWaterMeter; }
			set
			{
				SetField(ref selectedWaterMeter, value);
				DeleteWaterMeterCommand.RaiseCanExecuteChanged();
			}
		}
		public int? NewWaterMeterId
		{
			get { return newWaterMeterId; }
			set
			{
				SetField(ref newWaterMeterId, value);
			}
		}
		public string NewWaterMeterName
		{
			get { return newWaterMeterName; }
			set
			{
				SetField(ref newWaterMeterName, value);
			}
		}
		public WaterMeterType NewWaterMeterWaterMeterType
		{
			get { return newWaterMeterWaterMeterType; }
			set
			{
				SetField(ref newWaterMeterWaterMeterType, value);
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
		public WaterMeterType SelectedFilterBy
		{
			get { return selectedFilterBy; }
			set
			{
				SetField(ref selectedFilterBy, value);
			}
		}
		public FilterConditionType SelectedFilterConditionType
		{
			get { return selectedFilterConditionType; }
			set
			{
				SetField(ref selectedFilterConditionType, value);
			}
		}
		public ObservableCollection<WaterMeterType> FilterByList { get; set; }
		#endregion

		public MyICommand AddWaterMeterCommand { get; set; }
		public MyICommand DeleteWaterMeterCommand { get; set; }
		public MyICommand FilterCommand { get; set; }
		public MyICommand ClearFilterCommand { get; set; }

		public NetworkDataViewModel()
		{
			Everything.FilterCollection = new CollectionViewSource();
			Everything.FilterCollection.Source = Everything.WaterMeters;
			FilterByList = new ObservableCollection<WaterMeterType>(Everything.WaterMeterTypes.ToList());
			SelectedFilterBy = FilterByList[0];
			AddWaterMeterCommand = new MyICommand(OnAddWaterMeter);
			DeleteWaterMeterCommand = new MyICommand(OnDeleteWaterMeter, CanDeleteWaterMeter);
			FilterCommand = new MyICommand(OnFilter);
			ClearFilterCommand = new MyICommand(OnClearFilter);
			ResetErrorMessages();
		}

		#region Command methods
		private void OnClearFilter()
		{
			ClearFilterCommand clearFilterCommand = new ClearFilterCommand();
			CommandExecuter.ExecuteAndAdd(clearFilterCommand);
			FilterId = 0;
			SelectedFilterBy = FilterByList[0];
		}
		private void OnFilter()
		{
			FilterCommand filterCommand = new FilterCommand(SelectedFilterBy, SelectedFilterConditionType, FilterId);
			CommandExecuter.ExecuteAndAdd(filterCommand);
		}
		private bool CanDeleteWaterMeter()
		{
			return SelectedWaterMeter != null;
		}
		private void OnDeleteWaterMeter()
		{
			DeleteWaterMeterCommand deleteCommand = new DeleteWaterMeterCommand(SelectedWaterMeter);
			CommandExecuter.ExecuteAndAdd(deleteCommand);
		}
		private bool CanAddWaterMeter()
		{
			// Before each check, reset error message
			ResetErrorMessages();

			bool waterMeterIdAlreadyExists = false;

			// Id check
			if (NewWaterMeterId == null)
			{
				AddIdErrorMessage = "Id cannot be empty";
			}
			else
			{
				foreach (var waterMeter in Everything.WaterMeters)
				{
					if (waterMeter.Id == NewWaterMeterId)
					{
						waterMeterIdAlreadyExists = true;
						AddIdErrorMessage = $"WaterMeter with ID = {newWaterMeterId} already exists.";
						break;
					}
				}
			}

			// Name check
			if (String.IsNullOrWhiteSpace(NewWaterMeterName))
			{
				AddNameErrorMessage = "Name cannot be empty.";
			}

			return !(NewWaterMeterId == null || NewWaterMeterId < 0 || waterMeterIdAlreadyExists || String.IsNullOrWhiteSpace(NewWaterMeterName));
		}
		private void OnAddWaterMeter()
		{
			if (CanAddWaterMeter())
			{
				WaterMeter newWaterMeter = new WaterMeter(
				NewWaterMeterId == null ? default(int) : NewWaterMeterId.Value,
				NewWaterMeterName,
				NewWaterMeterWaterMeterType,
				Everything.WaterMeterTypesAndImagesMapper[NewWaterMeterWaterMeterType]);

				AddWaterMeterCommand addCommand = new AddWaterMeterCommand(newWaterMeter);
				CommandExecuter.ExecuteAndAdd(addCommand);
				ResetForm();
			}
		}
		#endregion

		private void ResetForm()
		{
			NewWaterMeterId = null;
			NewWaterMeterName = "";
			ResetErrorMessages();
		}
		private void ResetErrorMessages()
		{
			AddIdErrorMessage = "";
			AddNameErrorMessage = "";
		}
	}
}
