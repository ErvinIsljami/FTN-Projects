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
using System.Collections.ObjectModel;

namespace STEFANSTUPAR
{
    /// <summary>
    /// Interaction logic for pageDodajLokal.xaml
    /// </summary>
    public partial class pageDodajLokal : Page
    {

        private static pageDodajLokal instance = null;

        public static pageDodajLokal Instance()
        {
            return instance;
        }

        private ObservableCollection<Etiketa> odabraneEtikete = new ObservableCollection<Etiketa>();
        public ObservableCollection<Etiketa> OdabraneEtikete
        {
            get;
            set;
        }

        private ObservableCollection<string> alkoholi;
        public ObservableCollection<string> Alkoholi
        {
            get { return alkoholi; }
            set { }
        }

        private ObservableCollection<string> cene;
        public ObservableCollection<string> Cene
        {
            get { return cene; }
            set { }
        }

        public pageDodajLokal()
        {
            InitializeComponent();
            instance = this;
            alkoholi = new ObservableCollection<string>();
            alkoholi.Add("ne sluzi");
            alkoholi.Add("sluzi samo do 23:00");
            alkoholi.Add("sluzi kasno nocu");
            cene = new ObservableCollection<string>();
            cene.Add("niske cene");
            cene.Add("srednje cene");
            cene.Add("visoke cene");
            cene.Add("izuzetno visoke cene");
            this.DataContext = this;
        }

        private void DodajTip_ButtonClik(object sender, RoutedEventArgs e)
        {
            var s = new OdabirTipaWindow();
            s.Show();      
        }

        private void OdaberiIkonicuLokala_ButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();


            fileDialog.Title = "Izaberi ikonicu";
            fileDialog.Filter = "Images|*.jpg;*.jpeg;*.png|" +
                                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                "Portable Network Graphic (*.png)|*.png";
            if (fileDialog.ShowDialog() == true)
            {
                imgLokala.Source = new BitmapImage(new Uri(fileDialog.FileName));
                //slika = imgLokala.Source.ToString();
            }
        }

        private void OdaberiEtiketu_ButtonClick(object sender, RoutedEventArgs e)
        {
            var s = new OdabirEtiketeWindow();
            s.Show();
        }

        private void Nazad_ButtonClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void Dodaj_ButtonClick(object sender, RoutedEventArgs e)
        {
            //Etiketa.etikete.Add(new Etiketa(tbOznaka.Text, tbOpis.Text, (Color)cp.SelectedColor));
            //Lokal.lokali.Add(new Lokal(tbOznaka.Text, tbIme.Text,));
            //Console.WriteLine(Alkoholi[i]);
            //Console.WriteLine(Cene[i]);
            alert.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            if (string.IsNullOrWhiteSpace(tbIme.Text) || string.IsNullOrWhiteSpace(tbOznaka.Text) || string.IsNullOrWhiteSpace(tbOpis.Text) || string.IsNullOrWhiteSpace(tbKapacitet.Text) || string.IsNullOrWhiteSpace(tbEtiketa.Text) || string.IsNullOrWhiteSpace(tbTip.Text) || comboBox.SelectedItem == null || cbCene.SelectedItem == null ||
                (rbPusenjeDA.IsChecked.Equals(false) && rbPusenjeNE.IsChecked.Equals(false)) || (rbHendikepiraniDA.IsChecked.Equals(false) && rbHendikepiraniNE.IsChecked.Equals(false)) || (rbRezervacijeDA.IsChecked.Equals(false) && rbRezervacijeNE.IsChecked.Equals(false)) ||
                 dateCalendar.SelectedDate.Equals(null))
            {
                alert.Text = "Niste uneli sve podatke !";
                return;
            }
            double r;
            if (double.TryParse(tbIme.Text, out r) || double.TryParse(tbOznaka.Text, out r) || double.TryParse(tbOpis.Text, out r) || double.TryParse(tbEtiketa.Text, out r) || double.TryParse(tbKapacitet.Text, out r) || double.TryParse(tbTip.Text, out r))
            {
                alert.Text = "Možete uneti samo slova. Brojevi nisu dozvoljeni !";
                return;

            }

            Boolean pusenje = false;
            if (rbPusenjeDA.IsChecked == true)
            {
                pusenje = true;
            }
            Boolean rezervacije = false;
            if (rbRezervacijeDA.IsChecked == true)
            {
                rezervacije = true;
            }
            Boolean hendikepirani = false;
            if (rbHendikepiraniDA.IsChecked == true)
            {
                hendikepirani = true;
            }
            //dateCalendar.SelectedDate.ToString();
            Tip tip = new Tip();

            foreach (Tip t in Tip.tipovi)
            {
                if (t.Oznaka.Equals(tbTip.Text))
                {
                    tip = t;
                }
            }

            Lokal.lokali.Add(new Lokal(tbOznaka.Text, tbIme.Text, tbOpis.Text,tip,Alkoholi[comboBox.SelectedIndex], hendikepirani, pusenje, Cene[cbCene.SelectedIndex],tbKapacitet.Text,dateCalendar.SelectedDate.Value,  odabraneEtikete,imgLokala.Source.ToString() , rezervacije));
            alert.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            alert.Text = "Lokal uspešno dodata !";
        }


    }
}
