using PZ3_NetworkService;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class ClearGridCommand : ICommand
	{
		private WaterMeter draggedWaterMeter;
		private string targetDataGridId;

		public ClearGridCommand(WaterMeter draggedWaterMeter, string targetDataGridId)
		{
			this.draggedWaterMeter = draggedWaterMeter;
			this.targetDataGridId = targetDataGridId;
		}

		public void Execute()
		{
			Everything.DataGridMap[targetDataGridId][0].IsDragged = false;
			Everything.DataGridMap[targetDataGridId].Clear();
		}

		public void Undo()
		{
			// Check if datagrid to which we're gonna drop is taken or not
			if (Everything.DataGridMap[targetDataGridId].Count == 0)
			{
				Everything.DataGridMap[targetDataGridId].Add(draggedWaterMeter);
				Everything.DataGridMap[targetDataGridId][0].IsDragged = true;
			}
		}
	}
}
