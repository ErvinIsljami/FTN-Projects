using PZ3_NetworkService.Containers;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class DragValveCommand : ICommand
	{
		private Valve draggedValve;
		private string targetDataGridId;

		public DragValveCommand(Valve draggedValve, string targetDataGridId)
		{
			this.draggedValve = draggedValve;
			this.targetDataGridId = targetDataGridId;
		}

		public void Execute()
		{
			// Check if datagrid to which we're gonna drop is taken or not
			if (Container.DataGridMap[targetDataGridId].Count == 0)
			{
				Container.DataGridMap[targetDataGridId].Add(draggedValve);
				Container.DataGridMap[targetDataGridId][0].IsDragged = true;
			}
		}

		public void Undo()
		{
			if(Container.DataGridMap[targetDataGridId].Count > 0)
			{
				Container.DataGridMap[targetDataGridId][0].IsDragged = false;
				Container.DataGridMap[targetDataGridId].Clear();
			}
		}
	}
}
