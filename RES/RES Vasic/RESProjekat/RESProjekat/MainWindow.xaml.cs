using RESProjekat.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RESProjekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Transformator> transformatori = DataAccessHelper.Instance.Transformatori;
        public MainWindow()
        {
            InitializeComponent();

            cmb_box_grupe.ItemsSource = (DataAccessHelper.Instance.Grupe.Select(x => x.Kod));
            cmb_box_grupe.SelectedIndex = 0;
            dataGrid.ItemsSource = DataAccessHelper.Instance.GrupisanaBaza[(string)cmb_box_grupe.SelectedItem];


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window dodajNoviGenerator_Window = new DodajNoviGeneratorWindow();
            dodajNoviGenerator_Window.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window dodajNovuGrupuWindow = new DodajNovuGrupuWindow();
            dodajNovuGrupuWindow.Show();
        }

        private void Cmb_box_grupe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGrid.ItemsSource = DataAccessHelper.Instance.GrupisanaBaza[(string)cmb_box_grupe.SelectedItem];
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Window promenaTipa = new PromenaTipaKontroleWindow((Transformator)dataGrid.SelectedItem);
            promenaTipa.Show();
        } 
    }
}
