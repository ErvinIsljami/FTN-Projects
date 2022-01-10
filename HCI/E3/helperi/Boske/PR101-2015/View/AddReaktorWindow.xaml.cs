using PR101_2015.Model;
using PR101_2015.ViewModel;
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

namespace PR101_2015.View
{
    /// <summary>
    /// Interaction logic for AddReaktorWindow.xaml
    /// </summary>
    public partial class AddReaktorWindow : Window
    {
        public AddReaktorWindow()
        {
            InitializeComponent();
            comboBox.ItemsSource = MainViewModel.tipovi;
        }
    }
}
