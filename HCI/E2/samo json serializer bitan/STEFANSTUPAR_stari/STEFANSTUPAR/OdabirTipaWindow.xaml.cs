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
    /// Interaction logic for OdabirTipaWindow.xaml
    /// </summary>
    public partial class OdabirTipaWindow : Window
    {
        public OdabirTipaWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            foreach (Tip t in Tip.tipovi)
            {
                dgrTip.Items.Add(t);
            }
        }

        private void Nazad_ButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void OdaberiTip_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (dgrTip.SelectedValue is Tip)
            {
                if (dgrTip.SelectedValue is Tip)
                {
                    Tip.temp = (Tip)dgrTip.SelectedValue;
                    pageDodajLokal.Instance().tbTip.Text = Tip.temp.Oznaka;
                }
                //else VALIDACIJA
                this.Close();
            }
            //else VALIDACIJA
        }
    }
}
