using HCI_MapaManifestacijaGrada.Controller;
using HCI_MapaManifestacijaGrada.Model;
using HCI_MapaManifestacijaGrada.MyHelp;
using Microsoft.Win32;
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

namespace HCI_MapaManifestacijaGrada.View
{
    /// <summary>
    /// Interaction logic for AddManifestationType.xaml
    /// </summary>
    public partial class AddManifestationType : Window
    {
        public MainWindow parent1;
        ManifestationTypeCtrl manifestationTypeCtrl = new ManifestationTypeCtrl();
		ManifestationCtrl manifestationCtrl = new ManifestationCtrl();
        bool isNew = true;
        OpenFileDialog op;

        public AddManifestationType()
        {
            InitializeComponent();
        }

        public AddManifestationType(ManifestationType manifestationType)
        {
            InitializeComponent();
            SetManifestationTypeValues(manifestationType);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                img.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            var isFormValid = true;


            if (tbId.Text == "")
            {
                isFormValid = false;
                MessageBox.Show("Morate uneti oznaku!", "Upozorenje",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (img.Source == null)
            {
                isFormValid = false;
                MessageBox.Show("Morate izabrati ikonicu!", "Upozorenje",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                isFormValid = true;
            }

            

            if (isFormValid)
            {
                if (!System.IO.File.Exists("../../Resources//" + op.SafeFileName))
                {
                    System.IO.File.Copy(op.FileName, @"../../Resources/" + op.SafeFileName, true);
                }

                ManifestationType manifestationType = new ManifestationType(this.tbId.Text, this.tbName.Text, this.tbDescription.Text, "../../Resources/" + op.SafeFileName, op.SafeFileName);

                if (isNew)
                {
                    if (!manifestationTypeCtrl.Save(manifestationType))
                    {
                        MessageBox.Show("Tip manifestacije sa oznakom " + manifestationType.JedinstvenaOznaka + " vec postoji u sistemu.", "Upozorenje",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
					manifestationTypeCtrl.Change(manifestationType);
				}

                this.Close();
            }
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            

            HelpViewer hh = new HelpViewer("NovaEtiketa", parent1);
            hh.Show();
        }

        private void SetManifestationTypeValues(ManifestationType manifestationType)
        {
            op = new OpenFileDialog();
            this.tbId.Text = manifestationType.JedinstvenaOznaka;
            this.tbId.IsEnabled = false;
            this.tbName.Text = manifestationType.Ime;
            this.tbDescription.Text = manifestationType.Opis;
            this.img.Source = new BitmapImage(new Uri(manifestationType.Ikona, UriKind.RelativeOrAbsolute));
            op.FileName = manifestationType.Ikona;

            isNew = false;
        }
    }
}
