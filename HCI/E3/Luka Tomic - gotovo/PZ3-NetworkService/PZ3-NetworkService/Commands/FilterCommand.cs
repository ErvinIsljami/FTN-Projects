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
		private WaterMeterType filterBy;
		private FilterConditionType filterConditionType;
		private int id;
		private List<WaterMeter> previousState;

		public FilterCommand(WaterMeterType filterBy, FilterConditionType filterConditionType, int id)
		{
			this.filterBy = filterBy;
			this.filterConditionType = filterConditionType;
			this.id = id;
			previousState = new List<WaterMeter>();

			// https://stackoverflow.com/questions/3499903/how-to-get-items-count-from-collectionviewsource
			foreach (var item in Everything.FilterCollection.View)
			{
				previousState.Add(item as WaterMeter);
			}
		}

		public void Execute()
		{
			if (filterBy == WaterMeterType.Turbo975)
			{
				List<WaterMeter> waterMeters = new List<WaterMeter>();
				if (id <= 0)
				{
					waterMeters = Everything.WaterMeters.Where(wm => wm.WaterMeterType == WaterMeterType.Turbo975).ToList();
				}
				else
				{
					if (filterConditionType == FilterConditionType.LessThan)
					{
						foreach (var wm in Everything.WaterMeters)
						{
							if (wm.Id < id && wm.WaterMeterType == WaterMeterType.Turbo975)
							{
								waterMeters.Add(wm);
							}
						}
					}
					else
					{
						foreach (var wm in Everything.WaterMeters)
						{
							if (wm.Id >= id && wm.WaterMeterType == WaterMeterType.Turbo975)
							{
								waterMeters.Add(wm);
							}
						}
					}
				}
				Everything.FilterCollection.Source = waterMeters;
			}
			else if (filterBy == WaterMeterType.HidroMer71)
			{
				List<WaterMeter> waterMeters = new List<WaterMeter>();
				if (id <= 0)
				{
					waterMeters = Everything.WaterMeters.Where(wm => wm.WaterMeterType == WaterMeterType.HidroMer71).ToList();
				}
				else
				{
					if (filterConditionType == FilterConditionType.LessThan)
					{
						foreach (var wm in Everything.WaterMeters)
						{
							if (wm.Id < id && wm.WaterMeterType == WaterMeterType.HidroMer71)
							{
								waterMeters.Add(wm);
							}
						}
					}
					else
					{
						foreach (var wm in Everything.WaterMeters)
						{
							if (wm.Id >= id && wm.WaterMeterType == WaterMeterType.HidroMer71)
							{
								waterMeters.Add(wm);
							}
						}
					}
				}
				Everything.FilterCollection.Source = waterMeters;
			}
			else
			{
				Everything.FilterCollection.Source = Everything.WaterMeters;
			}
		}

		public void Undo()
		{
			Everything.FilterCollection.Source = previousState;
		}
	}
}
