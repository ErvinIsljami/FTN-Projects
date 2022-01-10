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
using Vezba1_2;

namespace Vezba1_2WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {          
            InitializeComponent();

            textBoxIme.Text = "unesite ime";
            textBoxIme.Foreground = Brushes.SlateGray;

            comboBoxSmer.ItemsSource = Constants.smerovi;
        }

        private void buttonGenerisi_Click(object sender, RoutedEventArgs e)
        {
            Student novi;

            if (textBoxIme.Text.Equals("unesite ime"))
            {
                MessageBox.Show("Niste uneli ime!","GRESKA!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                if (radioButtonM.IsChecked == true)
                {
                    novi = new Student(textBoxIme.Text, textBoxPrezime.Text, 'M', comboBoxSmer.SelectedValue.ToString());
                }
                else
                {
                    novi = new Student(textBoxIme.Text, textBoxPrezime.Text, 'Z', comboBoxSmer.SelectedValue.ToString());
                }

                GeneratedView newWindow = new GeneratedView(novi);
                newWindow.ShowDialog();
            }
        }

        private void buttonIzadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textBoxIme_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxIme.Text.Equals("unesite ime"))
            {
                textBoxIme.Text = string.Empty;
                textBoxIme.Foreground = Brushes.Black;
            }
        }

        private void textBoxIme_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxIme.Text.Equals(string.Empty))
            {
                textBoxIme.Text = "unesite ime";
                textBoxIme.Foreground = Brushes.SlateGray;
            }
        }
    }
}
