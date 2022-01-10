using Microsoft.Win32;
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
using STEFANSTUPAR.Models;

namespace STEFANSTUPAR
{
    /// <summary>
    /// Interaction logic for pageDodajTip.xaml
    /// </summary>
    public partial class pageDodajTip : Page
    {
        public pageDodajTip()
        {
            InitializeComponent();
        }

        private void DodajTip_ButtonClick(object sender, RoutedEventArgs e)
        {
            alert.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            if (string.IsNullOrWhiteSpace(tbOpis.Text) || string.IsNullOrWhiteSpace(tbOznaka.Text) || string.IsNullOrWhiteSpace(tbIme.Text))
            {
                alert.Text = "Niste uneli sve podatke !";
                return;
            }
            double r;
            if (double.TryParse(tbOpis.Text, out r) || double.TryParse(tbOznaka.Text, out r) || double.TryParse(tbIme.Text, out r))
            {
                alert.Text = "Možete uneti samo slova. Brojevi nisu dozvoljeni !";
                return;

            }

            Tip.tipovi.Add(new Tip(tbOznaka.Text, tbIme.Text, tbOpis.Text, imgTip.Source.ToString()));
            alert.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            alert.Text = "Tip je uspešno dodat !";
            
        }

        private void OdaberiIkonicu_ButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();


            fileDialog.Title = "Izaberi ikonicu";
            fileDialog.Filter = "Images|*.jpg;*.jpeg;*.png|" +
                                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                "Portable Network Graphic (*.png)|*.png";
            if (fileDialog.ShowDialog() == true)
            {
                imgTip.Source = new BitmapImage(new Uri(fileDialog.FileName));
                //slika = imgTip.Source.ToString();
            }
        }

        

    }
}
