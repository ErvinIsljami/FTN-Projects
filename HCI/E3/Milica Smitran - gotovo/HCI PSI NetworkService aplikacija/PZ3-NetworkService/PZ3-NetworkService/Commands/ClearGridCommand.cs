﻿using PZ3_NetworkService.Containers;
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
		private Reactor draggedReactor;
		private string targetDataGridId;

		public ClearGridCommand(Reactor draggedReactor, string targetDataGridId)
		{
			this.draggedReactor = draggedReactor;
			this.targetDataGridId = targetDataGridId;
		}

		public void Execute()
		{
			Container.DataGridMap[targetDataGridId][0].IsDragged = false;
			Container.DataGridMap[targetDataGridId].Clear();
		}

		public void Undo()
		{
			// Check if datagrid to which we're gonna drop is taken or not
			if (Container.DataGridMap[targetDataGridId].Count == 0)
			{
				Container.DataGridMap[targetDataGridId].Add(draggedReactor);
				Container.DataGridMap[targetDataGridId][0].IsDragged = true;
			}
		}
	}
}
