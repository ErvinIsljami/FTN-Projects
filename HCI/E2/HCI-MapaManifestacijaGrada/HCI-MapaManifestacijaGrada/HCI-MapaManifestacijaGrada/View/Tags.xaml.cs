using HCI_MapaManifestacijaGrada.Model;
using HCI_MapaManifestacijaGrada.Controller;
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
using HCI_MapaManifestacijaGrada.MyHelp;

namespace HCI_MapaManifestacijaGrada.View
{
    /// <summary>
    /// Interaction logic for Tags.xaml
    /// </summary>
    public partial class Tags : Window
    {
        public MainWindow parent1;
        TagCtrl tagCtrl;
        Tag selectedTag;
        private ObservableCollection<Tag> tagList = new ObservableCollection<Tag>();

        public ObservableCollection<Tag> TagList
        {
            get { return tagList; }
            set { tagList = value; }
        }

        public Tags()
        {
            InitializeComponent();
            initDataGridView(); ;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTag addTag = new AddTag();
            addTag.ShowDialog();
            initDataGridView();
        }

        private void initDataGridView()
        {
            tagCtrl = new TagCtrl();
            tagList = new ObservableCollection<Tag>(tagCtrl.FindAll());
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = tagList;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTag != null)
            {
                AddTag addTag = new AddTag(selectedTag);
                addTag.ShowDialog();
                initDataGridView();
            }
            else
            {
                MessageBox.Show("Neophodno je selektovati tag.", "Upozorenje",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTag != null)
            {
                tagCtrl.Delete(selectedTag.JedinstvenaOznaka);
                initDataGridView();
            }
            else
            {
                MessageBox.Show("Neophodno je selektovati tag.", "Upozorenje",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnRowSelect(object sender, SelectedCellsChangedEventArgs e)
        {
            selectedTag = (Tag)dataGrid.SelectedItem;
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpViewer hh = new HelpViewer("ListaEtiketa", parent1);
            hh.Show();
        }
    }
}
