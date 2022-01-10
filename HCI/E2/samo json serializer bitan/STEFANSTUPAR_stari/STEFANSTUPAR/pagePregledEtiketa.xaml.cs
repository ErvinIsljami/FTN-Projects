using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
    /// Interaction logic for pagePregledEtiketa.xaml
    /// </summary>
    public partial class pagePregledEtiketa : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Etiketa> Etikete
        {
            get;
            set;
        }

        public pagePregledEtiketa()
        {
            InitializeComponent();
            this.DataContext=this;
            Etikete = new ObservableCollection<Etiketa>();
            foreach (Etiketa e in Etiketa.etikete)
            {
                //dgrEtiketa.Items.Add(e);
                Etikete.Add(e);
            }

            

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


        private Color boja;

        public Color Boja
        {
            get { return boja; }
            set
            {
                if (value != boja)
                {
                    boja = value;
                    OnPropertyChanged("Boja");
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



        private void IzmeniEtiketu_ButtonClick(object sender, RoutedEventArgs e)
        {
            Etiketa m;
            if (dgrEtiketa.SelectedValue is Etiketa)
            {
                m = (Etiketa)dgrEtiketa.SelectedValue;
                if (MainWindow.naziviEtiketa.Contains(tbOznaka.Text))
                { 
                    return;
                }
                else
                {
                    MainWindow.naziviEtiketa.Add(tbOznaka.Text);
                    MainWindow.naziviEtiketa.Remove(m.Oznaka);
                }

                m.Oznaka = tbOznaka.Text;
                m.Opis = tbOpis.Text;
                m.Boja = (Color)cp.SelectedColor;
            }
            //else VALIDACIJA
        }

        private void selektovano(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dgr = (DataGrid)sender;
            Etiketa temp = (Etiketa)dgr.SelectedValue;
            tbOznaka.Text = temp.Oznaka;
            tbOpis.Text = temp.Opis;
            cp.SelectedColor = temp.Boja;
        }

        
    }
}
