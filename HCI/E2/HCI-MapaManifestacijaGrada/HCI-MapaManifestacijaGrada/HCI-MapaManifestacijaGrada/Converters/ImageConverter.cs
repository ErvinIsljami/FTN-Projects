using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace HCI_MapaManifestacijaGrada.Converters
{
	public class ImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			BitmapImage bitmapImage = new BitmapImage();
			string path = value as string;
			try
			{
				if (!String.IsNullOrWhiteSpace(path))
				{
					// We need to take LocalPath in case of URI format paths (file:///)
					// because in that case File.Exists returns false
					string relativePath = $@"..\..\Resources\{path}";
					string absolutePath = Path.GetFullPath(relativePath);
					string localPath = new Uri(absolutePath).LocalPath;
					if (File.Exists(localPath))
					{
						bitmapImage = new BitmapImage(new Uri(localPath));
					}
				}
			}
			catch (Exception e)
			{

			}
			return bitmapImage;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
