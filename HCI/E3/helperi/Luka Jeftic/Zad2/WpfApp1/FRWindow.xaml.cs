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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for FRWindow.xaml
    /// </summary>
    public partial class FRWindow : Window
    {
        public FRWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                MainWindow.find = tb_Find.Text;
                MainWindow.replace = tb_Replace.Text;

                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.find = "";
            MainWindow.replace = "";
            this.Close();
        }

        private bool validate()
        {
            bool result = true;
            if (tb_Find.Text.Trim().Equals(""))
            {
                result = false;
                tb_Find.BorderBrush = Brushes.Red;
            }
            else
            {
                tb_Find.BorderBrush = Brushes.Gray;
            }

            if (tb_Replace.Text.Trim().Equals(""))
            {
                result = false;
                tb_Replace.BorderBrush = Brushes.Red;
            }
            else
            {
                tb_Replace.BorderBrush = Brushes.Gray;
            }

            return result;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

           if (Mouse.LeftButton == MouseButtonState.Pressed)
                 this.DragMove();
            
        }
    }
}
