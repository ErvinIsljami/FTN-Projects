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
using Classes;

namespace PZ1
{
    /// <summary>
    /// Interaction logic for ReadWindow.xaml
    /// </summary>
    public partial class ReadWindow : Window
    {
        //public ReadWindow()
        //{
        //    InitializeComponent();
        //}

        public ReadWindow(Classes.Kalashnikov passedKalashnikov)
        {
            InitializeComponent();
            
            var imageUri = new Uri(passedKalashnikov.Image, UriKind.RelativeOrAbsolute);
            ModelImage.Source = new BitmapImage(imageUri);
            ModelLabel.Content = passedKalashnikov.Model;
            ModelDescriptionTextBox.Text = passedKalashnikov.Description;
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
