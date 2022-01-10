using Common;
using Common.Model;
using LocalController.Proxies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
	/// Interaction logic for DodajNoviGeneratorWindow.xaml
	/// </summary>
	public partial class AddNewGeneratorWindow : Window
	{
		public Window SystemController { get; set; }

		public AddNewGeneratorWindow(Window systemController)
		{
			SystemController = systemController;

			InitializeComponent();

			cmb_box_tip_jedinice.ItemsSource = Enum.GetValues(typeof(UnitTypeEnum)).Cast<UnitTypeEnum>();
			cmb_box_tip_jedinice.SelectedValue = UnitTypeEnum.SOLAR;
			cmb_box_tip_kontrole.ItemsSource = Enum.GetValues(typeof(ControlTypeEnum)).Cast<ControlTypeEnum>();
			cmb_box_tip_kontrole.SelectedValue = ControlTypeEnum.LOCAL;

			cmb_box_grupa.ItemsSource = (SystemController as dynamic).LocalGroupsIds;

			GeneratorIdServiceProxy gidProxy = new GeneratorIdServiceProxy();
			txt_box_kod.Text = gidProxy.GetNewGeneratorId();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string kod = txt_box_kod.Text;
				UnitTypeEnum tip = (UnitTypeEnum)cmb_box_tip_jedinice.SelectedValue;
				double trenutnaAktivnaSnaga = Double.Parse(txt_box_trenutna.Text);
				double min = Double.Parse(txt_box_min.Text);
				double max = Double.Parse(txt_box_max.Text);
				ControlTypeEnum tipKontrole = (ControlTypeEnum)cmb_box_tip_kontrole.SelectedValue;
				double cena = Double.Parse(txt_box_cena.Text);
				string grupa = (string)cmb_box_grupa.SelectedItem;
				Generator g = new Generator(kod, tip, trenutnaAktivnaSnaga, min, max, tipKontrole, cena, grupa);
				if ((SystemController as dynamic).AddGenerator(g))
				{
					this.Close();
				}
				else
				{
					MessageBox.Show("Generator already exists. Please try again.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			catch
			{
				MessageBox.Show("Please check all entered data.", "Error while submitting.", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
