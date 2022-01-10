using PZ3_NetworkService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PZ3_NetworkService.Converters
{
	public class DoubleToColorConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			double? traffic = values[0] as double?;
			RoadType? roadType = values[1] as RoadType?;
			if (traffic != null && roadType != null)
			{
				if(roadType == RoadType.IA)
				{
					if (traffic > Container.IAMax)
					{
						return Brushes.Red;
					}
					else
					{
						return Brushes.ForestGreen;
					}
				}
				else
				{
					if (traffic > Container.IBMax)
					{
						return Brushes.Red;
					}
					else
					{
						return Brushes.ForestGreen;
					}
				}
			}
			return Brushes.Black;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
