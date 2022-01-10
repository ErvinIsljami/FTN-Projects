using Common;
using Common.Contracts;
using Common.Model;
using SystemController.Proxies;
using SystemController.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace SystemController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		private ObservableCollection<LC> lKs = new ObservableCollection<LC>();
		private ObservableCollection<Generator> currentlyDisplayedGenerators = new ObservableCollection<Generator>();
		private Dictionary<string, List<Generator>> lcsAndMatchingGenerators = new Dictionary<string, List<Generator>>();
        private Task sendSetpointsTask = null;
        private CancellationTokenSource tokenCancel = new CancellationTokenSource();
        private CancellationToken token;
        public LcToScService lcToScService;
		public LcToScServiceServiceHost lcToScServiceHost;
		public ObservableCollection<LC> LCs { get => lKs; set => lKs = value; }
		public ObservableCollection<Generator> CurrentlyDisplayedGenerators { get => currentlyDisplayedGenerators; set => currentlyDisplayedGenerators = value; }
		public Dictionary<string, List<Generator>> LcsAndMatchingGenerators { get => lcsAndMatchingGenerators; set => lcsAndMatchingGenerators = value; }
		public Statistics StatisticsCalculator { get; set; }
		public string CurrentForecast { get; set; }
		public Dictionary<string, double> LcsAndUpdateSetpointPorts { get; set; }
		public GeneratorIdServiceServiceHost generatorIdServiceHost;

		public MainWindow()
        {
			StatisticsCalculator = new Statistics();
			CurrentForecast = "";

			InitializeComponent();

			Cache.Instance.InitializeCache(this);

			LCs = new ObservableCollection<LC>();
			CurrentlyDisplayedGenerators = new ObservableCollection<Generator>();
			LcsAndUpdateSetpointPorts = new Dictionary<string, double>();

			LcListView.ItemsSource = LCs;
			dataGrid.ItemsSource = CurrentlyDisplayedGenerators;

			lcToScService = new LcToScService();
			lcToScServiceHost = new LcToScServiceServiceHost(lcToScService);
			OpenLcToScService();
			generatorIdServiceHost = new GeneratorIdServiceServiceHost();
			OpenGeneratorIdService();
			ProcessMeasurements();
			CalculateReport();
		}

		public void OpenLcToScService()
		{
			if(lcToScServiceHost != null)
			{
				lcToScServiceHost.OpenService();
			}
		}

		public void OpenGeneratorIdService()
		{
			if(generatorIdServiceHost != null)
			{
				generatorIdServiceHost.OpenService();
			}
		}

		private void LkSelect_Click(object sender, RoutedEventArgs e)
		{
			LC lc = LcListView.SelectedItem as LC;
			UpdateGeneratorsDataGrid(lc.Guid);
		}

		private void UpdateGeneratorsDataGrid(string lcId)
		{
			LC lc = LcListView.SelectedItem as LC;
			if (lc != null)
			{
				LcListView.SelectedItem = lc;
				CurrentlyDisplayedGenerators.Clear();
				CurrentlyDisplayedGenerators = new ObservableCollection<Generator>();
				foreach (Generator g in LcsAndMatchingGenerators[lcId])
				{
					CurrentlyDisplayedGenerators.Add(g);
				}
				dataGrid.ItemsSource = null;
				dataGrid.ItemsSource = CurrentlyDisplayedGenerators;
			}
		}

        private void SetExpectedPowerGeneration_Click(object sender, RoutedEventArgs e)
        {
			// TODO OVDE CE SE SVAKIH 10 SEKUNDI RESETOVATI ZELJENA SNAGA
            double requiredPower = 0;
            try
            {
                requiredPower = Double.Parse(ExpectedPowerGenerationTb.Text);
                ExpectedPowerGenerationTb.Text = "";

            }
            catch
            {
                MessageBox.Show("Invalid number. Please enter valid number.", "Error", MessageBoxButton.OK);
            }

            var remoteGeneratorsList = LcsAndMatchingGenerators.Values.SelectMany(x => x)
                                                                      .Where(a => a.ControlType == ControlTypeEnum.REMOTE)
                                                                      .OrderBy(b => b.WorkPrice)
                                                                      .ToList();

            double maxPower = 0;
            remoteGeneratorsList.ForEach(x => maxPower += x.MaxPower);
            if(requiredPower > maxPower)
            {
                MessageBox.Show($"Required power[{requiredPower}] must be lower than maximum remote capacity power[{maxPower}].");
            }

            if(sendSetpointsTask != null)
            {
                this.tokenCancel.Cancel();
            }
            token = tokenCancel.Token;
            sendSetpointsTask = new Task(() => SendCalculatedSetpoints(remoteGeneratorsList, requiredPower, token));
            sendSetpointsTask.Start();
            this.tokenCancel = new CancellationTokenSource();
        }

        private async void SendCalculatedSetpoints(List<Generator> remoteGeneratorsList, double requiredPowerFromView, CancellationToken cancellationToken)
        {
            
            while(true)
            {
                double requiredPower = requiredPowerFromView;
                Dictionary<string, double> generatorsCalculatedValue = new Dictionary<string, double>();

                remoteGeneratorsList.ForEach(x => { requiredPower -= x.MinPower; generatorsCalculatedValue[x.Id] = x.MinPower; });

                foreach (Generator g in remoteGeneratorsList)
                {
                    if (requiredPower >= (g.MaxPower - g.MinPower))
                    {
                        generatorsCalculatedValue[g.Id] = g.MaxPower;
                        requiredPower -= (g.MaxPower - g.MinPower);
                    }
                    else
                    {
                        generatorsCalculatedValue[g.Id] += requiredPower;
                        requiredPower = 0;
                    }
                    if (requiredPower == 0)
                    {
                        break;
                    }
                }

                string[] lcIdsArr = new string[LcsAndMatchingGenerators.Keys.Count];
                LcsAndMatchingGenerators.Keys.CopyTo(lcIdsArr, 0);
				List<string> lcIdsList = lcIdsArr.ToList();

				foreach (var lcId in lcIdsList)
                {
                    UpdateSetpointServiceProxy proxy = new UpdateSetpointServiceProxy(lcId, LcsAndUpdateSetpointPorts[lcId]);
                    List<SetpointArray> setpointArrays = new List<SetpointArray>();

                    foreach (Generator g in LcsAndMatchingGenerators[lcId].Where(x => x.ControlType == ControlTypeEnum.REMOTE))
                    {
                        SetpointArray setpointArray = new SetpointArray();
                        for (int i = 0; i < 10; i++)
                        {
                            Setpoint setPoint = new Setpoint(generatorsCalculatedValue[g.Id], DateTime.Now.AddSeconds(i * 10), g.Id);
                            setpointArray.Array.Add(setPoint);
                        }
                        setpointArrays.Add(setpointArray);
                    }
                    proxy.SetPointUpdate(lcId, setpointArrays);

                }

                if(token.IsCancellationRequested)
                {
                    break;
                }

                await Task.Delay(10000);
            }
        }

		private async void ProcessMeasurements()
		{
			while (true)
			{
				Dictionary<string, List<Generator>> localLoggedLcs = new Dictionary<string, List<Generator>>();

				lock (LcToScService.lockObj)
				{
					foreach (var lcAndGensPair in Cache.LoggedLcs)
					{
						localLoggedLcs.Add(lcAndGensPair.Key, lcAndGensPair.Value);
					}
				}

				foreach (var localLcAndGensPair in localLoggedLcs)
				{
					foreach (Generator g in localLcAndGensPair.Value)
					{
						LC lc = DataAccessHelper.Instance.LCs.FirstOrDefault(l => l.Guid == localLcAndGensPair.Key);
						if(lc != null)
						{
							foreach (Generator gen in lc.Generators)
							{
								if (gen.Id == g.Id)
								{
									gen.ManuallySetActivePower(g.CurrentActivePower);
								}
							}
						}
					}
				}
				DataAccessHelper.Instance.UpdateDatabase();

				await Task.Delay(10000);
			}
		}

		public void Login(LoginRequestResponse newLoggedLcData, List<Generator> newLoggedLcValues)
		{
			LcsAndMatchingGenerators.Add(newLoggedLcData.LcId, newLoggedLcValues);
			LCs.Add(new LC(newLoggedLcData.LcId, newLoggedLcValues));
			LcsAndUpdateSetpointPorts.Add(newLoggedLcData.LcId, newLoggedLcData.UpdateServicePointPort);
			LcListView.Items.Refresh();
		}

		public void UpdateLcGenerators(string lcId, List<Generator> generators)
		{
			LcsAndMatchingGenerators[lcId] = generators;
			UpdateGeneratorsDataGrid(lcId);
			DataAccessHelper.Instance.UpdateLcGenerators(lcId, generators);
		}

		private void GenerateForecast_Click(object sender, RoutedEventArgs e)
		{
			ForecastWindow fw = new ForecastWindow(CurrentForecast);
			fw.Show();
		}

		public async void CalculateReport()
		{
			while (true)
			{
				Dictionary<Generator, List<Tuple<DateTime, double>>> forecast = StatisticsCalculator.LoadFollowing(StatisticsCalculator.AllGeneratorsLoadForecast());
				StringBuilder sb = new StringBuilder();

				foreach(var generatorForecast in forecast)
				{
					sb.AppendLine($"Generator {generatorForecast.Key.Id}: ----------------------------------------------------------------------------------------------------");

                    int newlineCnt = 0;
					foreach(var forecastValue in generatorForecast.Value)
					{
                        if(newlineCnt == 10)
                        {
                            sb.AppendLine();
                            newlineCnt = 0;
                        }
                        else
                        {
                            sb.Append($" {forecastValue.Item1} - {forecastValue.Item2}");
                            if(newlineCnt != 9)
                            {
                                sb.Append(" -----");
                            }
                            newlineCnt++;
                        }
                    }
                }

				CurrentForecast = sb.ToString();

				await Task.Delay(10000);
			}
		}
	}
}
