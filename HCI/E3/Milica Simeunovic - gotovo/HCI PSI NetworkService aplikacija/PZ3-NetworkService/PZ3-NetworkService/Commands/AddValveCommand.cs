using PZ3_NetworkService.Containers;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class AddValveCommand : ICommand
	{
		private Valve valveToAdd;

		public AddValveCommand(Valve valveToAdd)
		{
			this.valveToAdd = valveToAdd;
		}

		public void Execute()
		{
			Container.Valves.Add(valveToAdd);
			Container.SearchCollection.Source = Container.Valves;
			Container.SaveValvesToXml();
		}

		public void Undo()
		{
			foreach (var dgPair in Container.DataGridMap)
			{
				Valve draggedValve = dgPair.Value.FirstOrDefault(v => v.Id == valveToAdd.Id);
				if (draggedValve != null)
				{
					dgPair.Value.Clear();
				}
			}
			Container.Valves.Remove(valveToAdd);
			Container.SearchCollection.Source = Container.Valves;
			Container.SaveValvesToXml();
		}
	}
}
