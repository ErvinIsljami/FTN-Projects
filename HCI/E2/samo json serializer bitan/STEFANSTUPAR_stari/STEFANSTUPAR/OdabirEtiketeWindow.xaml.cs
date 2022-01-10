using STEFANSTUPAR.Models;
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
using System.Windows.Shapes;

namespace STEFANSTUPAR
{
    /// <summary>
    /// Interaction logic for OdabirEtiketeWindow.xaml
    /// </summary>
    public partial class OdabirEtiketeWindow : Window
    {
        public OdabirEtiketeWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            
            foreach (Etiketa e in Etiketa.etikete)
            {
                dgrEtiketa.Items.Add(e);
            }
        }

        private void OdaberiEtiketu_ButtonClick(object sender, RoutedEventArgs e)
        {

            if (dgrEtiketa.SelectedValue is Etiketa)
            {
                Etiketa.temp = (Etiketa)dgrEtiketa.SelectedValue;
                pageDodajLokal.Instance().tbEtiketa.Text = Etiketa.temp.Oznaka;
                pageDodajLokal.Instance().OdabraneEtikete = new System.Collections.ObjectModel.ObservableCollection<Etiketa>();
                pageDodajLokal.Instance().OdabraneEtikete.Add(Etiketa.temp);
            }
            //else VALIDACIJA
            this.Close();
        }

        private void Nazad_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void selektovano(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
