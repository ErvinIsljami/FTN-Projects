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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(validate())
            {
                resLabel.Content = (Int32.Parse(textBox.Text) + Int32.Parse(textBox1.Text));
            }
        }

        private bool validate()
        {
            bool result = true;

            if (textBox.Text.Trim().Equals(""))
            {
                result = false;
                textBox.BorderBrush = Brushes.Red;
                textBox.BorderThickness = new Thickness(1);
            }
            else
            {
                textBox.BorderBrush = Brushes.Black;
                try
                {
                    Int32.Parse(textBox.Text.Trim());
                }
                catch(Exception exc)
                {
                    MessageBox.Show("Prvi sabirak nije broj!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                    Console.WriteLine(exc.Message);
                    result = false;
                }
            }

            if (textBox1.Text.Trim().Equals(""))
            {
                result = false;
                textBox1.BorderBrush = Brushes.Red;
                textBox1.BorderThickness = new Thickness(1);
            }
            else
            {
                textBox.BorderBrush = Brushes.Black;
                try
                {
                    Int32.Parse(textBox1.Text.Trim());
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Drugi sabirak nije broj!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                    Console.WriteLine(exc.Message);
                    result = false;
                }
            }

            return result;
        }
    }
}
