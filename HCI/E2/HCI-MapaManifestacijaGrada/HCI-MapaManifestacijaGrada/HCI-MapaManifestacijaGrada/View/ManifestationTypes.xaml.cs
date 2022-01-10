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
    /// Interaction logic for ManifestationTypes.xaml
    /// </summary>
    public partial class ManifestationTypes : Window
    {
        private MainWindow parent1;
        ManifestationTypeCtrl manifestationTypeCtrl;
		ManifestationCtrl manifestationCtrl;
        ManifestationType selectedManifestationType;
        private ObservableCollection<ManifestationType> typeList = new ObservableCollection<ManifestationType>();

        public ObservableCollection<ManifestationType> TypeList
        {
            get { return typeList; }
            set { typeList = value; }
        }

        public ManifestationTypes()
        {
            InitializeComponent();
            this.DataContext = this;
            initDataGridView();
        }
        
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddManifestationType addManifestationType = new AddManifestationType();
            addManifestationType.ShowDialog();
            initDataGridView();
        }

        private void onRowSelect(object sender, SelectionChangedEventArgs e)
        {
            selectedManifestationType = (ManifestationType)dgManifestationType.SelectedItem;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
         

            if (selectedManifestationType != null)
            {
                MessageBox.Show("Ako budete menjali tip, on ce se promeniti kod svih manifestacija koje ga sadrže!", "Upozorenje",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                AddManifestationType addManifestationType = new AddManifestationType(selectedManifestationType);
                addManifestationType.ShowDialog();
                initDataGridView();
            }
            else
            {
                MessageBox.Show("Neophodno je selektovati tip manifestacije.", "Upozorenje",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedManifestationType != null)
            {
				List<Manifestation> manifestacije = manifestationCtrl.FindAll();
				foreach(Manifestation m in manifestacije)
				{
					if(m.Tip.JedinstvenaOznaka == selectedManifestationType.JedinstvenaOznaka)
					{
						manifestationCtrl.Delete(m.JedinstvenaOznaka);
					}
				}
                manifestationTypeCtrl.Delete(selectedManifestationType.JedinstvenaOznaka);
                initDataGridView();
            }
            else
            {
                MessageBox.Show("Neophodno je selektovati tip manifestacije.", "Upozorenje",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void initDataGridView()
        {
            manifestationTypeCtrl = new ManifestationTypeCtrl();
			manifestationCtrl = new ManifestationCtrl();
            typeList = new ObservableCollection<ManifestationType>(manifestationTypeCtrl.FindAll());
            dgManifestationType.ItemsSource = null;
            dgManifestationType.ItemsSource = typeList;
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpViewer hh = new HelpViewer("NoviTip", parent1);
            hh.Show();
        }
    }
}
