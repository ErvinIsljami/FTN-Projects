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
		private List<WaterMeter> previousState;

		public ClearFilterCommand()
		{
			previousState = new List<WaterMeter>();

			// https://stackoverflow.com/questions/3499903/how-to-get-items-count-from-collectionviewsource
			foreach (var item in Everything.FilterCollection.View)
			{
				previousState.Add(item as WaterMeter);
			}
		}

		public void Execute()
		{
			Everything.FilterCollection.Source = Everything.WaterMeters;
		}

		public void Undo()
		{
			Everything.FilterCollection.Source = previousState;
		}
	}
}
