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
	public class DeleteReactorCommand : ICommand
	{
		private Reactor reactorToDelete;
		private ObservableCollection<Reactor> dataGrid;

		public DeleteReactorCommand(Reactor reactorToDelete)
		{
			this.reactorToDelete = reactorToDelete;
		}

		public void Execute()
		{
			// Clear network view
			foreach (var dg in Container.DataGridMap)
			{
				Reactor draggedReactor = dg.Value.FirstOrDefault(v => v.Id == reactorToDelete.Id);
				if (draggedReactor != null)
				{
					dataGrid = dg.Value;
					dg.Value.Clear();
				}
			}
			Container.Reactors.Remove(reactorToDelete);
			Container.FilterCollection.Source = Container.Reactors;
			Container.SaveReactorsToXml();
		}

		public void Undo()
		{
			Container.Reactors.Add(reactorToDelete);
			Container.FilterCollection.Source = Container.Reactors;
			if(dataGrid != null)
			{
				dataGrid.Add(reactorToDelete);
			}	
			Container.SaveReactorsToXml();
		}
	}
}
