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
	public class DoubleToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double? megaPascal = value as double?;
			if(megaPascal != null)
			{
				if(megaPascal < Everything.TemperatureMin || megaPascal > Everything.TemperatureMax)
				{
					return Brushes.Red;
				}
				else
				{
					return Brushes.ForestGreen;
				}
			}
			return Brushes.Black;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return -1;
		}
	}
}
