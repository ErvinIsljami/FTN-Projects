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
            //textBoxIme.Text = "unesite ime";
            //textBoxIme.Foreground = Brushes.LightSlateGray;
            InitializeComponent();

            textBoxIme.Text = "unesite ime";
            textBoxIme.Foreground = Brushes.LightSlateGray;

            textBoxPrezime.Text = "unesite prezime";
            textBoxPrezime.Foreground = Brushes.LightSlateGray;

            comboBoxSmer.ItemsSource = Constants.smerovi;
        }

        private void buttonGenerisi_Click(object sender, RoutedEventArgs e)
        {
            Student novi;

            if (radioButtonM.IsChecked==true)
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

        private void buttonIzadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textBoxIme_GotFocus(object sender, RoutedEventArgs e)
        {
            if(textBoxIme.Text.Trim().Equals("unesite ime"))
            {
                textBoxIme.Text = "";
                textBoxIme.Foreground = Brushes.Black;
            }
        }

        private void textBoxIme_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxIme.Text.Trim().Equals(string.Empty))
            {
                textBoxIme.Text = "unesite ime";
                textBoxIme.Foreground = Brushes.LightSlateGray;
            }
        }

        private void textBoxPrezime_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxPrezime.Text.Trim().Equals("unesite prezime"))
            {
                textBoxPrezime.Text = "";
                textBoxPrezime.Foreground = Brushes.Black;
            }
        }

        private void textBoxPrezime_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxPrezime.Text.Trim().Equals(string.Empty))
            {
                textBoxPrezime.Text = "unesite prezime";
                textBoxPrezime.Foreground = Brushes.LightSlateGray;
            }
        }
    }
}
