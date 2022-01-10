using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Controls;
using static PZ3_NetworkService.MyCommand;

namespace PZ3_NetworkService.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand<Window> ExitCommand { get; set; }
        public MyICommand<Window> DragMoveCommand { get; set; }
        public MyICommand<string> NavCommand { get; private set; }
        public MyICommand UpCommand { get; private set; }
        public MyICommand DownCommand { get; private set; }
        private BindableBase currentViewModel;
        private NetworkDataViewModel networkDataViewModel = new NetworkDataViewModel();
        private DataChartViewModel dataChartViewModel = new DataChartViewModel();
        private NetworkViewViewModel networkViewViewModel = new NetworkViewViewModel();
        private DataReportViewModel dataReportViewModel = new DataReportViewModel();
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        public MainWindowViewModel()
        {
            Loger.InitializeLoger();
            CreateListener();
            currentViewModel = networkDataViewModel;
            ExitCommand = new MyICommand<Window>(OnExit);
            DragMoveCommand = new MyICommand<Window>(OnDragMove);
            NavCommand = new MyICommand<string>(OnNav);
            UpCommand = new MyICommand(OnUp);
            DownCommand = new MyICommand(OnDown);
            DataBase.Valve_MainStorage.Add(1, new ValveModel { Id = 1, Value = 0, Name = "Valve0", Type = Types.TypesList[1] });
            DataBase.Valve_MainStorage.Add(2, new ValveModel { Id = 2, Value = 0, Name = "Valve1", Type = Types.TypesList[2] });
            ++DataBase.IACount;
            ++DataBase.IBCount;
            DataBase.ItemsCount += 2;
        }

        private void OnDragMove(Window window)
        {
            window.DragMove();
        }

        private void OnExit(Window window)
        {
            window.Close();
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "data":
                    CurrentViewModel = networkDataViewModel;
                    break;
                case "chart":
                    CurrentViewModel = dataChartViewModel;
                    break;
                case "view":
                    CurrentViewModel = networkViewViewModel;
                    break;
                case "report":
                    CurrentViewModel = dataReportViewModel;
                    break;
                    
            }
        }

        private void OnDown()
        {
            if(CurrentViewModel.GetType()==typeof(NetworkViewViewModel))
            {
                CurrentViewModel = networkDataViewModel;
            }
            else if(CurrentViewModel.GetType() == typeof(NetworkDataViewModel))
            {
                CurrentViewModel = dataChartViewModel;
            }
            else if(CurrentViewModel.GetType() == typeof(DataChartViewModel))
            {
                CurrentViewModel = dataReportViewModel;
            }
            else if(CurrentViewModel.GetType() == typeof(DataReportViewModel))
            {
                CurrentViewModel = networkViewViewModel;
            }
        }

        private void OnUp()
        {
            if (CurrentViewModel.GetType() == typeof(NetworkViewViewModel))
            {
                CurrentViewModel = dataReportViewModel;
            }
            else if (CurrentViewModel.GetType() == typeof(NetworkDataViewModel))
            {
                CurrentViewModel = networkViewViewModel;
            }
            else if (CurrentViewModel.GetType() == typeof(DataChartViewModel))
            {
                CurrentViewModel = networkDataViewModel;
            }
            else if (CurrentViewModel.GetType() == typeof(DataReportViewModel))
            {
                CurrentViewModel = dataChartViewModel;
            }
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

                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(DataBase.Valve_MainStorage.Count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {

                            List<ValveModel> list = DataBase.Valve_MainStorage.Values.ToList();
                            if (Int32.TryParse(incomming.Split('_')[1].Split(':')[0], out int n))
                            {
                                int objindex = Int32.Parse(incomming.Split('_')[1].Split(':')[0]);
                                if (objindex < DataBase.Valve_MainStorage.Count)
                                {
                                    try
                                    {
                                        double val = Double.Parse(incomming.Split(':')[1]);
                                        DataBase.Valve_MainStorage[list[objindex].Id].Value = val;
                                        DataBase.Valve_MainStorage[list[objindex].Id].Time = DateTime.Now;
                                        Loger.Log($"{DateTime.Now.ToString()},Object,{objindex},{list[objindex].Name},{val} {Environment.NewLine}");
                                        DataBase.logtext += $"{DateTime.Now.ToString()},Object,{objindex},{list[objindex].Name},{val} {Environment.NewLine}";
                                    }
                                    catch {}
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
