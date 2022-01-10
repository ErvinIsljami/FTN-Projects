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

namespace Vezba2
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();

            comboBoxSmer.ItemsSource = Classes.Constants.smerovi;
        }

        private void buttonDodaj_Click(object sender, RoutedEventArgs e)
        {
            Classes.Student novi;

            if (validate())
            {
                if (radioButtonM.IsChecked == true)
                {
                    novi = new Classes.Student(textBoxIme.Text, textBoxPrezime.Text, 'M', comboBoxSmer.SelectedValue.ToString());
                    MainWindow.Studenti.Add(novi);
                    this.Close();
                }
                else
                {
                    novi = new Classes.Student(textBoxIme.Text, textBoxPrezime.Text, 'Z', comboBoxSmer.SelectedValue.ToString());
                    MainWindow.Studenti.Add(novi);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Podaci nisu dobro popunjeni", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonIzadji_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool validate()
        {
            bool result = true;

            if (textBoxIme.Text.Trim().Equals(""))
            {
                result = false;
                textBoxIme.BorderBrush = Brushes.Red;
                textBoxIme.BorderThickness = new Thickness(1);
                labelImeGreska.Content = "Ne moze biti prazno!";
            }
            else
            {
                textBoxIme.BorderBrush = Brushes.Green;
                labelImeGreska.Content = string.Empty;
            }

            if (textBoxPrezime.Text.Trim().Equals(""))
            {
                result = false;
                textBoxPrezime.BorderBrush = Brushes.Red;
                textBoxPrezime.BorderThickness = new Thickness(1);
                labelPrezimeGreska.Content = "Ne moze biti prazno!";
            }
            else
            {
                textBoxPrezime.BorderBrush = Brushes.Green;
                labelPrezimeGreska.Content = string.Empty;
            }

            if (radioButtonM.IsChecked == false && radioButtonZ.IsChecked == false)
            {
                result = false;
                labelPolGreska.Content = "Mora biti odabrana opcija!";
            }
            else
            {
                labelPolGreska.Content = string.Empty;
            }

            if (comboBoxSmer.SelectedItem == null)
            {
                result = false;
                comboBoxSmer.BorderBrush = Brushes.Red;
                comboBoxSmer.BorderThickness = new Thickness(1);
                labelSmerGreska.Content = "Mora biti odabrana opcija!";
            }
            else
            {
                comboBoxSmer.BorderBrush = Brushes.Green;
                labelSmerGreska.Content = string.Empty;
            }

            return result;
        }
    }
}
