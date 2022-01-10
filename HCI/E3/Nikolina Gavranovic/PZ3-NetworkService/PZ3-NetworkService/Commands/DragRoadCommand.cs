﻿using PZ3_NetworkService;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class DragRoadCommand : ICommand
	{
		private Road draggedRoad;
		private string targetDataGridId;

		public DragRoadCommand(Road draggedRoad, string targetDataGridId)
		{
			this.draggedRoad = draggedRoad;
			this.targetDataGridId = targetDataGridId;
		}

		public void Execute()
		{
			// Check if datagrid to which we're gonna drop is taken or not
			if (Container.DataGridMap[targetDataGridId].Count == 0)
			{
				Container.DataGridMap[targetDataGridId].Add(draggedRoad);
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
