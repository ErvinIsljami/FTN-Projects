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
	public class IsDraggedToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool)
			{
				bool isDragged = (bool)value;
				return isDragged ? new SolidColorBrush(Color.FromArgb(127, 255, 255, 255)) : new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return false;
		}
	}
}
