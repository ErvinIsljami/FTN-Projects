using HCI_MapaManifestacijaGrada.Controller;
using HCI_MapaManifestacijaGrada.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace HCI_MapaManifestacijaGrada.Tutorial
{
    /// <summary>
    /// Interaction logic for Tutorijal.xaml
    /// </summary>
    /// 

    public partial class Tutorijal : Window, INotifyPropertyChanged
    {
        public List<Tag> tutEtikete = new List<Tag>();
        public List<ManifestationType> tutTip = new List<ManifestationType>();
        ManifestationCtrl manifestationCtrl = new ManifestationCtrl();
        ManifestationTypeCtrl manifestationTypeCtrl = new ManifestationTypeCtrl();
        TagCtrl tagCtrl = new TagCtrl();

        public event PropertyChangedEventHandler PropertyChanged;

        OpenFileDialog ofd = new OpenFileDialog();

        //validacija
        private double pomBroj1;
        private string pomString1;
        private string pomString2;
        private string pomString3;
        private string pomString4;
        private string pomString5;
        private string pomString6;
        private string pomString7;
        private ManifestationType pomTip = new ManifestationType();

        private int trenutnoStanje = 0;
        OpenFileDialog op;

        private ObservableCollection<ManifestationType> typeList = new ObservableCollection<ManifestationType>();
        private ObservableCollection<Tag> tagList = new ObservableCollection<Tag>();


        public Tutorijal()
        {
            InitializeComponent();
            this.DataContext = this;
            InitializeManifestationTypeCB();
            InitializeTagCB();

            trenutnoStanje = 0;

            this.sveUgasi();
        }

        

        public void sveUgasi()
        {
            Console.WriteLine("USAO U SVEUGASI");
            this.tbName.IsEnabled = false;
            this.tbDescription.IsEnabled = false;
            this.cbType.IsEnabled = false;
            this.cbTag.IsEnabled = false;
            this.cbAlcohol.IsEnabled = false;
            this.btnUcitaj.IsEnabled = false;
            this.rbHendiNo.IsEnabled = false;
            this.rbHendiYes.IsEnabled = false;
            this.rbInsideNo.IsEnabled = false;
            this.rbInsadeYes.IsEnabled = false;
            this.rbSmokingNo.IsEnabled = false;
            this.rbSmokingYes.IsEnabled = false;
            this.dpDateOfMnfst.IsEnabled = false;
            this.cbPrices.IsEnabled = false;
            this.tbPublic.IsEnabled = false;
            this.sledeceStanje(this.trenutnoStanje);
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

      

        public void sledeceStanje(int stanje)
        {
            Console.WriteLine("USAO U SLEDECASTANJA");
            switch (stanje)
            {
                case 0:
                    this.btnDalje.IsEnabled = false;
                    break;
                case 1:
                    this.btnDalje.IsEnabled = false;
                    this.tbId.IsEnabled = false;
                    this.tbName.IsEnabled = true;
                    break;
                case 2:
                    this.btnDalje.IsEnabled = false;
                    this.tbName.IsEnabled = false;
                    this.tbDescription.IsEnabled = true;
                    break;
                case 3:
                    this.btnDalje.IsEnabled = true;
                    this.tbDescription.IsEnabled = false;
                    this.cbType.IsEnabled = true;
                    break;
                case 4:
                    this.btnDalje.IsEnabled = true;
                    this.cbType.IsEnabled = false;
                    this.cbTag.IsEnabled = true;
                    break;
                case 5:
                    this.btnDalje.IsEnabled = true;
                    this.cbTag.IsEnabled = false;
                    this.cbAlcohol.IsEnabled = true;
                    break;
                case 6:
                    this.btnDalje.IsEnabled = true;
                    this.cbAlcohol.IsEnabled = false;
                    btnUcitaj.IsEnabled = true;
                    break;
                case 7:
                    this.btnDalje.IsEnabled = false;
                    btnUcitaj.IsEnabled = false;
                    this.rbHendiNo.IsEnabled = true;
                    this.rbHendiYes.IsEnabled = true;
                    break;
                case 8:
                    this.btnDalje.IsEnabled = false;
                    this.rbHendiYes.IsEnabled = false;
                    this.rbHendiNo.IsEnabled = false;
                    this.rbSmokingNo.IsEnabled = true;
                    this.rbSmokingYes.IsEnabled = true;
                    break;

                case 9:
                    this.btnDalje.IsEnabled = false;
                    this.rbInsideNo.IsEnabled = true;
                    this.rbInsadeYes.IsEnabled = true;
                    this.rbSmokingNo.IsEnabled = false;
                    this.rbSmokingYes.IsEnabled = false;
                    break;
                case 10:
                    this.btnDalje.IsEnabled = false;
                    this.rbInsideNo.IsEnabled = false;
                    this.rbInsadeYes.IsEnabled = false;
                    this.cbPrices.IsEnabled = true;
                    break;
                case 11:
                    this.btnDalje.IsEnabled = true;
                    this.cbPrices.IsEnabled = false;
                    this.tbPublic.IsEnabled = true;
                    break;
                case 12:
                    this.btnDalje.IsEnabled = true;
                    this.tbPublic.IsEnabled = false;
                    this.dpDateOfMnfst.IsEnabled = true;
                    this.btnZavrsi.IsEnabled = true;
                    this.btnOtkazi.IsEnabled = false;
                    break;
            }



        }

        public void stanje0()
        {
            Console.WriteLine("USAO U STANJE0");
            BindingExpression be1 = this.tbId.GetBindingExpression(TextBox.TextProperty);
            be1.UpdateSource();

            if (be1.HasError || String.IsNullOrEmpty(pomString1) || String.IsNullOrWhiteSpace(pomString1))
            {
                this.btnDalje.IsEnabled = false;
            }
            else
            {
                this.btnDalje.IsEnabled = true;
                Console.WriteLine("jcfyguhirdtfgujj");
            }
        }

        public void stanje1()
        {
            Console.WriteLine("USAO U STANJE1");
            BindingExpression be1 = this.tbName.GetBindingExpression(TextBox.TextProperty);
            be1.UpdateSource();
            if (be1.HasError)
            {
                this.btnDalje.IsEnabled = false;
            }
            else
            {
                this.btnDalje.IsEnabled = true;
            }
        }

        public void stanje2()
        {
            BindingExpression be1 = this.cbType.GetBindingExpression(ComboBox.SelectedItemProperty);
            be1.UpdateSource();
            if (be1.HasError)
            {
                this.btnDalje.IsEnabled = false;
            }
            else
            {
                this.btnDalje.IsEnabled = true;
            }
        }

        public void stanje3()
        {
            BindingExpression be1 = this.tbDescription.GetBindingExpression(TextBox.TextProperty);
            be1.UpdateSource();
            if (be1.HasError || String.IsNullOrEmpty(pomString1) || String.IsNullOrWhiteSpace(pomString1))
            {
                this.btnDalje.IsEnabled = true;
            }
            else
            {
                this.btnDalje.IsEnabled = true;
            }
        }

        public void stanje10()
        {
            BindingExpression be1 = this.cbPrices.GetBindingExpression(TextBox.TextProperty);
            if (be1.HasError)
            {
                this.btnDalje.IsEnabled = false;
            }
            else
            {
                this.btnDalje.IsEnabled = true;
            }
        }

        public void stanje11()
        {
            BindingExpression be1 = this.tbPublic.GetBindingExpression(ComboBox.TextProperty);
            if (be1.HasError)
            {
                this.btnDalje.IsEnabled = false;
            }
            else
            {
                this.btnDalje.IsEnabled = true;
            }
        }

        public void stanje5()
        {
            BindingExpression be1 = this.cbAlcohol.GetBindingExpression(TextBox.TextProperty);
            if (be1.HasError)
            {
                this.btnDalje.IsEnabled = false;
            }
            else
            {
                this.btnDalje.IsEnabled = true;
            }
        }

        public double PomBroj1
        {
            get { return pomBroj1; }
            set
            {
                if (value != pomBroj1)
                {
                    pomBroj1 = value;
                    OnPropertyChanged("PomBroj1");
                    stanje11();
                }
            }
        }

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public string PomString1
        {
            get { return pomString1; }
            set
            {
                if (value != pomString1)
                {
                    pomString1 = value;
                    OnPropertyChanged("PomString1");
                    Console.WriteLine(this.tbId.Text);
                    stanje0();
                }
            }
        }

        public string PomString2
        {
            get { return pomString2; }
            set
            {
                if (value != pomString2)
                {
                    pomString2 = value;
                    OnPropertyChanged("PomString2");
                    stanje1();
                }
            }
        }

        public string PomString3
        {
            get { return pomString3; }
            set
            {
                if (value != pomString3)
                {
                    pomString3 = value;
                    OnPropertyChanged("PomString3");
                    stanje10();
                }
            }
        }

        public string PomString4
        {
            get { return pomString4; }
            set
            {
                if (value != pomString4)
                {
                    pomString4 = value;
                    OnPropertyChanged("PomString4");
                    stanje5();
                }
            }
        }

        public string PomString5
        {
            
            get { return pomString5; }
            set
            {
                if (value != pomString5)
                {
                    pomString5 = value;
                    OnPropertyChanged("PomString5");
                    stanje3();
                }
            }
        }

        public string PomString6
        {
            get { return pomString6; }
            set
            {
                if (value != pomString6)
                {
                    pomString6 = value;
                    OnPropertyChanged("PomString6");
                }
            }
        }

        public string PomString7
        {
            get { return pomString7; }
            set
            {
                if (value != pomString7)
                {
                    pomString7 = value;
                    OnPropertyChanged("PomString7");
                }
            }
        }

        public ManifestationType PomTip
        {
            get { return pomTip; }
            set
            {
                if (value != pomTip)
                {
                    pomTip = value;
                    OnPropertyChanged("PomTip");
                    stanje2();
                }
            }
        }

        private void manifestacijaOznaka_KeyUp(object sender, KeyEventArgs e)
        {

            BindingExpression be1 = this.tbId.GetBindingExpression(TextBox.TextProperty);
            be1.UpdateSource();

            if (be1.HasError || String.IsNullOrEmpty(pomString1) || String.IsNullOrWhiteSpace(pomString1))
            {
                this.btnDalje.IsEnabled = false;
            }
            else
            {
                this.btnDalje.IsEnabled = true;
            }
        }

        private void manifestacijaIme_KeyUp(object sender, KeyEventArgs e)
        {

            BindingExpression be1 = this.tbName.GetBindingExpression(TextBox.TextProperty);
            be1.UpdateSource();

            if (be1.HasError || String.IsNullOrEmpty(pomString2) || String.IsNullOrWhiteSpace(pomString2))
            {
                this.btnDalje.IsEnabled = false;
            }
            else
            {
                this.btnDalje.IsEnabled = true;
            }
        }

        private void BtnDalje_Click(object sender, RoutedEventArgs e)
        {
            trenutnoStanje++;
            sledeceStanje(trenutnoStanje);
        }

        private void BtnZavrsi_Click(object sender, RoutedEventArgs e)
        {
            ZavrsenTutorijal zt = new ZavrsenTutorijal();
            zt.ShowDialog();
            this.Close();
        }

        private void BtnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnUcitaj_Click(object sender, RoutedEventArgs e)
        {
            op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                img.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

    }
}
