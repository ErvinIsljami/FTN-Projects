using PZ3_NetworkService;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class DragWaterMeterCommand : ICommand
	{
		private WaterMeter draggedWaterMeter;
		private string targetDataGridId;

		public DragWaterMeterCommand(WaterMeter draggedWaterMeter, string targetDataGridId)
		{
			this.draggedWaterMeter = draggedWaterMeter;
			this.targetDataGridId = targetDataGridId;
		}

		public void Execute()
		{
			// Check if datagrid to which we're gonna drop is taken or not
			if (Everything.DataGridMap[targetDataGridId].Count == 0)
			{
				Everything.DataGridMap[targetDataGridId].Add(draggedWaterMeter);
				Everything.DataGridMap[targetDataGridId][0].IsDragged = true;
			}
		}

		public void Undo()
		{
			if(Everything.DataGridMap[targetDataGridId].Count > 0)
			{
				Everything.DataGridMap[targetDataGridId][0].IsDragged = false;
				Everything.DataGridMap[targetDataGridId].Clear();
			}
		}
	}
}
