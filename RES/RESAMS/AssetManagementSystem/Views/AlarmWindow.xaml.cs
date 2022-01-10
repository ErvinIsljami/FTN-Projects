using AssetManagementSystem.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AssetManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for AlarmWindow.xaml
    /// </summary>

  

    public partial class AlarmWindow : Window
    {
        public static ObservableCollection<AMSDevice> DevicesCollection { get; set; }
        AMSDbContext dbContext = new AMSDbContext();
        public AlarmWindow(List<AMSDevice> list)
        {
            InitializeComponent();
            DevicesCollection = new ObservableCollection<AMSDevice>(list);
            data_grid.ItemsSource = DevicesCollection;

            
        }
    }
}
