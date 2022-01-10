using PR101_2015.Commands;
using PR101_2015.Model;
using PR101_2015.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace PR101_2015.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public static BindingList<Reaktor> Reaktori { get; set; } //povezna lista //za tabelu
        public static BindingList<Reaktor> ReaktoriListView { get; set; } //za listview posto ne treba da nestane iz tabele
        public static BindingList<Reaktor> ReaktoriFilterLista { get; set; } //za filter posebno da ne bi izgubili prave podatke prilikom filriranja
        public static int br = 0; //brojac reaktora u rijecniku
        public static List<string> tipovi = new List<string>() { "FUZIONI", "TERMALNI", "TIP3" };
        public static DateTime datum = new DateTime();

        public static Dictionary<int, Reaktor> ReakDiksn = new Dictionary<int, Reaktor>();  //zbog prihvatanja objekata iz servera,key=rb servera
        public static Dictionary<string, Reaktor> CanvasiDiksn = new Dictionary<string, Reaktor>();      // key = ime canvasa u koji je dodat objekat
        public static Dictionary<string, Canvas> aktuelniCanvas = new Dictionary<string, Canvas>();    // key = ime canvasa, a vrijednost sam taj Canvas    


        //FileStream fileStream = new FileStream("report.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

        public static Reaktor aktuelni_reak = new Reaktor(); //aktuelni za obradu
        public static int trenutno = 0; //tr stanje aktuelnog reaktora
        public static Reaktor o;

        public static Picture draggedItem = null; //objekat koji se draguje
        public static bool dragging = false;   //logicka prom

        public MainViewModel()
        {

            OslobodiCommand = new OslobodiCommand(this);
            FilterCommand = new FilterCommand(this);
            TableBeforeCommand = new TableBeforeCommand(this);
            AddGraphCommand = new AddGraphCommand(this);
            AddReaktorCommand = new AddReaktorCommand(this);
            AddReportCommand = new AddReportCommand(this);

            datum = DateTime.Now.Date;

            Reaktori = new BindingList<Reaktor>();
            ReaktoriListView = new BindingList<Reaktor>();
            ReaktoriFilterLista = new BindingList<Reaktor>();

            Reaktor r1 = new Reaktor(0, "r1", "TIP3");
            Reaktor r2 = new Reaktor(1, "r2", "FUZIONI");
            Reaktor r3 = new Reaktor(2, "r3", "TERMALNI");
            Reaktor r4 = new Reaktor(3, "gaga", "TERMALNI");

            Reaktori.Add(r1);
            Reaktori.Add(r2);
            Reaktori.Add(r3);
            Reaktori.Add(r4);
            ReaktoriListView.Add(r1);
            ReaktoriListView.Add(r2);
            ReaktoriListView.Add(r3);
            ReaktoriListView.Add(r4);
            ReaktoriFilterLista.Add(r1);
            ReaktoriFilterLista.Add(r2);
            ReaktoriFilterLista.Add(r3);
            ReaktoriFilterLista.Add(r4);
            ReakDiksn.Add(1, r1);
            ReakDiksn.Add(2, r2);
            ReakDiksn.Add(3, r3);
            ReakDiksn.Add(4, r4);
        }

        public Window Window { get; set; }

        public Reaktor ReaktorList { get; set; }
        public string ComboBox1 { get; set; }

        //public TextBox TextBoxValue { get; set; }
        public string TextBoxValue { get; set; }
        public string TextBoxGraphValue { get; set; }

        public string TextBoxReportValue { get; set; }
        public string TextBlockValue { get; set; }

        public Canvas CanvasGraph { get; set; }

        public ICommand OslobodiCommand
        {
            get;
            private set;
        }

        public ICommand AddReportCommand
        {
            get;
            private set;
        }

        public ICommand FilterCommand
        {
            get;
            private set;
        }

        public ICommand AddGraphCommand
        {
            get;
            private set;
        }

        public ICommand TableBeforeCommand
        {
            get;
            private set;
        }
        public ICommand AddReaktorCommand
        {
            get;
            private set;
        }



        public void AddReport()
        {
            for (int i = 0; i < Reaktori.Count; i++) //da bi bili sortirani reaktori 
            {
                string log = "Izvjestaj_" + i + ".txt";
                TextBoxReportValue += "Reaktor " + i + "\n";
                using (StreamReader sr = new StreamReader(log))
                {
                    while (sr.Peek() >= 0)
                    {
                        String line = sr.ReadLine();
                        TextBoxReportValue += line + "\n";
                    }
                }
            }
        }

        public void Oslobodi()
        {
            //Reaktor r = (Reaktor)dataGridReaktori.SelectedItem;
            Reaktor r = ReaktorList;
            bool nasao = false;
            int i = 0;
            foreach (Reaktor re in ReaktoriListView)  //prolazim kroz listu objekata u listview od servera //PROLAZILA KROZ POGRESNU LISTU
            {

                if (re.Id == r.Id)
                {
                    nasao = true;
                    break;
                }
                else
                {
                    nasao = false;

                }
                i++;

            }
            if (nasao)
            {
                ReaktoriFilterLista.RemoveAt(i);
                ReaktoriListView.RemoveAt(i);
                Reaktori.RemoveAt(i);
            }
            else
            {
                MessageBox.Show("Nema u listView pa ne moze da se obrise.");
            }
        }

        public void TableBefore()
        {
            ReaktoriFilterLista.Clear();

            foreach (Reaktor s in Reaktori)
            {
                ReaktoriFilterLista.Add(s);
            }
        }

        public void Filter()
        {
            ReaktoriFilterLista.Clear();                                            // praznjenje lista, tj. tabele

            if (TextBoxValue == String.Empty && ComboBox1 == null)
            {
                MessageBox.Show("Morate popuniti bar jedno od filter polja.");
            }
            else
            {
                if (ComboBox1 != null && TextBoxValue.ToString() != String.Empty)
                {
                    foreach (Reaktor r in Reaktori)                                 // prolazak kroz listu objekata i dodavanje odgovarajucih u tabelu
                    {
                        if (r.Type.Contains(ComboBox1) && r.Name.Contains(TextBoxValue))
                        {
                            ReaktoriFilterLista.Add(r);
                        }
                    }
                }
                else if (ComboBox1 != String.Empty)
                {
                    foreach (Reaktor r in Reaktori)                                 // prolazak kroz listu objekata i dodavanje odgovarajucih u tabelu
                    {
                        if (r.Type.Contains(ComboBox1))
                        {
                            ReaktoriFilterLista.Add(r);
                        }
                    }
                }
                else if (TextBoxValue != String.Empty)
                {
                    foreach (Reaktor r in Reaktori)
                    {
                        if (r.Name.Contains(TextBoxValue))
                        {
                            ReaktoriFilterLista.Add(r);
                        }
                    }
                }
            }

            //TextBoxValue.Clear(); //da ne ostaje ispisano to po cemu smo filtrirali
        }

        public void AddReaktor()
        {
            Window w = new AddReaktorWindow();
            w.DataContext = new AddReaktorViewModel() { Window = w};
            w.ShowDialog();
        }

        public void AddGraph()
        {

            CanvasGraph.Children.Clear();  //ocistimo sve sto je bilo

            List<int> mjerenja = new List<int>();                        // mjerenja ucitana iz log fajla
            int brMjerenja = 0;

            using (StreamReader sr = File.OpenText("report.txt"))
            {
                while (sr.Peek() > 0)
                {
                    string s = sr.ReadLine();                       // parsiranje log fajla
                    string[] deo = s.Split('_', ':', '|');

                    int rbr = Int32.Parse(deo[1]);                  // id objekta
                    int stanje = Int32.Parse(deo[3]);               // trenutno stanje objekta

                    if (Int32.Parse(TextBoxGraphValue) == rbr)     //iscrtava samo za odredjeni objekat
                    {
                        mjerenja.Add(stanje);                            // dodavanje procitane vrijednosti iz log fajla
                        brMjerenja++;
                    }
                }


                double width = CanvasGraph.Width;   //sirina kanvasa u kom se nalazi graf
                double widthBar = 20; //sirinabara 20
                double mjesto = 30; //u odnosu na pocetak canvasa,dole lijevo //pozicija

                Line x_osa = new Line();                                // crtam x osu
                x_osa.X1 = 30;  //OK 30
                x_osa.X2 = width - 50;  //OK 50
                x_osa.Y1 = 398;    //350
                x_osa.Y2 = 398;   //350  gleda odozgo
                x_osa.Stroke = Brushes.Black; //koje boje je linija
                x_osa.StrokeThickness = 2;
                CanvasGraph.Children.Add(x_osa);  //dodajemo canvasu children X osu

                Line y_osa = new Line();                                // crtam  y osu
                y_osa.X1 = 30; //OK
                y_osa.X2 = 30;
                y_osa.Y1 = 398;  //350
                y_osa.Y2 = 0;
                y_osa.Stroke = Brushes.Black;
                y_osa.StrokeThickness = 2;
                CanvasGraph.Children.Add(y_osa);
                //Zavrseno crtanje osa,sada crtamo podoke

                double stepX = (width - 82) / 24;   //OK                  // crtanje podioka na x osi   
                for (int i = 1; i <= 24; i++)                           //24h pa zato 24 podeoka
                {
                    Line podiok = new Line();
                    podiok.X1 += x_osa.X1 + i * stepX;
                    podiok.X2 += x_osa.X1 + i * stepX;
                    podiok.Y1 = 400;
                    podiok.Y2 = 396;
                    podiok.Stroke = Brushes.Black;
                    podiok.StrokeThickness = 3;
                    CanvasGraph.Children.Add(podiok);

                    TextBlock brojUzPodeok = new TextBlock();           // broj koji pise uz podiok
                    brojUzPodeok.Text = i.ToString();
                    brojUzPodeok.Foreground = Brushes.Black;
                    CanvasGraph.Children.Add(brojUzPodeok);
                    Canvas.SetLeft(brojUzPodeok, podiok.X1); //udaljenost elementa od lijeve strane njegovog roditelja Canvasa
                    Canvas.SetBottom(brojUzPodeok, 1);      //udaljenost elementa od dna njegovog roditelja Canvasa
                }

                double stepY = 10;               //OK          // 400 je maksimum, hocu deset podeoka, znaci korak je 40
                for (int i = 1; i <= 40; i++)               //hocu 10 podeoka na Y osi
                {
                    Line podiok = new Line();
                    podiok.X1 = 33;             //OK za - linijicu podeoka
                    podiok.X2 = 28;             //OK za - linijicu podeoka
                    podiok.Y1 += y_osa.Y1 - i * stepY; //- posto ide odozgo
                    podiok.Y2 += y_osa.Y1 - i * stepY;
                    podiok.Stroke = Brushes.Black;
                    podiok.StrokeThickness = 3;
                    CanvasGraph.Children.Add(podiok);

                    TextBlock broj = new TextBlock();       // broj koji se pise uz podeok
                    broj.Text = (i * 10).ToString();        //da prikazuje 10,20,30..
                    broj.Foreground = Brushes.Black;
                    CanvasGraph.Children.Add(broj);
                    Canvas.SetLeft(broj, 3);
                    Canvas.SetTop(broj, podiok.Y1 - 8);
                }

                if (brMjerenja <= 100)   // maskimalno se prikazuje 100 merenja, ako ih ima vise, onda prikazujem poslednjih 35
                {
                    foreach (int m in mjerenja)
                    {
                        Rectangle bar = new Rectangle();                            // za svako merenje potrebno je napraviti jedan bar
                        bar.Width = widthBar;                                     // definisanje sirine widthBar
                        bar.Height = m - 37;                                           // definisanje visine (-37 da bi se ljepse uklopilo u visinu prozora)

                        if (m < 250 || m > 350)                // ako je vrijednost izvan opsega bar je ljubicast, ako je dobra onda je bijel
                        {
                            bar.Fill = Brushes.Purple;
                        }
                        else
                        {
                            bar.Fill = Brushes.Azure;
                        }


                        CanvasGraph.Children.Add(bar);                                    // dodavanje novog djeteta canvasu
                        Canvas.SetBottom(bar, 18);                                   // pozicioniranje na odgovarajuce mjesto na canvasu
                        Canvas.SetLeft(bar, mjesto + 3);
                        mjesto += widthBar + 5;                                 // pomjeranje pozicije za crtanje sledeceg bara
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
