﻿using PZ3_NetworkService.Containers;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PZ3_NetworkService.Converters
{
	public class DoubleToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double? temperature = value as double?;
			if(temperature != null)
			{
				if(temperature < Container.TemperatureMin || temperature > Container.TemperatureMax)
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
