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
using STEFANSTUPAR.Models;

namespace STEFANSTUPAR
{
    /// <summary>
    /// Interaction logic for pageDodajEtiketu.xaml
    /// </summary>
    public partial class pageDodajEtiketu : Page
    {

        public pageDodajEtiketu()
        {
            InitializeComponent();
        }

        private void DodajEtiketu_ButtonClick(object sender, RoutedEventArgs e)
        {
            alert.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            if (string.IsNullOrWhiteSpace(tbOpis.Text) || string.IsNullOrWhiteSpace(tbOznaka.Text) || cp.SelectedColor.Equals(null))
            {
                alert.Text = "Niste uneli sve podatke !";
                return;
            }
            double r;
            if (double.TryParse(tbOpis.Text,out r)||double.TryParse(tbOznaka.Text,out r))
            {
                alert.Text = "Možete uneti samo slova. Brojevi nisu dozvoljeni !";
                return;

            }
            if (MainWindow.naziviEtiketa.Contains(tbOznaka.Text))
            {
                alert.Text = "Postoji etiketa !";
                return;
            }
            else
                MainWindow.naziviEtiketa.Add(tbOznaka.Text);

            Etiketa.etikete.Add(new Etiketa(tbOznaka.Text, tbOpis.Text, (Color)cp.SelectedColor));
            alert.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            alert.Text = "Etiketa uspešno dodata !";
        }

        /*
        private void IspisiLISTU(object sender, RoutedEventArgs e)
        {
            foreach (Etiketa List in Etiketa.etikete) {
                Console.WriteLine(List.Oznaka);
            }
        }
        */
    }
}
