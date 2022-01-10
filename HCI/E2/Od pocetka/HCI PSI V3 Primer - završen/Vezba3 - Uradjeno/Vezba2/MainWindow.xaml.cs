using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vezba2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //objekat klase pomocu kojeg ce se raditi sa fajlovima (upis/iscitavanje)
        private Classes.DataIO serializer = new Classes.DataIO();
        //BindingLista - Observable kolekcija koju koristimo sa idejom da se obavlja automatsko azuriranje
        public static BindingList<Classes.Student> Studenti { get; set; }
 
        public MainWindow()
        {
            //Iscitavamo iz fajla objekat tipa BindingLista koji zelimo da nam se prikaze u tabeli (njen je ItemsSource)
            Studenti = serializer.DeSerializeObject<BindingList<Classes.Student>>("studenti.xml");
            if(Studenti == null) //U slucaju da nista nije ucitano
            {
                Studenti = new BindingList<Classes.Student>(); //nova lista pa cemo u nju dodavati
            }
            DataContext = this; //okidac Data Bindinga
            InitializeComponent();
        }

        private void buttonIzlaz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonDodavanje_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWin = new AddWindow();
            addWin.ShowDialog();
        }

        private void buttonObrisi_Click(object sender, RoutedEventArgs e)
        {
            if(Studenti.Count>0)
            {
                Studenti.RemoveAt(Studenti.Count - 1);
                //Studenti.RemoveAt(dataGridStudenti.SelectedIndex);  -za brisanje selektovanog u tabeli
            }
        }

        private void save(object sender, CancelEventArgs e)
        {
            //cuvanje u fajl objekta klase BindingList - nasa lista studenata
            serializer.SerializeObject<BindingList<Classes.Student>>(Studenti, "studenti.xml");
        }
    }
}
