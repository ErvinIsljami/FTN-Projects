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

namespace PZ3_NetworkService
{
	public static class Container
	{
		private static string xmlFilename;
		private static DataIO dataIO;
		public static double TemperatureMax;
		public static double TemperatureMin;
		private static string ThermalImage;
		private static string FusionImage;
		public static CommandInvoker CommandInvoker;
		public static ObservableCollection<Reactor> Reactors;
		public static IEnumerable<ReactorType> ReactorTypes;
		public static Dictionary<ReactorType, string> ReactorTypesAndImagesMapper;
		public static Dictionary<string, ObservableCollection<Reactor>> DataGridMap;
		public static CollectionViewSource SearchCollection { get; set; }

		static Container()
		{
			xmlFilename = "reactors.xml";
			dataIO = new DataIO();
			TemperatureMax = 350;
			TemperatureMin = 250;
			ThermalImage = "pack://application:,,,/Images/thermal.png";
			FusionImage = "pack://application:,,,/Images/fusion.png";
			CommandInvoker = new CommandInvoker();
			ObservableCollection<Reactor> reactors = LoadReactorsFromXml();
			if(reactors == null)
			{
				Reactors = new ObservableCollection<Reactor>();
			}
			else
			{
				Reactors = reactors;
			}
			ReactorTypes = Enum.GetValues(typeof(ReactorType)).Cast<ReactorType>();
			ReactorTypesAndImagesMapper = new Dictionary<ReactorType, string>();
			ReactorTypesAndImagesMapper.Add(ReactorType.Thermal, ThermalImage);
			ReactorTypesAndImagesMapper.Add(ReactorType.Fusion, FusionImage);
			DataGridMap = new Dictionary<string, ObservableCollection<Reactor>>();
			SearchCollection = new CollectionViewSource();
			SearchCollection.Source = Reactors;
		}

		public static void SaveReactorsToXml()
		{
			dataIO.SerializeObject<ObservableCollection<Reactor>>(Reactors, xmlFilename);
		}

		public static ObservableCollection<Reactor> LoadReactorsFromXml()
		{
			return dataIO.DeserializeObject<ObservableCollection<Reactor>>(xmlFilename);
		}
	}
}
