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

namespace Vezba2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int colNum = 0;
        public static List<Classes.Student> Studenti = new List<Classes.Student>();

        public MainWindow()
        {
            InitializeComponent();
            dataGridStudenti.ItemsSource = Studenti;
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 4)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void obrisiStudenta(object sender, RoutedEventArgs e)
        {
            if (Studenti.Count > 0)
            {
                Studenti.RemoveAt(Studenti.Count - 1);
                dataGridStudenti.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Nije moguce brisati iz prazne tabele.", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonIzlaz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dodajStudenta(object sender, RoutedEventArgs e)
        {
            AddWindow newWindow = new AddWindow();
            newWindow.ShowDialog();
            dataGridStudenti.Items.Refresh();
        }
    }
}
