using PZ3_NetworkService;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class AddRoadCommand : ICommand
	{
		private Road roadToAdd;

		public AddRoadCommand(Road roadToAdd)
		{
			this.roadToAdd = roadToAdd;
		}

		public void Execute()
		{
			Container.Roads.Add(roadToAdd);
			Container.SearchCollection.Source = Container.Roads;
			Container.SaveRoadsToXml();
		}

		public void Undo()
		{
			foreach (var dgPair in Container.DataGridMap)
			{
				Road draggedRoad = dgPair.Value.FirstOrDefault(v => v.Id == roadToAdd.Id);
				if (draggedRoad != null)
				{
					dgPair.Value.Clear();
				}
			}
			Container.Roads.Remove(roadToAdd);
			Container.SearchCollection.Source = Container.Roads;
			Container.SaveRoadsToXml();
		}
	}
}
