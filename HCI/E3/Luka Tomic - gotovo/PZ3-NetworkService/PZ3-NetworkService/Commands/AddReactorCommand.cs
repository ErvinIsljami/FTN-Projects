using PZ3_NetworkService;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class AddWaterMeterCommand : ICommand
	{
		private WaterMeter waterMeterToAdd;

		public AddWaterMeterCommand(WaterMeter waterMeterToAdd)
		{
			this.waterMeterToAdd = waterMeterToAdd;
		}

		public void Execute()
		{
			Everything.WaterMeters.Add(waterMeterToAdd);
			Everything.FilterCollection.Source = Everything.WaterMeters;
			Everything.SaveWaterMetersToXml();
		}

		public void Undo()
		{
			foreach (var dgPair in Everything.DataGridMap)
			{
				WaterMeter draggedWaterMeter = dgPair.Value.FirstOrDefault(v => v.Id == waterMeterToAdd.Id);
				if (draggedWaterMeter != null)
				{
					dgPair.Value.Clear();
				}
			}
			Everything.WaterMeters.Remove(waterMeterToAdd);
			Everything.FilterCollection.Source = Everything.WaterMeters;
			Everything.SaveWaterMetersToXml();
		}
	}
}
