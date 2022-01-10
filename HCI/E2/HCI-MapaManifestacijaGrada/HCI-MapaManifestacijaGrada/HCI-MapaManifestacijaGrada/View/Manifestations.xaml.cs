using HCI_MapaManifestacijaGrada.Controller;
using HCI_MapaManifestacijaGrada.Model;
using HCI_MapaManifestacijaGrada.MyHelp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace HCI_MapaManifestacijaGrada.View
{
    /// <summary>
    /// Interaction logic for Manifestations.xaml
    /// </summary>
    public partial class Manifestations : Window
    {
        public MainWindow parent1;
        ManifestationCtrl manifestationCtrl;
        Manifestation selectedManifestation;
        private ObservableCollection<Manifestation> manifestationList = new ObservableCollection<Manifestation>();

        public ObservableCollection<Manifestation> ManifestationList
        {
            get { return manifestationList; }
            set { manifestationList = value; }
        }

        public Manifestations()
        {
            InitializeComponent();
            this.DataContext = this;
            initDataGridView();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddManifestation addManifestation = new AddManifestation();
            addManifestation.ShowDialog();
            initDataGridView();
        }

        private void initDataGridView()
        {
            manifestationCtrl = new ManifestationCtrl();
            manifestationList = new ObservableCollection<Manifestation>(manifestationCtrl.FindAll());
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = manifestationList;
        }

        private void OnRowChange(object sender, SelectionChangedEventArgs e)
        {
            selectedManifestation = (Manifestation)dataGrid.SelectedItem;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedManifestation != null)
            {
                AddManifestation addManifestation = new AddManifestation(selectedManifestation);
                addManifestation.ShowDialog();
                initDataGridView();
            }
            else
            {
                MessageBox.Show("Neophodno je selektovati manifestaciju.", "Upozorenje",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedManifestation != null)
            {
                manifestationCtrl.Delete(selectedManifestation.JedinstvenaOznaka);
                initDataGridView();
            }
            else
            {
                MessageBox.Show("Neophodno je selektovati manifestaciju.", "Upozorenje",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpViewer hh = new HelpViewer("NovaManifestacija", parent1);
            hh.Show();
        }
    }
}
