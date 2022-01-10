using System;
using System.Collections.Generic;
using System.Drawing;
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
				return isDragged ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(128, 128, 128)) : new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 255, 255, 255));
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return false;
		}
	}
}
