using PZ3_NetworkService;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Commands
{
	public class DeleteRoadCommand : ICommand
	{
		private Road roadToDelete;
		private ObservableCollection<Road> dataGrid;

		public DeleteRoadCommand(Road roadToDelete)
		{
			this.roadToDelete = roadToDelete;
		}

		public void Execute()
		{
			foreach (var dg in Container.DataGridMap)
			{
				Road draggedRoad = dg.Value.FirstOrDefault(v => v.Id == roadToDelete.Id);
				if (draggedRoad != null)
				{
					dataGrid = dg.Value;
					dg.Value.Clear();
				}
			}
			Container.Roads.Remove(roadToDelete);
			Container.SearchCollection.Source = Container.Roads;
			Container.SaveRoadsToXml();
		}

		public void Undo()
		{
			Container.Roads.Add(roadToDelete);
			Container.SearchCollection.Source = Container.Roads;
			if(dataGrid != null)
			{
				dataGrid.Add(roadToDelete);	
			}
			Container.SaveRoadsToXml();
		}
	}
}
