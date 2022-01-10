using PZ3_NetworkService.Containers;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class DeleteValveCommand : ICommand
	{
		private Valve valveToDelete;
		private ObservableCollection<Valve> dataGrid;

		public DeleteValveCommand(Valve valveToDelete)
		{
			this.valveToDelete = valveToDelete;
		}

		public void Execute()
		{
			foreach (var dg in Container.DataGridMap)
			{
				Valve draggedValve = dg.Value.FirstOrDefault(v => v.Id == valveToDelete.Id);
				if (draggedValve != null)
				{
					dataGrid = dg.Value;
					dg.Value.Clear();
				}
			}
			Container.Valves.Remove(valveToDelete);
			Container.SearchCollection.Source = Container.Valves;
			Container.SaveValvesToXml();
		}

		public void Undo()
		{
			Container.Valves.Add(valveToDelete);
			Container.SearchCollection.Source = Container.Valves;
			if(dataGrid != null)
			{
				dataGrid.Add(valveToDelete);	
			}
			Container.SaveValvesToXml();
		}
	}
}
