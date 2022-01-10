using HCI_MapaManifestacijaGrada.Controller;
using HCI_MapaManifestacijaGrada.Model;
using HCI_MapaManifestacijaGrada.MyHelp;
using Microsoft.Win32;
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

namespace HCI_MapaManifestacijaGrada.View
{
    /// <summary>
    /// Interaction logic for AddManifestation.xaml
    /// </summary>
    public partial class AddManifestation : Window
    {
        public MainWindow parent1;
        ManifestationCtrl manifestationCtrl = new ManifestationCtrl();
        ManifestationTypeCtrl manifestationTypeCtrl = new ManifestationTypeCtrl();
        TagCtrl tagCtrl = new TagCtrl();
        
        bool isNew = true;
        OpenFileDialog op;

        private ObservableCollection<ManifestationType> typeList = new ObservableCollection<ManifestationType>();
        private ObservableCollection<Tag> tagList = new ObservableCollection<Tag>();

        public ObservableCollection<ManifestationType> TypeList
        {
            get { return typeList; }
            set { typeList = value; }
        }

        public ObservableCollection<Tag> TagList
        {
            get { return tagList; }
            set { tagList = value; }
        }

        public AddManifestation()
        {
            InitializeComponent();
            InitializeManifestationTypeCB();
            InitializeTagCB();
        }

        public AddManifestation(Manifestation manifestation)
        {
            InitializeComponent();
            InitializeManifestationTypeCB();
            InitializeTagCB();
            SetManifestationValues(manifestation);
        }

        private void InitializeManifestationTypeCB()
        {
            typeList = new ObservableCollection<ManifestationType>(manifestationTypeCtrl.FindAll());
            foreach (var type in typeList)
            {
                ComboboxItem cbi = new ComboboxItem();
                cbi.Text = type.Ime;
                cbi.Value = type.JedinstvenaOznaka;

                cbType.Items.Add(cbi);
            }
        }

        private void InitializeTagCB()
        {
            tagList = new ObservableCollection<Tag>(tagCtrl.FindAll());
            foreach (var tag in tagList)
            {
                ComboboxItem cbi = new ComboboxItem();
                cbi.Text = tag.JedinstvenaOznaka;
                cbi.Value = tag.JedinstvenaOznaka;

                cbTag.Items.Add(cbi);
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
            else if (cbTag.SelectedIndex == -1)
            {
                isFormValid = false;
                MessageBox.Show("Morate selektovati etiketu!", "Upozorenje",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (cbType.SelectedIndex == -1)
            {
                isFormValid = false;
                MessageBox.Show("Morate selektovati tip!", "Upozorenje",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                isFormValid = true;
            }

            if (isFormValid)
            {
                ManifestationType selectedManifestationType = manifestationTypeCtrl.FindById((cbType.SelectedItem as ComboboxItem).Value.ToString());
                Tag selectedTag = tagCtrl.FindById((cbTag.SelectedItem as ComboboxItem).Value.ToString());

				// If manifestation image is not picked set the default ManifestationType image
				string manifestationImage = "";
				if (op != null)
				{
					if (!System.IO.File.Exists(@"../../Resources/" + op.SafeFileName))
					{
						System.IO.File.Copy(op.FileName, @"../../Resources/" + op.SafeFileName, true);
					}
				}
				if (image.Source == null)
				{
					manifestationImage = selectedManifestationType.ImeIkonice;
				}
				else
				{
					manifestationImage = op.SafeFileName;
				}
				///////////////////////////////////////////////////////////////////////////////

				Manifestation manifestation = new Manifestation(this.tbId.Text, this.tbName.Text, this.tbDescription.Text, this.cbAlcohol.Text,
																manifestationImage, selectedManifestationType, selectedTag,
                                                                (bool)rbHendiYes.IsChecked, (bool)rbSmokingYes.IsChecked,
                                                                (bool)rbInsadeYes.IsChecked, this.cbPrices.Text, this.tbPublic.Text, this.dpDateOfMnfst.Text);

                if (isNew)
                {
                    if (!manifestationCtrl.Save(manifestation))
                    {
                        MessageBox.Show("Tip manifestacije sa oznakom " + manifestation.JedinstvenaOznaka + " vec postoji u sistemu.", "Upozorenje",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    manifestationCtrl.Change(manifestation);
                }

                this.Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_BrowseButton1_Click(object sender, RoutedEventArgs e)
        {
            op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                image.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void SetManifestationValues(Manifestation manifestation)
        {
            op = new OpenFileDialog();
            this.tbId.Text = manifestation.JedinstvenaOznaka;
            this.tbId.IsEnabled = false;
            this.tbName.Text = manifestation.Ime;
            this.tbDescription.Text = manifestation.Opis;
            this.cbTag.Text = manifestation.Etiketa.JedinstvenaOznaka;
            this.cbType.Text = manifestation.Tip.Ime;
            this.cbAlcohol.Text = manifestation.Alkohol;
            if (manifestation.Hendikepirani)
            {
                this.rbHendiYes.IsChecked = true;
            }
            else
            {
                this.rbHendiNo.IsChecked = true;
            }
            if (manifestation.Pušenje)
            {
                this.rbSmokingYes.IsChecked = true;
            }
            else
            {
                this.rbSmokingNo.IsChecked = true;
            }
            if (manifestation.Unutra)
            {
                this.rbInsadeYes.IsChecked = true;
            }
            else
            {
                this.rbInsideNo.IsChecked = true;
            }
            this.cbPrices.Text = manifestation.Cena;
            this.tbPublic.Text = manifestation.Publika;
            this.image.Source = new BitmapImage(new Uri(manifestation.Ikona, UriKind.Relative));
            op.FileName = manifestation.Ikona;
            this.dpDateOfMnfst.Text = manifestation.Datum;

            isNew = false;
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpViewer hh = new HelpViewer("NovaManifestacija", parent1);
            hh.Show();

        }
    }
}
