using PZ3_NetworkService;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class AddReactorCommand : ICommand
	{
		private Reactor reactorToAdd;

		public AddReactorCommand(Reactor reactorToAdd)
		{
			this.reactorToAdd = reactorToAdd;
		}

		public void Execute()
		{
			Container.Reactors.Add(reactorToAdd);
			Container.FilterCollection.Source = Container.Reactors;
			Container.SaveReactorsToXml();
		}

		public void Undo()
		{
			foreach (var dgPair in Container.DataGridMap)
			{
				Reactor draggedReactor = dgPair.Value.FirstOrDefault(v => v.Id == reactorToAdd.Id);
				if (draggedReactor != null)
				{
					dgPair.Value.Clear();
				}
			}
			Container.Reactors.Remove(reactorToAdd);
			Container.FilterCollection.Source = Container.Reactors;
			Container.SaveReactorsToXml();
		}
	}
}
