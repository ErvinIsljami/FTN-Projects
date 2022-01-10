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
	/// Interaction logic for PromenaTipaKontroleWindow.xaml
	/// </summary>
	public partial class ChangeControlTypeAndValueWindow : Window
	{
		private Generator g;
		public Window SystemController { get; set; }
		public ChangeControlTypeAndValueWindow(Generator g, Window systemController)
		{
			InitializeComponent();
			slider.Value = (int)g.ControlType;
			this.g = g;
			SystemController = systemController;
			if (slider.Value == 0)
			{
				txt_box_trenutnaSnaga.IsReadOnly = false;
				txt_box_trenutnaSnaga.Text = g.CurrentActivePower.ToString();
			}
			else
			{
				txt_box_trenutnaSnaga.Text = "";
				txt_box_trenutnaSnaga.IsReadOnly = true;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				double newActivePowerValue = 0;

				if(slider.Value == 0)
				{
					newActivePowerValue = Double.Parse(txt_box_trenutnaSnaga.Text);
				}
				else
				{
					newActivePowerValue = 0;
				}

				(SystemController as dynamic).ChangeGeneratorControlTypeAndCurrentRealPower(g, (ControlTypeEnum)slider.Value, newActivePowerValue);
			}
			catch(Exception ex)
			{
				MessageBox.Show("If control type is LOCAL, set an active power value.", "Error", MessageBoxButton.OK);
			}
			this.Close();
		}

		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (slider.Value == 0)
			{
				if(txt_box_trenutnaSnaga != null)
				{
					txt_box_trenutnaSnaga.IsReadOnly = false;
				}
				//txt_box_trenutnaSnaga.Text = g.TrenutnaAktivnaSnaga.ToString();
			}
			else
			{
				if (txt_box_trenutnaSnaga != null)
				{
					txt_box_trenutnaSnaga.Text = "";
					txt_box_trenutnaSnaga.IsReadOnly = true;
				}
			}
		}
	}
}
