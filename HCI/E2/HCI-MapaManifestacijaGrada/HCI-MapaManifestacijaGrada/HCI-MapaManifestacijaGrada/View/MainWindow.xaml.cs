using HCI_MapaManifestacijaGrada.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HCI_MapaManifestacijaGrada.Tutorial;
using HCI_MapaManifestacijaGrada.MyHelp;
using HCI_MapaManifestacijaGrada.Model;
using HCI_MapaManifestacijaGrada.HelperModels;
using HCI_MapaManifestacijaGrada.Controller;
using System.IO;
using System.Collections.ObjectModel;

namespace HCI_MapaManifestacijaGrada
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		public MainWindow()
        {
			InitializeComponent();
        }

        private void BtnManifestationTypes_Click(object sender, RoutedEventArgs e)
        {
            ManifestationTypes manifestationTypes = new ManifestationTypes();
            manifestationTypes.ShowDialog();
        }

        private void BtnManifestations_Click(object sender, RoutedEventArgs e)
        {
            Manifestations manifestations = new Manifestations();
            manifestations.ShowDialog();
        }

        private void BtnTag_Click(object sender, RoutedEventArgs e)
        {
            Tags tags = new Tags();
            tags.ShowDialog();
        }

        private void BtnTutorial_Click(object sender, RoutedEventArgs e)
        {
            Tutorijal tut = new Tutorijal();
            tut.ShowDialog();
        }

		private void BtnMap_Click(object sender, RoutedEventArgs e)
		{
			Map map = new Map();
			map.ShowDialog();
		}

		private void BtnHelp_Click(object sender, RoutedEventArgs e)
		{
			HelpViewer hh = new HelpViewer("Main", null);
			hh.Show();
		}
	}
}
