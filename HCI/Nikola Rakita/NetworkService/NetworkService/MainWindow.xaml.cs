using NetworkService.Model;
using NetworkService.ViewModel;
using System;
using System.Collections.Generic;
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

namespace NetworkService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //private int count = 15; // Inicijalna vrednost broja objekata u sistemu
        // ######### ZAMENITI stvarnim brojem elemenata
        private StreamWriter sw = null;
        public MainWindow()
        {
            InitializeComponent();
            createListener(); //Povezivanje sa serverskom aplikacijom
            
        }

        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();
            int brojac = 0;

            Kolekcija.GlobalBarValues.Add(0);
            Kolekcija.GlobalBarValues.Add(0);
            Kolekcija.GlobalBarValues.Add(0);
            Kolekcija.GlobalBarValues.Add(0);
            Kolekcija.GlobalBarValues.Add(0);
            Kolekcija.GlobalBarValues.Add(0);
            Kolekcija.GlobalBarValues.Add(0);

            Brush _colorGreen = Brushes.Green;
            Brush _colorRed = Brushes.Red;
 

            Kolekcija.ChartColor.Add(_colorGreen);
            Kolekcija.ChartColor.Add(_colorGreen);
            Kolekcija.ChartColor.Add(_colorGreen);
            Kolekcija.ChartColor.Add(_colorGreen);
            Kolekcija.ChartColor.Add(_colorGreen);
            Kolekcija.ChartColor.Add(_colorGreen);
            Kolekcija.ChartColor.Add(_colorGreen);

            Kolekcija.GlobDatumi.Add("");
            Kolekcija.GlobDatumi.Add("");
            Kolekcija.GlobDatumi.Add("");
            Kolekcija.GlobDatumi.Add("");
            Kolekcija.GlobDatumi.Add("");
            Kolekcija.GlobDatumi.Add("");
            Kolekcija.GlobDatumi.Add("");





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
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(Kolekcija.AllObjects.Count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Objekat_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji
                            int idx = Int32.Parse(incomming.Split('_', ':')[1]);


                            Kolekcija.AllObjects[idx].Valuee = double.Parse(incomming.Split('_', ':')[2]);
                            Kolekcija.GlobalBarValues[brojac] = 0.05143 * double.Parse(incomming.Split('_', ':')[2]);
                            Kolekcija.GlobDatumi[brojac] = DateTime.Now.ToString("HH:mm:ss");

                            if (double.Parse(incomming.Split('_', ':')[2]) >= 1000 && double.Parse(incomming.Split('_', ':')[2]) <= 5000)
                            {
                                Kolekcija.ChartColor[brojac] = _colorGreen;
                            }
                            else
                            {
                                Kolekcija.ChartColor[brojac] = _colorRed;
                            }

                            if (brojac == 6)
                            {
                                brojac = 0;
                            }
                            else
                            {
                                ++brojac;
                            }



                            using (sw = new StreamWriter("log.txt", true))
                            {
                                sw.WriteLine(DateTime.Now.ToString() + "\t|\tObject[" + Kolekcija.AllObjects[idx].Id + "]\t\tValue[" + Kolekcija.AllObjects[idx].Valuee + "]");
                                //1/12/2019 8:15:31 PM | Object[12]	  Value[418]


                            }
                            //sw.Close();

                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }


    }
}
