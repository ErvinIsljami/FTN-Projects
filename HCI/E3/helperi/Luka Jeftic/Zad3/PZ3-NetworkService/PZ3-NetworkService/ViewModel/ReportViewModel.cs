using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.ViewModel
{
    public class ReportViewModel : BindableBase
    {

        private Dictionary<int, List<string>> reportText;


        private StreamReader sr = null;

        private string networkTerminal;
        private string networkTerminal2;


        private DateTime datumPocetka;
        private DateTime datumKraja;

        private string showString;

        public MyICommand<string> ReportCommand { get; private set; }



        public MyICommand ShowCommand { get; set; }


        public Dictionary<int, List<string>> ReportText
        {
            get
            {
                return reportText;
            }

            set
            {
                if (reportText != value)
                {
                    reportText = value;
                    OnPropertyChanged("ReportText");

                }

            }
        }


        public DateTime DatumPocetka
        {
            get { return datumPocetka; }

            set
            {

                if (datumPocetka != value)
                {
                    datumPocetka = value;
                    OnPropertyChanged("DatumPocetka");
                }
            }
        }


        public DateTime DatumKraja
        {
            get { return datumKraja; }

            set
            {

                if (datumKraja != value)
                {
                    datumKraja = value;
                    OnPropertyChanged("DatumKraja");
                }
            }
        }


        public string ShowString
        {
            get { return showString; }

            set
            {

                if (showString != value)
                {
                    showString = value;
                    OnPropertyChanged("ShowString");

                }
            }

        }


        public string NetworkTerminal
        {

            get { return networkTerminal; }
            set
            {
                if (networkTerminal != value)
                {
                    networkTerminal = value;
                    OnPropertyChanged("NetworkTerminal");
                }
            }

        }

        public string NetworkTerminal2
        {
            get { return networkTerminal2; }
            set
            {
                if (networkTerminal2 != value)
                {
                    networkTerminal2 = value;
                    OnPropertyChanged("NetworkTerminal2");
                }
            }
        }

        public ReportViewModel()
        {
            reportText = new Dictionary<int, List<string>>();
            ReportCommand = new MyICommand<string>(OnNav);
            ShowCommand = new MyICommand(OnShow);
            NetworkTerminal2 = ">>";

            DatumPocetka = DateTime.Now;
            DatumKraja = DateTime.Now;

        }

        public void OnShow()
        {
            readLogFile();
            
            ShowString = "";

            foreach (KeyValuePair<int, List<string>> keyValuePair in reportText)
            {
                string pom = "";
                pom = "\n\n\tOBJECT " + keyValuePair.Key + " :";
                ShowString += pom;
                foreach (string str in keyValuePair.Value)
                {
                    string s = "";
                    s = Environment.NewLine + "\t\t" + str;
                    ShowString += s;
                }
            }
        }

        public void readLogFile()
        {
            string line;

            sr = new StreamReader("log.txt");
            ReportText.Clear();

            while ((line = sr.ReadLine()) != null)
            {

                string[] lineParts = line.Split('|', '[', ']');

                //1/12/2019 8:15:31 PM | Object[12]  Value[418]
                //    DateTime 0         
                // ID   2    
                //Value  4



                DateTime dateParse = Convert.ToDateTime(lineParts[0].Split(' ')[0]);
                //DateTime dateParse = DateTime.ParseExact(lineParts[0].TrimEnd(), "mm/dd/yyyy HH:mm:ss tt", null);
                int idParse = Int32.Parse(lineParts[2]);
                // int valueParse = Int32.Parse(lineParts[4]);
                // string value = lineParts[4];

                string addStr = lineParts[0] + "\tCHANGED STATE: " + lineParts[4];

                int cmp1 = DateTime.Compare(datumPocetka, dateParse);
                int cmp2 = DateTime.Compare(dateParse, datumKraja);

                if (cmp1 <= 0 && cmp2 <= 0)
                {
                    if (reportText.ContainsKey(idParse))
                    {


                        reportText[idParse].Add(addStr);

                    }
                    else
                    {
                        reportText.Add(idParse, new List<string>());
                        reportText[idParse].Add(addStr);

                    }

                }
            }
            sr.Close();
        }


        private void OnNav(string destination)
        {
            if (destination == "enter")
            {

                switch(NetworkTerminal2)
                {
                    case "Start date (dd/MM/yyyy):":

                        
                        DatumPocetka = DateTime.ParseExact(NetworkTerminal, "dd/MM/yyyy", null);
                        NetworkTerminal = "";
                        NetworkTerminal2 = "End date (dd/MM/yyyy):";
                        break;
                    case "End date (dd/MM/yyyy):":

                        
                        DatumKraja = DateTime.ParseExact(networkTerminal, "dd/MM/yyyy", null);
                        OnShow();
                        NetworkTerminal = "";
                        NetworkTerminal2 = ">>";
                        
                        break;
                }


                switch (NetworkTerminal)
                {
                    case "networkData":
                        MainWindowViewModel.currentViewModel = MainWindowViewModel.networkDataViewModel;
                        MainWindowViewModel.RisePropChanged();
                        break;
                    case "network":
                        MainWindowViewModel.currentViewModel = MainWindowViewModel.homeViewModel;
                        MainWindowViewModel.RisePropChanged();
                        break;
                    case "chart":
                        MainWindowViewModel.currentViewModel = MainWindowViewModel.dataChartViewModel;
                        MainWindowViewModel.RisePropChanged();
                        break;
                    case "show":
                        NetworkTerminal = "";
                        NetworkTerminal2 = "Start date (dd/MM/yyyy):";
                        break;
                    default:
                        NetworkTerminal = "";
                        break;





                }

            }
        }
    }
}
