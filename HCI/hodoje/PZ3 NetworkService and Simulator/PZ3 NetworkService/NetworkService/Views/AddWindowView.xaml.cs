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

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for AddWindowView.xaml
    /// </summary>
    public partial class AddWindowView : Window
    {
        public AddWindowView()
        {
            InitializeComponent();
            this.DataContext = new NetworkService.ViewModel.NetworkDataViewModel();
        }
    }
}
