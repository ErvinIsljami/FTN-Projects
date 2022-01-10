using PZ3_NetworkService.Commands;
using PZ3_NetworkService.Containers;
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
		private Valve _selectedValve;
		private DataObject currentlyDraggedData;

		public Valve SelectedValve
		{
			get { return _selectedValve; }
			set
			{
				SetField(ref _selectedValve, value);
			}
		}
		public MyICommand<string> ClearDataGridCommand { get; set; }
		public MyICommand<object> DropCommand { get; set; }
		public MyICommand<object> ListViewSelectionChangedCommand { get; set; }
		#region DataGrids
		public ObservableCollection<Valve> DataGrid0 { get; set; }
		public ObservableCollection<Valve> DataGrid1 { get; set; }
		public ObservableCollection<Valve> DataGrid2 { get; set; }
		public ObservableCollection<Valve> DataGrid3 { get; set; }
		public ObservableCollection<Valve> DataGrid4 { get; set; }
		public ObservableCollection<Valve> DataGrid5 { get; set; }
		public ObservableCollection<Valve> DataGrid6 { get; set; }
		public ObservableCollection<Valve> DataGrid7 { get; set; }
		public ObservableCollection<Valve> DataGrid8 { get; set; }
		public ObservableCollection<Valve> DataGrid9 { get; set; }
		public ObservableCollection<Valve> DataGrid10 { get; set; }
		public ObservableCollection<Valve> DataGrid11 { get; set; }
		public ObservableCollection<Valve> DataGrid12 { get; set; }
		public ObservableCollection<Valve> DataGrid13 { get; set; }
		public ObservableCollection<Valve> DataGrid14 { get; set; }
		public ObservableCollection<Valve> DataGrid15 { get; set; }
		#endregion

		public NetworkViewViewModel()
		{
			ClearDataGridCommand = new MyICommand<string>(OnClearDataGrid);
			DropCommand = new MyICommand<object>(OnDrop);
			ListViewSelectionChangedCommand = new MyICommand<object>(OnListViewSelectionChanged);
			DataGrid0 = new ObservableCollection<Valve>();
			DataGrid1 = new ObservableCollection<Valve>();
			DataGrid2 = new ObservableCollection<Valve>();
			DataGrid3 = new ObservableCollection<Valve>();
			DataGrid4 = new ObservableCollection<Valve>();
			DataGrid5 = new ObservableCollection<Valve>();
			DataGrid6 = new ObservableCollection<Valve>();
			DataGrid7 = new ObservableCollection<Valve>();
			DataGrid8 = new ObservableCollection<Valve>();
			DataGrid9 = new ObservableCollection<Valve>();
			DataGrid10 = new ObservableCollection<Valve>();
			DataGrid11 = new ObservableCollection<Valve>();
			DataGrid12 = new ObservableCollection<Valve>();
			DataGrid13 = new ObservableCollection<Valve>();
			DataGrid14 = new ObservableCollection<Valve>();
			DataGrid15 = new ObservableCollection<Valve>();
			Container.DataGridMap.Add("0", DataGrid0);
			Container.DataGridMap.Add("1", DataGrid1);
			Container.DataGridMap.Add("2", DataGrid2);
			Container.DataGridMap.Add("3", DataGrid3);
			Container.DataGridMap.Add("4", DataGrid4);
			Container.DataGridMap.Add("5", DataGrid5);
			Container.DataGridMap.Add("6", DataGrid6);
			Container.DataGridMap.Add("7", DataGrid7);
			Container.DataGridMap.Add("8", DataGrid8);
			Container.DataGridMap.Add("9", DataGrid9);
			Container.DataGridMap.Add("10", DataGrid10);
			Container.DataGridMap.Add("11", DataGrid11);
			Container.DataGridMap.Add("12", DataGrid12);
			Container.DataGridMap.Add("13", DataGrid13);
			Container.DataGridMap.Add("14", DataGrid14);
			Container.DataGridMap.Add("15", DataGrid15);
		}

		// Check if list view item is already dragged somewhere
		private bool CheckIfItemAlreadyDragged()
		{			
			bool isDragged = false;

			foreach (var dgPair in Container.DataGridMap)
			{
				Valve draggedValve = dgPair.Value.FirstOrDefault(v => v.Id == SelectedValve.Id);
				if (draggedValve != null)
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
			if(SelectedValve != null)
			{
				if (!CheckIfItemAlreadyDragged())
				{
					currentlyDraggedData = new DataObject(typeof(Valve), SelectedValve);
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

			Valve droppedValve = currentlyDraggedData.GetData(typeof(Valve)) as Valve;

			DragValveCommand dragValveCommand = new DragValveCommand(droppedValve, targetDataGridId);
			Container.CommandInvoker.AddAndExecute(dragValveCommand);
		}

		private void OnClearDataGrid(string targetDataGridId)
		{
			if(Container.DataGridMap[targetDataGridId].Count > 0)
			{
				ClearGridCommand clearGridCommand = new ClearGridCommand(Container.DataGridMap[targetDataGridId][0], targetDataGridId);
				Container.CommandInvoker.AddAndExecute(clearGridCommand);
			}
		}
	}
}