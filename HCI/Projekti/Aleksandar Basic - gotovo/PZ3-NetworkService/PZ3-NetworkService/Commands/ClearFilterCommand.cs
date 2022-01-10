using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class ClearFilterCommand : ICommand
	{
		private List<Reactor> previousState;

		public ClearFilterCommand()
		{
			previousState = new List<Reactor>();

			// https://stackoverflow.com/questions/3499903/how-to-get-items-count-from-collectionviewsource
			foreach (var item in Container.FilterCollection.View)
			{
				previousState.Add(item as Reactor);
			}
		}

		public void Execute()
		{
			Container.FilterCollection.Source = Container.Reactors;
		}

		public void Undo()
		{
			Container.FilterCollection.Source = previousState;
		}
	}
}
