using Common;
using Common.Contracts;
using Common.Model;
using LocalController.Analytics;
using LocalController.Proxies;
using LocalController.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
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

namespace LocalController
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public ObservableCollection<Generator> LocalGenerators { get; set; }
		public ObservableCollection<string> LocalGeneratorsIds { get; set; }
		public ObservableCollection<Group> LocalGroups { get; set; }
		public ObservableCollection<string> LocalGroupsIds { get; set; }
		public Dictionary<string, List<Generator>> MatchedGroupsAndGenerators { get; set; }
		public ObservableCollection<Generator> CurrentlyPickedGroupGenerators { get; set; }
		public LoginRequestResponse LRR { get; set; }
		public string LcGuid { get; set; }
		public LcToScServiceProxy LcToScServiceProxy { get; set; }
		public UpdateSetpointServiceHost UpdateSetpointServiceHost { get; set; }
		public XmlReaderWriter ReaderWriter { get; set; }
		private string localGeneratorsFilename = "localGenerators.xml";
		private string localGroupsFilename = "localGroups.xml";

        private void DbForTest()
        {
            Group group1 = new Group("GR-01");
            Group group2 = new Group("GR-02");
            Group group3 = new Group("GR-03");

            Generator gen1 = new Generator("GN-01", UnitTypeEnum.SOLAR, 424, 10, 1000, ControlTypeEnum.REMOTE, 312, "GR-01");
            Generator gen2 = new Generator("GN-02", UnitTypeEnum.WIND, 21, 10, 1000, ControlTypeEnum.REMOTE, 312, "GR-02");
            Generator gen3 = new Generator("GN-03", UnitTypeEnum.MICRO_HYDRO, 312, 10, 1000, ControlTypeEnum.LOCAL, 312, "GR-01");
            Generator gen4 = new Generator("GN-04", UnitTypeEnum.SOLAR, 42, 10, 1000, ControlTypeEnum.REMOTE, 312, "GR-03");
            Generator gen5 = new Generator("GN-05", UnitTypeEnum.SOLAR, 756, 10, 1000, ControlTypeEnum.LOCAL, 312, "GR-01");
            Generator gen6 = new Generator("GN-07", UnitTypeEnum.WIND, 4, 10, 1000, ControlTypeEnum.REMOTE, 312, "GR-01");
            Generator gen7 = new Generator("GN-08", UnitTypeEnum.MICRO_HYDRO, 76, 10, 1000, ControlTypeEnum.REMOTE, 312, "GR-02");
            Generator gen8 = new Generator("GN-09", UnitTypeEnum.WIND, 345, 10, 1000, ControlTypeEnum.LOCAL, 312, "GR-01");
            Generator gen9 = new Generator("GN-00", UnitTypeEnum.SOLAR, 23, 10, 1000, ControlTypeEnum.LOCAL, 312, "GR-02");

            AddGroup(group1);
            AddGroup(group2);
            AddGroup(group3);

            AddGenerator(gen1);
            AddGenerator(gen2);
            AddGenerator(gen3);
            AddGenerator(gen4);
            AddGenerator(gen5);
            AddGenerator(gen6);
            AddGenerator(gen7);
            AddGenerator(gen8);
            AddGenerator(gen9);
        }

        public MainWindow()
		{
			ReaderWriter = new XmlReaderWriter();
			LocalGenerators = new ObservableCollection<Generator>();
			LocalGeneratorsIds = new ObservableCollection<string>();
			LocalGroups = new ObservableCollection<Group>();
			LocalGroupsIds = new ObservableCollection<string>();
			CurrentlyPickedGroupGenerators = new ObservableCollection<Generator>();
			MatchedGroupsAndGenerators = new Dictionary<string, List<Generator>>();

            InitializeComponent();

			groupsListView.ItemsSource = LocalGroups;
			generatorsDataGrid.ItemsSource = CurrentlyPickedGroupGenerators;

            //DbForTest();

			UpdateLocalDatabase();

			LcToScServiceProxy = new LcToScServiceProxy();
			LcToScServiceProxy.Login(LocalGenerators.ToList());
			LRR = LcToScServiceProxy.LRR;
			LcGuid = LcToScServiceProxy.LRR.LcId;

			try
			{
				if (!String.IsNullOrWhiteSpace(LcGuid))
				{
					UpdateSetpointServiceHost = new UpdateSetpointServiceHost(LRR.UpdateServicePointPort, LcGuid);
					PowerUpdater pu = new PowerUpdater(this);
					OpenUpdateSetpointService();
					StartSendingMeasurements();
				}
			}
			catch (Exception e)
			{
				throw new Exception("Local Controller failed to login. Please restart the application.", e);
			}
		}

		public void OpenUpdateSetpointService()
		{
			if (UpdateSetpointServiceHost != null)
			{
				UpdateSetpointServiceHost.OpenService();
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Window dodajNoviGenerator_Window = new AddNewGeneratorWindow(this);
			dodajNoviGenerator_Window.Show();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Window dodajNovuGrupuWindow = new AddNewGroupWindow(this);
			dodajNovuGrupuWindow.Show();
		}

		private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			Window promenaTipa = new ChangeControlTypeAndValueWindow((Generator)generatorsDataGrid.SelectedItem, this);
			promenaTipa.Show();
		}

		private void GroupSelect_Click(object sender, RoutedEventArgs e)
		{
			Group g = groupsListView.SelectedItem as Group;
			CurrentlyPickedGroupGenerators = new ObservableCollection<Generator>(MatchedGroupsAndGenerators[g.Id].ToList());
			generatorsDataGrid.ItemsSource = null;
			generatorsDataGrid.ItemsSource = CurrentlyPickedGroupGenerators;
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			try
			{
				Calculations calculations = new Calculations(this);
				DateTime dateFrom = DateTime.ParseExact(txt_box_poc_datum.Text + " 00:00", "g", new CultureInfo("fr-FR"));
				DateTime dateTo = DateTime.ParseExact(txt_box_kraj_datum.Text + " 00:00", "g", new CultureInfo("fr-FR"));
				string id = cmb_box_stat.SelectedValue.ToString();
				if (radio_btn_generator.IsChecked == true)
				{
					txt_box_min.Text = calculations.MinPowerGenerator(LcGuid, id, dateFrom, dateTo).ToString("0.##") + " kW";
					txt_box_srednja.Text = calculations.MeanPowerGenerator(LcGuid, id, dateFrom, dateTo).ToString("0.##") + " kW";
					txt_boz_max.Text = calculations.MaxPowerGenerator(LcGuid, id, dateFrom, dateTo).ToString("0.##") + " kW";
				}
				else
				{
					txt_box_min.Text = calculations.MinPowerGroup(LcGuid, id, dateFrom, dateTo).ToString("0.##") + " kW";
					txt_box_srednja.Text = calculations.MeanPowerGroup(LcGuid, id, dateFrom, dateTo).ToString("0.##") + " kW";
					txt_boz_max.Text = calculations.MaxPowerGroup(LcGuid, id, dateFrom, dateTo).ToString("0.##") + " kW";
				}
			}
			catch (Exception exc)
			{
				MessageBox.Show("Error while parsing data. Check all fields and try again. " + exc.Message, "Parse Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void Btn_pocetni_datum_Click(object sender, RoutedEventArgs e)
		{
			if (calendar.SelectedDate != null)
				txt_box_poc_datum.Text = calendar.SelectedDate.Value.ToString("dd/MM/yyyy");
		}

		private void Btn_krajnji_datum_Click(object sender, RoutedEventArgs e)
		{
			if (calendar.SelectedDate != null)
				txt_box_kraj_datum.Text = calendar.SelectedDate.Value.ToString("dd/MM/yyyy");
		}

		private void RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			cmb_box_stat.ItemsSource = LocalGroups.Select(x => x.Id);
		}

		private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
		{
			cmb_box_stat.ItemsSource = LocalGenerators.Select(x => x.Id);
		}

		private async void StartSendingMeasurements()
		{
			while (true)
			{
                LcToScServiceProxy.SendMeasurements(LcGuid, ReaderWriter.DeserializeObject<ObservableCollection<Generator>>(localGeneratorsFilename).ToList());
				await Task.Delay(3000);
			}
		}

		public bool AddGenerator(Generator newGenerator)
		{
			if ((LocalGenerators.FirstOrDefault(g => g.Id == newGenerator.Id)) == null)
			{
				LocalGenerators.Add(newGenerator);
				MatchedGroupsAndGenerators[newGenerator.Group].Add(newGenerator);
				if(groupsListView.SelectedItem != null)
				{
					CurrentlyPickedGroupGenerators = new ObservableCollection<Generator>(MatchedGroupsAndGenerators[newGenerator.Group]);
					generatorsDataGrid.ItemsSource = CurrentlyPickedGroupGenerators;
					generatorsDataGrid.Items.Refresh();
				}
				UpdateLocalData();
				return true;
			}
			return false;
		}

		public bool AddGroup(Group newGroup)
		{
			if ((LocalGroups.FirstOrDefault(g => g.Id == newGroup.Id)) == null)
			{
				LocalGroups.Add(newGroup);
				MatchedGroupsAndGenerators.Add(newGroup.Id, new List<Generator>());
				LocalGroupsIds = new ObservableCollection<string>(LocalGroups.Select(g => g.Id).ToList());
				groupsListView.ItemsSource = LocalGroups;
				groupsListView.Items.Refresh();
				UpdateLocalData();
				return true;
			}
			return false;
		}

		public void AddGeneratorToGroup(string generatorId, string groupId)
		{
			Generator generator = LocalGenerators.FirstOrDefault(gen => gen.Id == generatorId);
			if (generator != null)
			{
				Group group = LocalGroups.FirstOrDefault(gr => gr.Id == groupId);
				generator.Group = group.Id;
				if (group != null)
				{
					MatchedGroupsAndGenerators[group.Id].Add(generator);
				}
			}
		}

		public void ChangeGeneratorControlTypeAndCurrentRealPower(Generator g, ControlTypeEnum newControlType, double newRealPower)
		{
			g.ControlType = newControlType;
			if (g.ControlType == ControlTypeEnum.LOCAL)
			{
				g.ManuallySetActivePower(newRealPower);
			}			
			UpdateLocalData();
		}

		private void UpdateLocalDatabase()
		{
			ReaderWriter.SerializeObject<ObservableCollection<Generator>>(LocalGenerators, localGeneratorsFilename);
			ReaderWriter.SerializeObject<ObservableCollection<Group>>(LocalGroups, localGroupsFilename);
		}

		private void UpdateLocalGeneratorsDatagrid()
		{
			Group gr = groupsListView.SelectedItem as Group;
			if (gr != null)
			{
				CurrentlyPickedGroupGenerators = new ObservableCollection<Generator>(MatchedGroupsAndGenerators[gr.Id]);
				generatorsDataGrid.ItemsSource = CurrentlyPickedGroupGenerators;
				generatorsDataGrid.Items.Refresh();
			}
		}

		public void UpdateLocalData()
		{
			Dispatcher.Invoke(UpdateLocalGeneratorsDatagrid);
			UpdateLocalDatabase();
		}
	}
}
