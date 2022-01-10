using PZ3_NetworkService;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class DeleteWaterMeterCommand : ICommand
	{
		private WaterMeter waterMeterToDelete;
		private ObservableCollection<WaterMeter> dataGrid;

		public DeleteWaterMeterCommand(WaterMeter waterMeterToDelete)
		{
			this.waterMeterToDelete = waterMeterToDelete;
		}

		public void Execute()
		{
			foreach (var dg in Everything.DataGridMap)
			{
				WaterMeter draggedWaterMeter = dg.Value.FirstOrDefault(v => v.Id == waterMeterToDelete.Id);
				if (draggedWaterMeter != null)
				{
					dataGrid = dg.Value;
					dg.Value.Clear();
				}
			}
			Everything.WaterMeters.Remove(waterMeterToDelete);
			Everything.FilterCollection.Source = Everything.WaterMeters;
			Everything.SaveWaterMetersToXml();
		}

		public void Undo()
		{
			Everything.WaterMeters.Add(waterMeterToDelete);
			Everything.FilterCollection.Source = Everything.WaterMeters;
			if(dataGrid != null)
			{
				dataGrid.Add(waterMeterToDelete);	
			}
			Everything.SaveWaterMetersToXml();
		}
	}
}
