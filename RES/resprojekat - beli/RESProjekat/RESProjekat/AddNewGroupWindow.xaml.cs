using Common;
using Common.Model;
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

namespace LocalController
{
	/// <summary>
	/// Interaction logic for DodajNovuGrupuWindow.xaml
	/// </summary>
	public partial class AddNewGroupWindow : Window
	{
		public Window SystemController { get; set; }

		public AddNewGroupWindow(Window systemController)
		{
			SystemController = systemController;

			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Group g = new Group(txt_box_kod.Text);
			try
			{
				if ((SystemController as dynamic).AddGroup(g))
				{
					this.Close();
				}
				else
				{
					MessageBox.Show("Group already exists.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			catch
			{
				MessageBox.Show("Please check all entered data.", "Error while submitting.", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
