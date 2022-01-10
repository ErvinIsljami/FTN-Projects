using PZ3_NetworkService.Containers;
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
		private string filterType;
		private string lessThanOrGreaterThanOrEqual;
		private int id;
		private List<Reactor> previousState;

		public FilterCommand(string filterType, string lessThanOrGreaterThanOrEqual, int id)
		{
			this.filterType = filterType;
			this.lessThanOrGreaterThanOrEqual = lessThanOrGreaterThanOrEqual;
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
			if (filterType == "Thermal")
			{
				List<Reactor> reactors = new List<Reactor>();
				if (id <= 0)
				{
					reactors = Container.Reactors.Where(r => r.ReactorType == ReactorType.Thermal).ToList();
				}
				else
				{
					if (lessThanOrGreaterThanOrEqual == "LessThan")
					{
						foreach (var r in Container.Reactors)
						{
							if (r.Id < id && r.ReactorType == ReactorType.Thermal)
							{
								reactors.Add(r);
							}
						}
					}
					else if (lessThanOrGreaterThanOrEqual == "GreaterOrEqual")
					{
						foreach (var r in Container.Reactors)
						{
							if (r.Id >= id && r.ReactorType == ReactorType.Thermal)
							{
								reactors.Add(r);
							}
						}
					}
					else
					{
						reactors = Container.Reactors.Where(r => r.ReactorType == ReactorType.Thermal).ToList();
					}
				}
				Container.FilterCollection.Source = reactors;
			}
			else if (filterType == "Fusion")
			{
				List<Reactor> reactors = new List<Reactor>();
				if (id <= 0)
				{
					reactors = Container.Reactors.Where(r => r.ReactorType == ReactorType.Fusion).ToList();
				}
				else
				{
					if (lessThanOrGreaterThanOrEqual == "LessThan")
					{
						foreach (var r in Container.Reactors)
						{
							if (r.Id < id && r.ReactorType == ReactorType.Fusion)
							{
								reactors.Add(r);
							}
						}
					}
					else if (lessThanOrGreaterThanOrEqual == "GreaterOrEqual")
					{
						foreach (var r in Container.Reactors)
						{
							if (r.Id >= id && r.ReactorType == ReactorType.Fusion)
							{
								reactors.Add(r);
							}
						}
					}
					else
					{
						reactors = Container.Reactors.Where(r => r.ReactorType == ReactorType.Fusion).ToList();
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
