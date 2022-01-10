using PR101_2015.Model;
using PR101_2015.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace PR101_2015
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
               
                DataContext = new MainViewModel() { Window = this };
                InitializeComponent();
                comboBoxFilter.ItemsSource = MainViewModel.tipovi;


            //InitializeComponent();
            Start();
            createListener(); //Povezivanje sa serverskom aplikacijom

        }

        void Start() //pozadinska nit provjerava validnost vrijednosti i postavlja warning
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {

                    if (MainViewModel.trenutno < 250 || MainViewModel.trenutno > 350)
                    {    // ako je trenutno opterecenje izvan opsega
                        Greska(MainViewModel.aktuelni_reak.Id);
                    }
                    else
                    {
                        Ok(MainViewModel.aktuelni_reak.Id);
                    }
                    Thread.Sleep(500);
                }
            }).Start();
        }


        public void Greska(int id)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                foreach (KeyValuePair<string, Reaktor> par in MainViewModel.CanvasiDiksn) //Key je ima canvasa u koji je dodat objekat
                {
                    if (id == par.Value.Id) //trazi problematicni(id) u rijecniku canvasa i ako ga nadje postavi sliku na warning
                    {
                        try
                        {
                            BitmapImage upozorenje = new BitmapImage();
                            upozorenje.BeginInit();
                            upozorenje.UriSource = new Uri(@"C:\Users\boske\source\repos\PR101-2015\PR101-2015\slike\Warning.png");
                            upozorenje.EndInit();
                            (MainViewModel.aktuelniCanvas[par.Key]).Background = new ImageBrush(upozorenje);
                        }
                        catch { }
                    }
                }

            }));
        }

        public void Ok(int id)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                foreach (KeyValuePair<string, Reaktor> par in MainViewModel.CanvasiDiksn)
                {
                    if (id == par.Value.Id)
                    {
                        BitmapImage pic = new BitmapImage();
                        pic.BeginInit();
                        pic.UriSource = new Uri(par.Value.ImagePath.ToString());
                        pic.EndInit();
                        (MainViewModel.aktuelniCanvas[par.Key]).Background = new ImageBrush(pic);
                    }
                }
            }));
        }

        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(MainViewModel.Reaktori.Count.ToString());  //OVDJE IDE LIST.COUNT.TOSTRING()
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Objekat_1:272"
                            string[] parts = incomming.Split('_', ':');


                            int index = int.Parse(parts[1]);
                            MainViewModel.trenutno = int.Parse(parts[2]);
                            MainViewModel.aktuelni_reak.Id = (int.Parse(parts[1]));
                            MainViewModel.aktuelni_reak.Values.Add(MainViewModel.trenutno);
                            MainViewModel.aktuelni_reak.Value = MainViewModel.aktuelni_reak.Values[MainViewModel.aktuelni_reak.Values.Count - 1];  // pamcenje trenutnog stanja aktuelnog objekta


                            Console.WriteLine("Objekat {0} -> {1}", MainViewModel.aktuelni_reak.Id, MainViewModel.aktuelni_reak.Value);

                            if (index < MainViewModel.ReaktoriFilterLista.Count)
                            {
                                MainViewModel.ReaktoriFilterLista[index].Value = MainViewModel.aktuelni_reak.Value;
                            }




                            this.Dispatcher.Invoke(() =>
                            {
                                MainViewModel.ReaktoriFilterLista.ResetBindings();
                            });


                            //now = int.Parse(parts[2]);

                            string log = "Izvjestaj_" + index + ".txt";

                            using (StreamWriter sw = File.AppendText(log))
                            {
                                if (DateTime.Now.Date == MainViewModel.datum.Date)
                                {

                                    sw.WriteLine("Stanje:" + parts[2] + '_' + DateTime.Now);
                                }
                            }

                            string report = "report.txt";
                            using (StreamWriter sw = File.AppendText(report))
                            {
                                if (DateTime.Now.Date == MainViewModel.datum.Date)
                                {
                                    sw.WriteLine(String.Format("Objekat_{0}\t| Stanje: {1}\t| Vreme: {2}", parts[1], parts[2], DateTime.Now));
                                }
                            }


                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();


        }

        //private void listView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    draggedItem = null;
        //    listView.SelectedItem = null;
        //    dragging = false;
        //}

        private void dragOver(object sender, DragEventArgs e)
        {
            base.OnDragOver(e);
            if (((Canvas)sender).Resources["taken"] != null)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Copy;
                MainViewModel.ReaktoriListView.Remove(MainViewModel.o);
            }
            e.Handled = true;
        }

        private void drop(object sender, DragEventArgs e)
        {
            base.OnDrop(e);
            if (MainViewModel.draggedItem != null)
            {
                if (((Canvas)sender).Resources["taken"] == null)
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(MainViewModel.draggedItem.imageUri);
                    Console.WriteLine(MainViewModel.draggedItem.imageUri);
                    logo.EndInit();
                    ((Canvas)sender).Background = new ImageBrush(logo);
                    ((TextBlock)((Canvas)sender).Children[0]).Foreground = Brushes.Black;
                    ((TextBlock)((Canvas)sender).Children[0]).Text = MainViewModel.o.Id.ToString();
                    ((TextBlock)((Canvas)sender).Children[2]).Text = MainViewModel.o.Name;
                    ((Canvas)sender).Resources.Add("taken", true);

                }
                listView.SelectedItem = null;
                MainViewModel.dragging = false;
                //ReaktoriListView.Remove(o);
                MainViewModel.CanvasiDiksn.Add(((Canvas)sender).Name, MainViewModel.o);                    // dodavanje objekta u rijecnik kanvasa                             
                MainViewModel.aktuelniCanvas.Add(((Canvas)sender).Name, (Canvas)sender);     // pamcenje u koji canvas je dodat


            }
            e.Handled = true;
        }

        private void oslobodi1(object sender, RoutedEventArgs e)
        {
            if (canvas11.Resources["taken"] != null)
            {
                canvas11.Background = Brushes.Purple;
                ((TextBlock)canvas11.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas11.Children[0]).Text = "Free";
                ((TextBlock)canvas11.Children[2]).Text = String.Empty;
                canvas11.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas11.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas11.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas11.Name); //da bi se mogao drag and drop na isti canvas nakon vracanja u listview
            }
        }

        private void oslobodi2(object sender, RoutedEventArgs e)
        {
            if (canvas12.Resources["taken"] != null)
            {
                canvas12.Background = Brushes.Purple;
                ((TextBlock)canvas12.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas12.Children[0]).Text = "Free";
                ((TextBlock)canvas12.Children[2]).Text = String.Empty;
                canvas12.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas12.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas12.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas12.Name);
            }
        }

        private void oslobodi3(object sender, RoutedEventArgs e)
        {
            if (canvas13.Resources["taken"] != null)
            {
                canvas13.Background = Brushes.Purple;
                ((TextBlock)canvas13.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas13.Children[0]).Text = "Free";
                ((TextBlock)canvas13.Children[2]).Text = String.Empty;
                canvas13.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas13.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas13.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas13.Name);
            }
        }

        private void oslobodi4(object sender, RoutedEventArgs e)
        {
            if (canvas14.Resources["taken"] != null)
            {
                canvas14.Background = Brushes.Purple;
                ((TextBlock)canvas14.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas14.Children[0]).Text = "Free";
                ((TextBlock)canvas14.Children[2]).Text = String.Empty;
                canvas13.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas14.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas14.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas14.Name);
            }
        }

        private void oslobodi5(object sender, RoutedEventArgs e)
        {
            if (canvas21.Resources["taken"] != null)
            {
                canvas21.Background = Brushes.Purple;
                ((TextBlock)canvas21.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas21.Children[0]).Text = "Free";
                ((TextBlock)canvas21.Children[2]).Text = String.Empty;
                canvas21.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas21.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas21.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas21.Name);
            }
        }

        private void oslobodi6(object sender, RoutedEventArgs e)
        {
            if (canvas22.Resources["taken"] != null)
            {
                canvas22.Background = Brushes.Purple;
                ((TextBlock)canvas22.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas22.Children[0]).Text = "Free";
                ((TextBlock)canvas22.Children[2]).Text = String.Empty;
                canvas22.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas22.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas22.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas22.Name);
            }
        }

        private void oslobodi7(object sender, RoutedEventArgs e)
        {
            if (canvas23.Resources["taken"] != null)
            {
                canvas23.Background = Brushes.Purple;
                ((TextBlock)canvas23.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas23.Children[0]).Text = "Free";
                ((TextBlock)canvas23.Children[2]).Text = String.Empty;
                canvas23.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas23.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas23.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas23.Name);
            }

        }

        private void oslobodi8(object sender, RoutedEventArgs e)
        {
            if (canvas24.Resources["taken"] != null)
            {
                canvas24.Background = Brushes.Purple;
                ((TextBlock)canvas24.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas24.Children[0]).Text = "Free";
                ((TextBlock)canvas24.Children[2]).Text = String.Empty;
                canvas24.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas24.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas24.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas24.Name);
            }

        }
        private void oslobodi9(object sender, RoutedEventArgs e)
        {
            if (canvas25.Resources["taken"] != null)
            {
                canvas31.Background = Brushes.Purple;
                ((TextBlock)canvas25.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas25.Children[0]).Text = "Free";
                ((TextBlock)canvas25.Children[2]).Text = String.Empty;
                canvas25.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas25.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas25.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas25.Name);
            }

        }

        private void oslobodi10(object sender, RoutedEventArgs e)
        {
            if (canvas31.Resources["taken"] != null)
            {
                canvas31.Background = Brushes.Purple;
                ((TextBlock)canvas31.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas31.Children[0]).Text = "Free";
                ((TextBlock)canvas31.Children[2]).Text = String.Empty;
                canvas31.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas31.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas31.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas31.Name);
            }

        }

        private void oslobodi11(object sender, RoutedEventArgs e)
        {
            if (canvas32.Resources["taken"] != null)
            {
                canvas32.Background = Brushes.Purple;
                ((TextBlock)canvas32.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas32.Children[0]).Text = "Free";
                ((TextBlock)canvas32.Children[2]).Text = String.Empty;
                canvas32.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas32.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas32.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas32.Name);
            }

        }
        private void oslobodi12(object sender, RoutedEventArgs e)
        {
            if (canvas33.Resources["taken"] != null)
            {
                canvas33.Background = Brushes.Purple;
                ((TextBlock)canvas33.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas33.Children[0]).Text = "Free";
                ((TextBlock)canvas33.Children[2]).Text = String.Empty;
                canvas33.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas33.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas33.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas33.Name);
            }


        }

        private void oslobodi13(object sender, RoutedEventArgs e)
        {
            if (canvas34.Resources["taken"] != null)
            {
                canvas34.Background = Brushes.Purple;
                ((TextBlock)canvas34.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas34.Children[0]).Text = "Free";
                ((TextBlock)canvas34.Children[2]).Text = String.Empty;
                canvas34.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas34.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas34.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas34.Name);
            }

        }

        private void oslobodi14(object sender, RoutedEventArgs e)
        {
            if (canvas41.Resources["taken"] != null)
            {
                canvas41.Background = Brushes.Purple;
                ((TextBlock)canvas41.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas41.Children[0]).Text = "Free";
                ((TextBlock)canvas41.Children[2]).Text = String.Empty;
                canvas41.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas41.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas41.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas41.Name);
            }

        }

        private void oslobodi15(object sender, RoutedEventArgs e)
        {

            if (canvas42.Resources["taken"] != null)
            {
                canvas42.Background = Brushes.Purple;
                ((TextBlock)canvas42.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas42.Children[0]).Text = "Free";
                ((TextBlock)canvas42.Children[2]).Text = String.Empty;
                canvas42.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas42.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas42.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas42.Name);
            }

        }

        private void oslobodi16(object sender, RoutedEventArgs e)
        {
            if (canvas43.Resources["taken"] != null)
            {
                canvas43.Background = Brushes.Purple;
                ((TextBlock)canvas43.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)canvas43.Children[0]).Text = "Free";
                ((TextBlock)canvas43.Children[2]).Text = String.Empty;
                canvas43.Resources.Remove("taken");

                MainViewModel.ReaktoriListView.Add(MainViewModel.CanvasiDiksn[canvas43.Name]);
                MainViewModel.CanvasiDiksn.Remove(canvas43.Name);
                MainViewModel.aktuelniCanvas.Remove(canvas43.Name);
            }

        }



        //private void buttonOslobodi_Click(object sender, RoutedEventArgs e)
        //{
        //    ReaktoriFilterLista.Clear();

        //    foreach (Reaktor s in Reaktori)
        //    {
        //        ReaktoriFilterLista.Add(s);
        //    }
        //}

        //private void button1_Click(object sender, RoutedEventArgs e) //delete
        //{
        //    Reaktor r = (Reaktor)dataGridReaktori.SelectedItem;
        //    bool nasao = false;
        //    int i = 0;
        //    foreach (Reaktor re in ReaktoriListView)  //prolazim kroz listu objekata u listview od servera //PROLAZILA KROZ POGRESNU LISTU
        //    {

        //        if (re.Id == r.Id)
        //        {
        //            nasao = true;
        //            break;
        //        }
        //        else
        //        {
        //            nasao = false;

        //        }
        //        i++;

        //    }
        //    if (nasao)
        //    {
        //       ReaktoriFilterLista.RemoveAt(i);
        //       ReaktoriListView.RemoveAt(i);
        //       Reaktori.RemoveAt(i);
        //    }

        //    else
        //    {
        //        MessageBox.Show("Nema u listView pa ne moze da se obrise.");
        //    }


        //}

        //private void button_Click(object sender, RoutedEventArgs e)
        //{
        //    AddWindow add = new AddWindow();
        //    add.ShowDialog();
        //}

        //private void buttonFF_Click(object sender, RoutedEventArgs e)
        //{
        //    Helper.ReaktoriFilterLista.Clear();                                            // praznjenje lista, tj. tabele

        //    if (TextBoxFilterName.Text.Trim() == String.Empty && comboBoxFilter.SelectedItem == null)
        //    {
        //        MessageBox.Show("Morate popuniti bar jedno od filter polja.");
        //    }
        //    else
        //    {
        //        if (comboBoxFilter.SelectedItem != null && TextBoxFilterName.Text != String.Empty)
        //        {
        //            foreach (Reaktor r in Helper.Reaktori)                                 // prolazak kroz listu objekata i dodavanje odgovarajucih u tabelu
        //            {
        //                if (r.Type.Contains(comboBoxFilter.Text) && r.Name.Contains(TextBoxFilterName.Text))
        //                {
        //                    Helper.ReaktoriFilterLista.Add(r);
        //                }
        //            }
        //        }
        //        else if (comboBoxFilter.Text != String.Empty)
        //        {
        //            foreach (Reaktor r in Helper.Reaktori)                                 // prolazak kroz listu objekata i dodavanje odgovarajucih u tabelu
        //            {
        //                if (r.Type.Contains(comboBoxFilter.Text))
        //                {
        //                    Helper.ReaktoriFilterLista.Add(r);
        //                }
        //            }
        //        }
        //        else if (TextBoxFilterName.Text != String.Empty)
        //        {
        //            foreach (Reaktor r in Helper.Reaktori)
        //            {
        //                if (r.Name.Contains(TextBoxFilterName.Text))
        //                {
        //                    Helper.ReaktoriFilterLista.Add(r);
        //                }
        //            }
        //        }
        //    }

        //    TextBoxFilterName.Clear(); //da ne ostaje ispisano to po cemu smo filtrirali
        //}

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!MainViewModel.dragging)
            {
                MainViewModel.dragging = true;
                MainViewModel.o = (Reaktor)listView.SelectedItem;
                MainViewModel.draggedItem = new Picture((MainViewModel.o.ImagePath).ToString());
                DragDrop.DoDragDrop(this, MainViewModel.draggedItem, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void listView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainViewModel.draggedItem = null;
            listView.SelectedItem = null;
            MainViewModel.dragging = false;
        }

        private void button2_Click(object sender, RoutedEventArgs e) //show
        {
            for (int i = 0; i < MainViewModel.Reaktori.Count; i++) //da bi bili sortirani reaktori 
            {
                string log = "Izvjestaj_" + i + ".txt";
                textBoxReport.Text += "Reaktor " + i + "\n";
                using (StreamReader sr = new StreamReader(log))
                {
                    while (sr.Peek() >= 0)
                    {
                        String line = sr.ReadLine();
                        textBoxReport.Text += line + "\n";
                    }
                }
            }
        }

        private void buttonShowGraph_Click(object sender, RoutedEventArgs e)
        {

            graph.Children.Clear();  //ocistimo sve sto je bilo

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

                    if (Int32.Parse(textBoxGraph.Text) == rbr)     //iscrtava samo za odredjeni objekat
                    {
                        mjerenja.Add(stanje);                            // dodavanje procitane vrijednosti iz log fajla
                        brMjerenja++;
                    }
                }


                double width = graph.Width;   //sirina kanvasa u kom se nalazi graf
                double widthBar = 20; //sirinabara 20
                double mjesto = 30; //u odnosu na pocetak canvasa,dole lijevo //pozicija

                Line x_osa = new Line();                                // crtam x osu
                x_osa.X1 = 30;  //OK 30
                x_osa.X2 = width - 50;  //OK 50
                x_osa.Y1 = 398;    //350
                x_osa.Y2 = 398;   //350  gleda odozgo
                x_osa.Stroke = Brushes.Black; //koje boje je linija
                x_osa.StrokeThickness = 2;
                graph.Children.Add(x_osa);  //dodajemo canvasu children X osu

                Line y_osa = new Line();                                // crtam  y osu
                y_osa.X1 = 30; //OK
                y_osa.X2 = 30;
                y_osa.Y1 = 398;  //350
                y_osa.Y2 = 0;
                y_osa.Stroke = Brushes.Black;
                y_osa.StrokeThickness = 2;
                graph.Children.Add(y_osa);
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
                    graph.Children.Add(podiok);

                    TextBlock brojUzPodeok = new TextBlock();           // broj koji pise uz podiok
                    brojUzPodeok.Text = i.ToString();
                    brojUzPodeok.Foreground = Brushes.Black;
                    graph.Children.Add(brojUzPodeok);
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
                    graph.Children.Add(podiok);

                    TextBlock broj = new TextBlock();       // broj koji se pise uz podeok
                    broj.Text = (i * 10).ToString();        //da prikazuje 10,20,30..
                    broj.Foreground = Brushes.Black;
                    graph.Children.Add(broj);
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


                        graph.Children.Add(bar);                                    // dodavanje novog djeteta canvasu
                        Canvas.SetBottom(bar, 18);                                   // pozicioniranje na odgovarajuce mjesto na canvasu
                        Canvas.SetLeft(bar, mjesto + 3);
                        mjesto += widthBar + 5;                                 // pomjeranje pozicije za crtanje sledeceg bara
                    }
                }

            }

        }
    }
}
