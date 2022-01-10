using PZ3_NetworkService.Commands;
using PZ3_NetworkService;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PZ3_NetworkService.ViewModels
{
	public class NetworkViewViewModel : BindableBase
	{
		private WaterMeter _selectedWaterMeter;
		private DataObject currentlyDraggedData;

		public WaterMeter SelectedWaterMeter
		{
			get { return _selectedWaterMeter; }
			set
			{
				SetField(ref _selectedWaterMeter, value);
			}
		}
		public MyICommand<string> ClearDataGridCommand { get; set; }
		public MyICommand<object> DropCommand { get; set; }
		public MyICommand<object> ListViewSelectionChangedCommand { get; set; }
		#region DataGrids
		public ObservableCollection<WaterMeter> DataGrid0 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid1 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid2 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid3 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid4 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid5 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid6 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid7 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid8 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid9 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid10 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid11 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid12 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid13 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid14 { get; set; }
		public ObservableCollection<WaterMeter> DataGrid15 { get; set; }
		#endregion

		public NetworkViewViewModel()
		{
			ClearDataGridCommand = new MyICommand<string>(OnClearDataGrid);
			DropCommand = new MyICommand<object>(OnDrop);
			ListViewSelectionChangedCommand = new MyICommand<object>(OnListViewSelectionChanged);
			DataGrid0 = new ObservableCollection<WaterMeter>();
			DataGrid1 = new ObservableCollection<WaterMeter>();
			DataGrid2 = new ObservableCollection<WaterMeter>();
			DataGrid3 = new ObservableCollection<WaterMeter>();
			DataGrid4 = new ObservableCollection<WaterMeter>();
			DataGrid5 = new ObservableCollection<WaterMeter>();
			DataGrid6 = new ObservableCollection<WaterMeter>();
			DataGrid7 = new ObservableCollection<WaterMeter>();
			DataGrid8 = new ObservableCollection<WaterMeter>();
			DataGrid9 = new ObservableCollection<WaterMeter>();
			DataGrid10 = new ObservableCollection<WaterMeter>();
			DataGrid11 = new ObservableCollection<WaterMeter>();
			DataGrid12 = new ObservableCollection<WaterMeter>();
			DataGrid13 = new ObservableCollection<WaterMeter>();
			DataGrid14 = new ObservableCollection<WaterMeter>();
			DataGrid15 = new ObservableCollection<WaterMeter>();
			Everything.DataGridMap.Add("0", DataGrid0);
			Everything.DataGridMap.Add("1", DataGrid1);
			Everything.DataGridMap.Add("2", DataGrid2);
			Everything.DataGridMap.Add("3", DataGrid3);
			Everything.DataGridMap.Add("4", DataGrid4);
			Everything.DataGridMap.Add("5", DataGrid5);
			Everything.DataGridMap.Add("6", DataGrid6);
			Everything.DataGridMap.Add("7", DataGrid7);
			Everything.DataGridMap.Add("8", DataGrid8);
			Everything.DataGridMap.Add("9", DataGrid9);
			Everything.DataGridMap.Add("10", DataGrid10);
			Everything.DataGridMap.Add("11", DataGrid11);
			Everything.DataGridMap.Add("12", DataGrid12);
			Everything.DataGridMap.Add("13", DataGrid13);
			Everything.DataGridMap.Add("14", DataGrid14);
			Everything.DataGridMap.Add("15", DataGrid15);
		}

		// Check if list view item is already dragged somewhere
		private bool CheckIfItemAlreadyDragged()
		{			
			bool isDragged = false;

			foreach (var dgPair in Everything.DataGridMap)
			{
				WaterMeter draggedWaterMeter = dgPair.Value.FirstOrDefault(v => v.Id == SelectedWaterMeter.Id);
				if (draggedWaterMeter != null)
				{
					isDragged = true;
					break;
				}
			}

			return isDragged;
		}

		// Start drag and drop
		private void OnListViewSelectionChanged(object source)
		{
			if(SelectedWaterMeter != null)
			{
				if (!CheckIfItemAlreadyDragged())
				{
					currentlyDraggedData = new DataObject(typeof(WaterMeter), SelectedWaterMeter);
					DragDrop.DoDragDrop(source as ListView, currentlyDraggedData, DragDropEffects.Copy);
				}
			}
		}

		// Handle drop
		// target will be the DataGrid on which an item was dropped on
		private void OnDrop(object target)
		{
			DataGrid targetDatagrid = target as DataGrid;
			string targetDataGridId = targetDatagrid.Name.Split('_')[1];

			WaterMeter droppedWaterMeter = currentlyDraggedData.GetData(typeof(WaterMeter)) as WaterMeter;

			DragWaterMeterCommand dragWaterMeterCommand = new DragWaterMeterCommand(droppedWaterMeter, targetDataGridId);
			CommandExecuter.ExecuteAndAdd(dragWaterMeterCommand);
		}

		private void OnClearDataGrid(string targetDataGridId)
		{
			if(Everything.DataGridMap[targetDataGridId].Count > 0)
			{
				ClearGridCommand clearGridCommand = new ClearGridCommand(Everything.DataGridMap[targetDataGridId][0], targetDataGridId);
				CommandExecuter.ExecuteAndAdd(clearGridCommand);
			}
		}
	}
}