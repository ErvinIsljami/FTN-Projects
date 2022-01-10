using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PZ1.Converters
{
    public class MessageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var messageColor = Colors.Transparent;

            if (value != null)
            {
                string message = value as string;
                if (!String.IsNullOrWhiteSpace(message))
                {
                    if (message.Contains("success"))
                    {
                        messageColor = Colors.Green;
                    }
                    else
                    {
                        messageColor = Colors.Red;
                    }
                }
            }
            return new SolidColorBrush(messageColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
