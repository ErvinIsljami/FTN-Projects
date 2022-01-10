using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PZ3_NetworkService.ViewModel
{
    public class DataChartViewModel : BindableBase
    {
        private ObservableCollection<double> bars;
        private ObservableCollection<Brush> barColor;
        private ObservableCollection<string> vrijeme;

        private string chartTerminal;
        private string chartTerminal2;

        public MyICommand<string> ChartCommand { get; private set; }

        public string ChartTerminal
        {

            get { return chartTerminal; }
            set
            {
                if (chartTerminal != value)
                {
                    chartTerminal = value;
                    OnPropertyChanged("ChartTerminal");
                }
            }

        }

        public string ChartTerminal2
        {
            get { return chartTerminal2; }
            set
            {
                if (chartTerminal2 != value)
                {
                    chartTerminal2 = value;
                    OnPropertyChanged("ChartTerminal2");
                }
            }
        }

        public DataChartViewModel()
        {
            Bars = new ObservableCollection<double>();
            BarColor = new ObservableCollection<Brush>();
            Vrijeme = new ObservableCollection<string>();

            ChartCommand = new MyICommand<string>(OnNav);

            ChartTerminal2 = ">>";

            Bars = Kolekcija.GlobalBarValues;
            BarColor = Kolekcija.ChartColor;
            Vrijeme = Kolekcija.GlobDatumi;

        }

        public ObservableCollection<double> Bars
        {
            get
            {
                return bars;
            }
            set
            {
                if (bars != value)
                {
                    bars = value;
                    OnPropertyChanged("Bars");
                }
            }
        }

        public ObservableCollection<Brush> BarColor
        {
            get
            {
                return barColor;
            }
            set
            {
                if (barColor != value)
                {
                    barColor = value;
                    OnPropertyChanged("BarColor");
                }
            }
        }

        public ObservableCollection<string> Vrijeme
        {
            get
            {
                return vrijeme;
            }
            set
            {
                if (vrijeme != value)
                {
                    vrijeme = value;
                    OnPropertyChanged("Vrijeme");
                }
            }
        }


        private void OnNav(string destination)
        {
            if (destination == "enter")
            {
                switch (ChartTerminal)
                {
                    case "networkData":
                        MainWindowViewModel.currentViewModel = MainWindowViewModel.networkDataViewModel;
                        MainWindowViewModel.RisePropChanged();
                        break;
                    case "network":
                        MainWindowViewModel.currentViewModel = MainWindowViewModel.homeViewModel;
                        MainWindowViewModel.RisePropChanged();
                        break;
                    case "report":
                        MainWindowViewModel.currentViewModel = MainWindowViewModel.reportViewModel;
                        MainWindowViewModel.RisePropChanged();
                        break;
                    default:
                        ChartTerminal2 = "Wrong Command!";
                        ChartTerminal = "";
                        break;


                }

            }
        }
    }
}
