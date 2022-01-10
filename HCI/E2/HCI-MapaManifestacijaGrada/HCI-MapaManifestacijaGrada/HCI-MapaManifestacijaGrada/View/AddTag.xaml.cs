using HCI_MapaManifestacijaGrada.Controller;
using HCI_MapaManifestacijaGrada.Model;
using HCI_MapaManifestacijaGrada.MyHelp;
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
    /// Interaction logic for AddTag.xaml
    /// </summary>
    public partial class AddTag : Window
    {
        public MainWindow parent1;
        TagCtrl tagCtrl = new TagCtrl();
        bool isNew = true;

        public AddTag()
        {
            InitializeComponent();
        }

        public AddTag(Tag tag)
        {
            InitializeComponent();
            SetTagValues(tag);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void SetTagValues(Tag tag)
        {
            this.tbId.Text = tag.JedinstvenaOznaka;
            this.tbId.IsEnabled = false;
            this.tbDescription.Text = tag.Opis;
            this.ClrPcker_Background.SelectedColor = (Color)ColorConverter.ConvertFromString(tag.Boja); ;

            isNew = false;
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
            else
            {
                isFormValid = true;
            }
            if (isFormValid)
            {
                Tag tag = new Tag(this.tbId.Text, this.ClrPcker_Background.SelectedColorText, this.tbDescription.Text);

                if (isNew)
                {
                    if (!tagCtrl.Save(tag))
                    {
                        MessageBox.Show("Tag sa oznakom " + tag.JedinstvenaOznaka + " vec postoji u sistemu.", "Upozorenje",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    tagCtrl.Change(tag);
                }

                this.Close();
            }
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpViewer hh = new HelpViewer("NoviTip", parent1);
            hh.Show();
        }
    }
}
