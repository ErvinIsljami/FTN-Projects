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
		private Reactor _selectedReactor;
		private DataObject currentlyDraggedData;

		public Reactor SelectedReactor
		{
			get { return _selectedReactor; }
			set
			{
				SetField(ref _selectedReactor, value);
			}
		}
		public MyICommand<string> ClearDataGridCommand { get; set; }
		public MyICommand<object> DropCommand { get; set; }
		public MyICommand<object> ListViewSelectionChangedCommand { get; set; }
		#region DataGrids
		public ObservableCollection<Reactor> DataGrid0 { get; set; }
		public ObservableCollection<Reactor> DataGrid1 { get; set; }
		public ObservableCollection<Reactor> DataGrid2 { get; set; }
		public ObservableCollection<Reactor> DataGrid3 { get; set; }
		public ObservableCollection<Reactor> DataGrid4 { get; set; }
		public ObservableCollection<Reactor> DataGrid5 { get; set; }
		public ObservableCollection<Reactor> DataGrid6 { get; set; }
		public ObservableCollection<Reactor> DataGrid7 { get; set; }
		public ObservableCollection<Reactor> DataGrid8 { get; set; }
		public ObservableCollection<Reactor> DataGrid9 { get; set; }
		public ObservableCollection<Reactor> DataGrid10 { get; set; }
		public ObservableCollection<Reactor> DataGrid11 { get; set; }
		public ObservableCollection<Reactor> DataGrid12 { get; set; }
		public ObservableCollection<Reactor> DataGrid13 { get; set; }
		public ObservableCollection<Reactor> DataGrid14 { get; set; }
		public ObservableCollection<Reactor> DataGrid15 { get; set; }
		#endregion

		public NetworkViewViewModel()
		{
			ClearDataGridCommand = new MyICommand<string>(OnClearDataGrid);
			DropCommand = new MyICommand<object>(OnDrop);
			ListViewSelectionChangedCommand = new MyICommand<object>(OnListViewSelectionChanged);
			DataGrid0 = new ObservableCollection<Reactor>();
			DataGrid1 = new ObservableCollection<Reactor>();
			DataGrid2 = new ObservableCollection<Reactor>();
			DataGrid3 = new ObservableCollection<Reactor>();
			DataGrid4 = new ObservableCollection<Reactor>();
			DataGrid5 = new ObservableCollection<Reactor>();
			DataGrid6 = new ObservableCollection<Reactor>();
			DataGrid7 = new ObservableCollection<Reactor>();
			DataGrid8 = new ObservableCollection<Reactor>();
			DataGrid9 = new ObservableCollection<Reactor>();
			DataGrid10 = new ObservableCollection<Reactor>();
			DataGrid11 = new ObservableCollection<Reactor>();
			DataGrid12 = new ObservableCollection<Reactor>();
			DataGrid13 = new ObservableCollection<Reactor>();
			DataGrid14 = new ObservableCollection<Reactor>();
			DataGrid15 = new ObservableCollection<Reactor>();
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
				Reactor draggedReactor = dgPair.Value.FirstOrDefault(v => v.Id == SelectedReactor.Id);
				if (draggedReactor != null)
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
			if(SelectedReactor != null)
			{
				if (!CheckIfItemAlreadyDragged())
				{
					currentlyDraggedData = new DataObject(typeof(Reactor), SelectedReactor);
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

			Reactor droppedReactor = currentlyDraggedData.GetData(typeof(Reactor)) as Reactor;

			DragReactorCommand dragReactorCommand = new DragReactorCommand(droppedReactor, targetDataGridId);
			Container.CommandInvoker.AddAndExecute(dragReactorCommand);
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