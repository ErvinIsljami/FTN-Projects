using RESProjekat.Model;
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

namespace RESProjekat
{
    /// <summary>
    /// Interaction logic for DodajNovuGrupuWindow.xaml
    /// </summary>
    public partial class DodajNovuGrupuWindow : Window
    {
        public DodajNovuGrupuWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Grupa g = new Grupa(txt_box_kod.Text);
            try
            {
                if(DataAccessHelper.Instance.DodajGrupu(g))
                    this.Close();
                else
                    MessageBox.Show("Grupa vec postoji.", "Greska.", MessageBoxButton.OK, MessageBoxImage.Error);
               

            }
            catch
            {
                MessageBox.Show("Molimo vas da pregledate opet sve podatke.", "Greska prilikom unosa.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
