using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
            ChartCommand = new MyICommand<string>(OnChartCommand);
            ChartTerminal2 = ">>";

			Kolekcija.GlobalBarValues.Add(0);
			Kolekcija.GlobalBarValues.Add(0);
			Kolekcija.GlobalBarValues.Add(0);
			Kolekcija.GlobalBarValues.Add(0);
			Kolekcija.GlobalBarValues.Add(0);

			Kolekcija.GlobDatumi.Add("");
			Kolekcija.GlobDatumi.Add("");
			Kolekcija.GlobDatumi.Add("");
			Kolekcija.GlobDatumi.Add("");
			Kolekcija.GlobDatumi.Add("");

			Kolekcija.ChartColor.Add(Brushes.Green);
			Kolekcija.ChartColor.Add(Brushes.Green);
			Kolekcija.ChartColor.Add(Brushes.Green);
			Kolekcija.ChartColor.Add(Brushes.Green);
			Kolekcija.ChartColor.Add(Brushes.Green);

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

        private void OnChartCommand(string destination)
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
					case "refreshChart":
						ShowChart();
						ChartTerminal = "";
						break;
                    default:
                        ChartTerminal2 = "Wrong Command!";
                        ChartTerminal = "";
                        break;
                }
            }
        }

		public void ShowChart()
		{
			ResetChart();

			// Iscitati sve linije iz loga
			string[] logLines;
			if (File.Exists("log.txt"))
			{
				while (true)
				{
					try
					{
						logLines = File.ReadAllLines("log.txt");
						break;
					}
					catch (Exception e)
					{
						continue;
					}
				}
			}
			else
			{
				return;
			}

			// Obrnuti log i izvuci poslednjih 5 elemenata
			List<string> reversedMeasurements = logLines.Reverse().ToList();
			List<Tuple<double, DateTime>> lastFive = new List<Tuple<double, DateTime>>();

			int i = 0;
			foreach (string line in reversedMeasurements)
			{
				if (i == 5)
				{
					break;
				}

				string[] elementi = line.Split('#');
				string datumString = elementi[0];
				double vrijednost = Double.Parse(elementi[2]);
				DateTime datum = Convert.ToDateTime(datumString);

				lastFive.Add(new Tuple<double, DateTime>(vrijednost, datum));
				i++;
			}

			// Primeniti na chart
			for(int j = 0; j < lastFive.Count; j++)
			{
				// Azurira visinu odgovarajuceg bara
				Kolekcija.GlobalBarValues[j] = 71 * lastFive[j].Item1;
				// Azurira datom odgovarajuceg bara
				Kolekcija.GlobDatumi[j] = lastFive[j].Item2.ToString("HH:mm:ss");

				// Podesava boju u zavisnosti od ogranicenja
				if (lastFive[j].Item1 >= 0.34 && lastFive[j].Item1 <= 2.73)
				{
					Kolekcija.ChartColor[j] = Brushes.Green;
				}
				else
				{
					Kolekcija.ChartColor[j] = Brushes.Red;
				}
			}
		}

		public void ResetChart()
		{
			Kolekcija.GlobalBarValues.Add(0);
			Kolekcija.GlobalBarValues.Add(0);
			Kolekcija.GlobalBarValues.Add(0);
			Kolekcija.GlobalBarValues.Add(0);
			Kolekcija.GlobalBarValues.Add(0);

			Kolekcija.GlobDatumi.Add("");
			Kolekcija.GlobDatumi.Add("");
			Kolekcija.GlobDatumi.Add("");
			Kolekcija.GlobDatumi.Add("");
			Kolekcija.GlobDatumi.Add("");

			Kolekcija.ChartColor.Add(Brushes.Green);
			Kolekcija.ChartColor.Add(Brushes.Green);
			Kolekcija.ChartColor.Add(Brushes.Green);
			Kolekcija.ChartColor.Add(Brushes.Green);
			Kolekcija.ChartColor.Add(Brushes.Green);
		}
    }
}
