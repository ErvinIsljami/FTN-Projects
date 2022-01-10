using PZ3_NetworkService.Model;
using PZ3_NetworkService.ViewModel;
using System;
using System.Collections.Generic;
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

namespace PZ3_NetworkService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        #region Old CreateListener() -> Not in use!
        //private void createListener()
        //{
        //    var tcp = new TcpListener(IPAddress.Any, 25565);
        //    tcp.Start();

        //    var listeningThread = new Thread(() =>
        //    {
        //        while (true)
        //        {
        //            var tcpClient = tcp.AcceptTcpClient();
        //            ThreadPool.QueueUserWorkItem(param =>
        //            {
        //                //Prijem poruke
        //                NetworkStream stream = tcpClient.GetStream();
        //                string incomming;
        //                byte[] bytes = new byte[1024];
        //                int i = stream.Read(bytes, 0, bytes.Length);
        //                //Primljena poruka je sacuvana u incomming stringu
        //                incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

        //                //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
        //                if (incomming.Equals("Need object count"))
        //                {

        //                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(DataBase.Valve_MainStorage.Count.ToString());
        //                    stream.Write(data, 0, data.Length);
        //                }
        //                else
        //                {

        //                    List<ValveModel> list = DataBase.Valve_MainStorage.Values.ToList();
        //                    if(Int32.TryParse(incomming.Split('_')[1].Split(':')[0], out int n))
        //                    {
        //                        int objindex = Int32.Parse(incomming.Split('_')[1].Split(':')[0]);
        //                        if(objindex<DataBase.Valve_MainStorage.Count)
        //                        {
        //                            double val = Double.Parse(incomming.Split(':')[1]);
        //                            DataBase.Valve_MainStorage[list[objindex].Id].Value = val;
        //                            DataBase.Valve_MainStorage[list[objindex].Id].Time = DateTime.Now;
        //                            Loger.Log($"{DateTime.Now.ToString()},Object,{objindex},{list[objindex].Name},{val} {Environment.NewLine}");
        //                            DataBase.logtext += $"{DateTime.Now.ToString()},Object,{objindex},{list[objindex].Name},{val} {Environment.NewLine}";
        //                        }

        //                    }


        //                }
        //            }, null);
        //        }
        //    });

        //    listeningThread.IsBackground = true;
        //    listeningThread.Start();
        //}
        #endregion
    }
}
