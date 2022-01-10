using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SystemController
{
	/// <summary>
	/// Interaction logic for ForecastWindow.xaml
	/// </summary>
	public partial class ForecastWindow : Window
	{
		public ForecastWindow(string forecast)
		{
			InitializeComponent();

			if (String.IsNullOrWhiteSpace(forecast))
			{
				forecastTextBlock.Text = "There is no forecast yet...";
			}
			else
			{
				forecastTextBlock.Text = forecast;
			}
		}
	}
}
