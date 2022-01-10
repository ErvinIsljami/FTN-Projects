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

namespace HCI_MapaManifestacijaGrada.Tutorial
{
    /// <summary>
    /// Interaction logic for ZavrsenTutorijal.xaml
    /// </summary>
    public partial class ZavrsenTutorijal : Window
    {
        public ZavrsenTutorijal()
        {
            InitializeComponent();
        }

        private void UpozorenjePotvrdi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
