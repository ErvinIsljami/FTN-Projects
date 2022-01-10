using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class FilterCommand : ICommand
	{
		private ReactorType filterBy;
		private bool isFilterLessThan;
		private int id;
		private List<Reactor> previousState;

		public FilterCommand(ReactorType filterBy, bool isFilterLessThan, int id)
		{
			this.filterBy = filterBy;
			this.isFilterLessThan = isFilterLessThan;
			this.id = id;
			previousState = new List<Reactor>();

			// https://stackoverflow.com/questions/3499903/how-to-get-items-count-from-collectionviewsource
			foreach (var item in Container.FilterCollection.View)
			{
				previousState.Add(item as Reactor);
			}
		}

		public void Execute()
		{
			if (filterBy == ReactorType.Thermal)
			{
				List<Reactor> reactors = new List<Reactor>();
				if (id <= 0)
				{
					reactors = Container.Reactors.Where(r => r.ReactorType == ReactorType.Thermal).ToList();
				}
				else
				{
					if (isFilterLessThan)
					{
						foreach (var r in Container.Reactors)
						{
							if (r.Id < id && r.ReactorType == ReactorType.Thermal)
							{
								reactors.Add(r);
							}
						}
					}
					else
					{
						foreach (var r in Container.Reactors)
						{
							if (r.Id >= id && r.ReactorType == ReactorType.Thermal)
							{
								reactors.Add(r);
							}
						}
					}
				}
				Container.FilterCollection.Source = reactors;
			}
			else if (filterBy == ReactorType.Fusion)
			{
				List<Reactor> reactors = new List<Reactor>();
				if (id <= 0)
				{
					reactors = Container.Reactors.Where(r => r.ReactorType == ReactorType.Fusion).ToList();
				}
				else
				{
					if (isFilterLessThan)
					{
						foreach (var r in Container.Reactors)
						{
							if (r.Id < id && r.ReactorType == ReactorType.Fusion)
							{
								reactors.Add(r);
							}
						}
					}
					else
					{
						foreach (var r in Container.Reactors)
						{
							if (r.Id >= id && r.ReactorType == ReactorType.Fusion)
							{
								reactors.Add(r);
							}
						}
					}
				}
				Container.FilterCollection.Source = reactors;
			}
			else
			{
				Container.FilterCollection.Source = Container.Reactors;
			}
		}

		public void Undo()
		{
			Container.FilterCollection.Source = previousState;
		}
	}
}
