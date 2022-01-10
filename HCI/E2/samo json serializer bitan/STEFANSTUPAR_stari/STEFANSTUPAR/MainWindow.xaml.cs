using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Newtonsoft.Json;
using STEFANSTUPAR.Models;

namespace STEFANSTUPAR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<String> naziviEtiketa = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        private void DodajLokal_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new pageDodajLokal();
        }

        private void DodajEtiketu_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new pageDodajEtiketu();
        }

        private void DodajTip_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new pageDodajTip();
        }

        private void PregledLokala_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new pagePregledLokala();
        }

        private void PregledEtiketa_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new pagePregledEtiketa();
        }

        private void PregledTipova_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = new pagePregledTipova();
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter file = File.CreateText(@"C:\AAStefan\Fakultet\Semestar_06\HCI\STEFANSTUPAR\Lokali.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Lokal.lokali);
            }
            using (StreamWriter file = File.CreateText(@"C:\AAStefan\Fakultet\Semestar_06\HCI\STEFANSTUPAR\Etikete.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Etiketa.etikete);
            }
            using (StreamWriter file = File.CreateText(@"C:\AAStefan\Fakultet\Semestar_06\HCI\STEFANSTUPAR\Tip.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Tip.tipovi);
            }
        }

        private void Ucitaj_Click(object sender, RoutedEventArgs e)
        {
            using (StreamReader r = new StreamReader(@"C:\AAStefan\Fakultet\Semestar_06\HCI\STEFANSTUPAR\Lokali.txt"))
            {
                string json = r.ReadToEnd();
                Lokal.lokali = JsonConvert.DeserializeObject<ObservableCollection<Lokal>>(json);
            }
            using (StreamReader r = new StreamReader(@"C:\AAStefan\Fakultet\Semestar_06\HCI\STEFANSTUPAR\Etikete.txt"))
            {
                string json = r.ReadToEnd();
                Etiketa.etikete = JsonConvert.DeserializeObject<ObservableCollection<Etiketa>>(json);
            }
            using (StreamReader r = new StreamReader(@"C:\AAStefan\Fakultet\Semestar_06\HCI\STEFANSTUPAR\Tip.txt"))
            {
                string json = r.ReadToEnd();
                Tip.tipovi = JsonConvert.DeserializeObject<ObservableCollection<Tip>>(json);
            }
        }
    }
}
