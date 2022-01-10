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
using Vezba1_2;

namespace Vezba1_2WPF
{
    /// <summary>
    /// Interaction logic for GeneratedView.xaml
    /// </summary>
    public partial class GeneratedView : Window
    {
        public GeneratedView()
        {
            InitializeComponent();
        }

        public GeneratedView(Student novi)
        {
            InitializeComponent();

            labelPodaci.Content += novi.Ime;
            labelPodaci.Content += " " + novi.Prezime;
            labelPodaci.Content += "\nSmer: " + novi.Smer;

            if(novi.Pol=='M')
            {
                labelPodaci.Content += "\nPol: Muski";
            }
            else
            {
                labelPodaci.Content += "\nPol: Zenski";
            }
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
