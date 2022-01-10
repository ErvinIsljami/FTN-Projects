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
using NetworkService.Models;
using System.Collections.ObjectModel;
using NetworkService.ViewModel;
using NetworkService.Views;

namespace NetworkService
{
    public partial class MainWindow : Window
    {
        //private int count = 15; // Inicijalna vrednost broja objekata u sistemu
                                // ######### ZAMENITI stvarnim brojem elemenata

        private DataIO serializer = new DataIO();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AllViewModelsContainer();
            CreateListener(); //Povezivanje sa serverskom aplikacijom
        }

        private void CreateListener()
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
                                                                              //count.ToStrin()
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(RoadsObs.Instance.Roads.Count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Objekat_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji
                            if (RoadsObs.Instance.Roads.Count > 0)
                            {
                                if (Int32.TryParse(incomming.Split('_')[1].Split(':')[0], out int n))
                                {
                                    string path = "log.txt";
                                    int roadIndex = Int32.Parse(incomming.Split('_')[1].Split(':')[0]);
                                    if (roadIndex < RoadsObs.Instance.Roads.Count + 1)
                                    {
                                        double value = Double.Parse(incomming.Split(':')[1]);
                                        RoadsObs.Instance.Roads[roadIndex].Value = value;

                                        if (RoadsObs.Instance.Roads[roadIndex].Type.NAME == "IA")
                                        {
                                            if (value > 15000)
                                            {
                                                RoadsObs.Instance.Roads[roadIndex].ShouldWarn = "Red";
                                            }
                                            else
                                            {
                                                RoadsObs.Instance.Roads[roadIndex].ShouldWarn = "Transparent";
                                            }
                                        }
                                        else if (RoadsObs.Instance.Roads[roadIndex].Type.NAME == "IB")
                                        {
                                            if (value > 7000)
                                            {
                                                RoadsObs.Instance.Roads[roadIndex].ShouldWarn = "Red";
                                            }
                                            else
                                            {
                                                RoadsObs.Instance.Roads[roadIndex].ShouldWarn = "Transparent";
                                            }
                                        }

                                        Road r = new Road();
                                        r = RoadsObs.Instance.Roads[roadIndex];
                                        NotifiedVms.Instance.NotifyAll(r);

                                        serializer.SerializeObject<ObservableCollection<Road>>(RoadsObs.Instance.Roads, "roads.xml");

                                        if (!File.Exists(path))
                                        {
                                            File.Create(path);
                                            using (var tw = new StreamWriter(path))
                                            {
                                                tw.WriteLine(
                                                    $"{DateTime.Now},{r.Id},Value:{r.Value}");
                                                tw.Close();
                                            }
                                        }
                                        else
                                        {
                                            using (var tw = new StreamWriter(path, true))
                                            {
                                                tw.WriteLine(
                                                    $"{DateTime.Now},{r.Id},Value:{r.Value}");
                                                tw.Close();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }
    }
}