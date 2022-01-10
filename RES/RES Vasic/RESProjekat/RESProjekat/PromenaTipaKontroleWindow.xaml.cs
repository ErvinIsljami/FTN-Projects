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
    /// Interaction logic for PromenaTipaKontroleWindow.xaml
    /// </summary>
    public partial class PromenaTipaKontroleWindow : Window
    {
        private Transformator t;
        public PromenaTipaKontroleWindow(Transformator t)
        {
            InitializeComponent();
            slider.Value = (int)t.TipKontrole;
            this.t = t;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            t.TipKontrole = (ETipKontrole)slider.Value;
            this.Close();
        }
    }
}
