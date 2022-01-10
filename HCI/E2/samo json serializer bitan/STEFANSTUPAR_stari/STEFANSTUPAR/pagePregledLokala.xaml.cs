using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Microsoft.Win32;
using STEFANSTUPAR.Models;

namespace STEFANSTUPAR
{
    /// <summary>
    /// Interaction logic for pagePregledLokala.xaml
    /// </summary>
    public partial class pagePregledLokala : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }

        }

        public ObservableCollection<Lokal> Lokali
        {
            get;
            set;
        }

        

        private string naziv;

        public string Naziv
        {
            get { return naziv; }
            set
            {
                if (value != naziv)
                {
                    naziv = value;
                    OnPropertyChanged("Naziv");
                }
            }
        }

        private string oznaka;

        public string Oznaka
        {
            get { return oznaka; }
            set
            {
                if (value != oznaka)
                {
                    oznaka = value;
                    OnPropertyChanged("Oznaka");
                }
            }
        }

        private Tip tip;
        public Tip Tip
        {
            get
            {
                return tip;
            }
            set
            {
                if (value != tip)
                {
                    tip = value;
                    OnPropertyChanged("Tip");
                }
            }
        }

        private string alkohol;
        public string Alkohol
        {
            get
            {
                return alkohol;
            }
            set
            {
                if (value != alkohol)
                {
                    alkohol = value;
                    OnPropertyChanged("Alkohol");
                }
            }
        }

        private bool rezervacije;
        public bool Rezervacije
        {
            get
            {
                return rezervacije;
            }
            set
            {
                if (value != rezervacije)
                {
                    rezervacije = value;
                    OnPropertyChanged("Rezervacije");
                }
            }
        }

        private string cena;
        public string Cena
        {
            get
            {
                return cena;
            }
            set
            {
                if (value != cena)
                {
                    cena = value;
                    OnPropertyChanged("Cena");
                }
            }
        }

        private string kapacitet;
        public string Kapacitet
        {
            get
            {
                return kapacitet;
            }
            set
            {
                if (value != kapacitet)
                {
                    cena = value;
                    OnPropertyChanged("Kapacitet");
                }
            }
        }

        public pagePregledLokala()
        {
            InitializeComponent();
            this.DataContext = this;
            Lokali = new ObservableCollection<Lokal>();
            foreach (Lokal e in Lokal.lokali)
            {
                //dgrEtiketa.Items.Add(e);
                Lokali.Add(e);
            }
            
        }

        private void IzmeniLokal_ButtonClick(object sender, RoutedEventArgs e)
        {
            Lokal m;
            if (dgrLokal.SelectedValue is Etiketa)
            {
                m = (Lokal)dgrLokal.SelectedValue;
                /*
                if (MainWindow.naziviEtiketa.Contains(tbOznaka.Text))
                {
                    return;
                }
                else
                {
                    MainWindow.naziviEtiketa.Add(tbOznaka.Text);
                    MainWindow.naziviEtiketa.Remove(m.Oznaka);
                }
                */
                m.Naziv = tbIme.Text;
                m.Oznaka = tbOznaka.Text;
                m.Opis = tbOpis.Text;
                foreach (Tip t in Tip.tipovi)
                {
                    if (tbTip.Text.Equals(t.Oznaka))
                    {
                        m.Tip = t;
                    }
                }
                m.Alkohol = pageDodajLokal.Instance().Alkoholi[comboBox.SelectedIndex];
                m.Cena = pageDodajLokal.Instance().Cene[cbCene.SelectedIndex];
                m.Kapacitet = tbKapacitet.Text;
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
                m.Hendikepirani = hendikepirani;
                m.Rezervacije = rezervacije;
                m.Pusenje = pusenje;
                m.Datum = dateCalendar.SelectedDate.Value;
                m.Etikete = pageDodajLokal.Instance().OdabraneEtikete;
                m.Slika = imgLokala.Source.ToString();

            }
        }

        private void OdaberiTip_ButtonClik(object sender, RoutedEventArgs e)
        {
            var s = new OdabirTipaWindow();
            s.Show();
        }

        private void OdaberiEtiketu_ButtonClick(object sender, RoutedEventArgs e)
        {
            var s = new OdabirEtiketeWindow();
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

        private void selektovano(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dgr = (DataGrid)sender;
            Lokal temp = (Lokal)dgr.SelectedValue;
            tbIme.Text = temp.Naziv;
            tbOznaka.Text = temp.Oznaka;
            tbOpis.Text = temp.Opis;
            tbTip.Text = temp.Tip.Oznaka;
            comboBox.SelectedItem = temp.Alkohol;
            comboBox.SelectedItem = temp.Cena;
            tbKapacitet.Text = temp.Kapacitet;
            if(temp.Pusenje)
            {
                rbPusenjeDA.IsChecked = true;
                rbPusenjeNE.IsChecked = false;
            }
            else
            {
                rbPusenjeDA.IsChecked = false;
                rbPusenjeNE.IsChecked = true;
            }
            if (temp.Rezervacije)
            {
                rbRezervacijeDA.IsChecked = true;
                rbRezervacijeNE.IsChecked = false;
            }
            else
            {
                rbRezervacijeDA.IsChecked = false;
                rbRezervacijeNE.IsChecked = true;
            }
            if (temp.Hendikepirani)
            {
                rbHendikepiraniDA.IsChecked = true;
                rbHendikepiraniNE.IsChecked = false;
            }
            else
            {
                rbHendikepiraniDA.IsChecked = false;
                rbHendikepiraniNE.IsChecked = true;
            }
            //dateCalendar.SelectedDate.Value = temp.Datum;
            //tbEtiketa.Text = temp.Etikete[1].Oznaka;

            Uri imageUri = new Uri(temp.Slika, UriKind.Absolute);
            BitmapImage imageBitmap = new BitmapImage(imageUri);
            imgLokala.Source = imageBitmap;
        }
    }
}
