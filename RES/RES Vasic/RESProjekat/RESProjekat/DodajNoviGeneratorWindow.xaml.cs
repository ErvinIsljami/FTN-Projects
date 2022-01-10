using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using RESProjekat.Model;

namespace RESProjekat
{
    /// <summary>
    /// Interaction logic for DodajNoviGeneratorWindow.xaml
    /// </summary>
    public partial class DodajNoviGeneratorWindow : Window
    {
        
        public DodajNoviGeneratorWindow()
        {
            InitializeComponent();

            cmb_box_tip_jedinice.ItemsSource = Enum.GetValues(typeof(ETipJedinice)).Cast<ETipJedinice>();
            cmb_box_tip_jedinice.SelectedValue = ETipJedinice.SOLAR;
            cmb_box_tip_kontrole.ItemsSource = Enum.GetValues(typeof(ETipKontrole)).Cast<ETipKontrole>();
            cmb_box_tip_kontrole.SelectedValue = ETipKontrole.LOCAL;

            cmb_box_grupa.ItemsSource = DataAccessHelper.Instance.Grupe.Select(x => x.Kod);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string kod = txt_box_kod.Text;
                ETipJedinice tip = (ETipJedinice)cmb_box_tip_jedinice.SelectedValue;
                double trenutnaAktivnaSnaga = Double.Parse(txt_box_trenutna.Text);
                double min = Double.Parse(txt_box_min.Text);
                double max = Double.Parse(txt_box_max.Text);
                ETipKontrole tipKontrole = (ETipKontrole)cmb_box_tip_kontrole.SelectedValue;
                double cena = Double.Parse(txt_box_cena.Text);
                Transformator t = new Transformator(kod, tip, trenutnaAktivnaSnaga, min, max, tipKontrole, cena);
                if(DataAccessHelper.Instance.DodajTransformator(t, (string)cmb_box_grupa.SelectedItem))
                    this.Close();
                else
                    MessageBox.Show("Transofmator vec postoji. Molimo pokusajte ponovo.", "Greska.", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch
            {
                MessageBox.Show("Molimo vas da pregledate opet sve podatke.", "Greska prilikom unosa.", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }
    }
}
