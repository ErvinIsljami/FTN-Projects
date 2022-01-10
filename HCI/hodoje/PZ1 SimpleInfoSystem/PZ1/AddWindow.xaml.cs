using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace PZ1
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {

        public AddWindow()
        {
            InitializeComponent();

            YearComboBox.ItemsSource = Classes.Constants.YearsList;
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {

            Classes.Kalashnikov novi;

            if (Validate())
            {
                novi = new Classes.Kalashnikov(VendorTextBox.Text, 
                                               ModelTextBox.Text,
                                               (int) YearComboBox.SelectedItem,
                                               DescriptionTextBox.Text,
                                               ImagePathContainerTextBox.Text);
                MainWindow.Kalashnikovs.Add(novi);
                this.Close();
            }
            else
            {
                MessageBox.Show("Entered data are not valid!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool Validate()
        {
            bool result = true;

            if (ModelTextBox.Text.Trim().Equals(""))
            {
                result = false;
                ModelTextBox.BorderBrush = Brushes.Red;
                ModelTextBox.BorderThickness = new Thickness(1);
                ModelErrorLabel.Content = "This field cannot be empty!";

            }
            else if(IsLeadingNumber(ModelTextBox.Text))
            {
                result = false;
                ModelTextBox.BorderBrush = Brushes.Red;
                ModelTextBox.BorderThickness = new Thickness(1);
                ModelErrorLabel.Content = "This field cannot start with a number!";
            }
            else if (HasPunctuations(ModelTextBox.Text))
            {
                result = false;
                ModelTextBox.BorderBrush = Brushes.Red;
                ModelTextBox.BorderThickness = new Thickness(1);
                ModelErrorLabel.Content = "This field cannot contain punctuations!";
            }
            else if (IsEqualModel(ModelTextBox.Text))
            {
                result = false;
                ModelTextBox.BorderBrush = Brushes.Red;
                ModelTextBox.BorderThickness = new Thickness(1);
                ModelErrorLabel.Content = "Model with this name already exists!";
            }
            else
            {
                ModelTextBox.BorderBrush = Brushes.Green;
                ModelErrorLabel.Content = string.Empty;
            }

            if (DescriptionTextBox.Text.Trim().Equals(""))
            {
                result = false;
                DescriptionTextBox.BorderBrush = Brushes.Red;
                DescriptionTextBox.BorderThickness = new Thickness(1);
                DescriptionErrorLabel.Content = "This field cannot be empty!";
            }
            else if (DescriptionTextBox.Text.Length < 20)
            {
                result = false;
                DescriptionTextBox.BorderBrush = Brushes.Red;
                DescriptionTextBox.BorderThickness = new Thickness(1);
                DescriptionErrorLabel.Content = "Description has to be at least 20 characters long!";
            }
            else
            {
                DescriptionTextBox.BorderBrush = Brushes.Green;
                DescriptionErrorLabel.Content = string.Empty;
            }

            if (YearComboBox.SelectedItem == null)
            {
                result = false;
                YearComboBox.BorderBrush = Brushes.Red;
                YearComboBox.BorderThickness = new Thickness(1);
                YearErrorLabel.Content = "You have to choose an option!";
            }
            else
            {
                YearComboBox.BorderBrush = Brushes.Green;
                YearErrorLabel.Content = string.Empty;
            }

            if (ImagePathContainerTextBox.Text.Trim().Equals(""))
            {
                result = false;
                ImageBorder.BorderBrush = Brushes.Red;
                BrowseImageErrorLabel.Content = "You have to choose an image!";
            }
            else
            {
                ImageBorder.BorderBrush = Brushes.Green;
                BrowseImageErrorLabel.Content = string.Empty;
            }

            return result;
        }

        private bool HasPunctuations(string input)
        {
            bool result = input.IndexOfAny("[](){}*,:=;...#".ToCharArray()) != -1;  //returns false if there is a punctuation in an array

            return result;
        }

        private bool IsLeadingNumber(string input)
        {
            char[] chars = input.ToCharArray();
            bool result = false;

            if (Char.IsDigit(chars[0]))
            {
                result = true;
            }
          
            return result;
        }

        private bool IsEqualModel(string input)
        {
            bool result = false;

            foreach (var kal in MainWindow.Kalashnikovs)
            {
                if (kal.Model == ModelTextBox.Text)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void ButtonBrowse_OnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Title = "Select an image";
            dialog.DefaultExt = ".png";
            dialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.gif|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "PNG (*.png)|*.png|" + 
                        "GIF (*.gif)|*.gif";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string absoluteFileName = dialog.FileName;
                //if (absoluteFileName.Contains("Images"))
                //{
                    string folder = "\\Images";
                    int slashLastIndex = absoluteFileName.LastIndexOf('\\');                  // izvlacimo index poslednjeg '\' kako bismo izdvojili samo ime fajla
                    string relativeFileName = absoluteFileName.Substring(slashLastIndex);

                    ImagePathContainerTextBox.Text = folder + relativeFileName;

                    var uri = new Uri(folder + relativeFileName, UriKind.Relative);
                    SelectedImage.Source = new BitmapImage(uri);
                //}
                //else
                //{
                //    int slashLastIndex = absoluteFileName.LastIndexOf('\\');
                //    string relativeFileName = absoluteFileName.Substring(slashLastIndex);

                //    // odrediti putanju da bude "Images" folder u direktorijumu solutiona (morali smo se vratiti nazad)
                //    string destFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Images"));


                //    if (!System.IO.Directory.Exists(destFilePath))                            // ako ne postoji ovaj direktorijum, dodati ga
                //    {
                //        System.IO.Directory.CreateDirectory(destFilePath);
                //    }

                //    System.IO.File.Copy(absoluteFileName, destFilePath + relativeFileName);   // kopiramo fajl


                //    // za upis u xaml elemente
                //    string folder = "\\Images";

                //    ImagePathContainerTextBox.Text = folder + relativeFileName;

                //    var uri = new Uri(folder + relativeFileName, UriKind.Relative);
                //    //var uri = new Uri(destFilePath + relativeFileName);
                //    SelectedImage.Source = new BitmapImage(uri);
                //}
            }
        }
    }
}
