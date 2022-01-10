using System;
using System.Collections.Generic;
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
    /// Interaction logic for pagePregledTipova.xaml
    /// </summary>
    public partial class pagePregledTipova : Page
    {
        public pagePregledTipova()
        {
            InitializeComponent();

            foreach (Tip t in Tip.tipovi)
            {
                dgrTip.Items.Add(t);
            }
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

        private void IzmeniTip_ButtonClick(object sender, RoutedEventArgs e)
        {
            Tip m;
            if (dgrTip.SelectedValue is Tip)
            {
                m = (Tip)dgrTip.SelectedValue;
                m.Oznaka = tbOznaka.Text;
                m.Opis = tbOpis.Text;
                m.Naziv = tbIme.Text;
                m.Slika = imgTip.Source.ToString();
            }
            //else VALIDACIJA
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
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


        private string opis;

        public string Opis
        {
            get { return opis; }
            set
            {
                if (value != opis)
                {
                    opis = value;
                    OnPropertyChanged("Opis");
                }
            }
        }



        

        private void selektovano(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dgr = (DataGrid)sender;
            Tip temp = (Tip)dgr.SelectedValue;
            tbOznaka.Text = temp.Oznaka;
            tbIme.Text = temp.Naziv;
            tbOpis.Text = temp.Opis;
            //imgTip.Source = temp.Slika.;

            Uri imageUri = new Uri(temp.Slika, UriKind.Absolute);
            BitmapImage imageBitmap = new BitmapImage(imageUri);
            imgTip.Source = imageBitmap;
        }
    }
}
