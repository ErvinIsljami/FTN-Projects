using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PZ1.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage bitmapImage = new BitmapImage();

            if (value != null)
            {
                if (!value.ToString().Equals(string.Empty))
                {
                    string path = value.ToString();
                    if (File.Exists(path))
                    {
                        bitmapImage = new BitmapImage(new Uri(path));
                    }
                }
            }

            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
