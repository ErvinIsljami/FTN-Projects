using PZ3_NetworkService.Commands;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace PZ3_NetworkService.Containers
{
	public static class Container
	{
		private static string xmlFilename;
		private static DataIO dataIO;
		public static double ValveMpMax;
		public static double ValveMpMin;
		private static string GasImage;
		private static string WaterImage;
		public static CommandInvoker CommandInvoker;
		public static ObservableCollection<Valve> Valves;
		public static IEnumerable<ValveType> ValveTypes;
		public static Dictionary<ValveType, string> ValveTypesAndImagesMapper;
		public static Dictionary<string, ObservableCollection<Valve>> DataGridMap;
		public static CollectionViewSource SearchCollection { get; set; }

		static Container()
		{
			xmlFilename = "valves.xml";
			dataIO = new DataIO();
			ValveMpMax = 16;
			ValveMpMin = 5;
			GasImage = "pack://application:,,,/Images/gas.png";
			WaterImage = "pack://application:,,,/Images/water.png";
			CommandInvoker = new CommandInvoker();
			ObservableCollection<Valve> valves = LoadValvesFromXml();
			if(valves == null)
			{
				Valves = new ObservableCollection<Valve>();
			}
			else
			{
				Valves = valves;
			}
			ValveTypes = Enum.GetValues(typeof(ValveType)).Cast<ValveType>();
			ValveTypesAndImagesMapper = new Dictionary<ValveType, string>();
			ValveTypesAndImagesMapper.Add(ValveType.Gas, GasImage);
			ValveTypesAndImagesMapper.Add(ValveType.Water, WaterImage);
			DataGridMap = new Dictionary<string, ObservableCollection<Valve>>();
			SearchCollection = new CollectionViewSource();
			SearchCollection.Source = Valves;
		}

		public static void SaveValvesToXml()
		{
			dataIO.SerializeObject<ObservableCollection<Valve>>(Valves, xmlFilename);
		}

		public static ObservableCollection<Valve> LoadValvesFromXml()
		{
			return dataIO.DeserializeObject<ObservableCollection<Valve>>(xmlFilename);
		}
	}
}
