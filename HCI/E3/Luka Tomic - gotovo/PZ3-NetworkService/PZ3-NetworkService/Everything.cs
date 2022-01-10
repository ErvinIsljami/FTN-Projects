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
	public static class Everything
	{
		private static string xmlFilename;
		private static DataIO dataIO;
		public static double TemperatureMax;
		public static double TemperatureMin;
		private static string Turbo975Image;
		private static string HidroMer71Image;
		public static ObservableCollection<WaterMeter> WaterMeters;
		public static IEnumerable<WaterMeterType> WaterMeterTypes;
		public static Dictionary<WaterMeterType, string> WaterMeterTypesAndImagesMapper;
		public static IEnumerable<FilterConditionType> FilterConditionTypes;
		public static Dictionary<string, ObservableCollection<WaterMeter>> DataGridMap;
		public static CollectionViewSource FilterCollection { get; set; }

		static Everything()
		{
			xmlFilename = "waterMeters.xml";
			dataIO = new DataIO();
			TemperatureMax = 735;
			TemperatureMin = 670;
			Turbo975Image = "pack://application:,,,/Images/turbo975.png";
			HidroMer71Image = "pack://application:,,,/Images/hidroMer71.png";
			ObservableCollection<WaterMeter> waterMeters = LoadWaterMetersFromXml();
			if(waterMeters == null)
			{
				WaterMeters = new ObservableCollection<WaterMeter>();
			}
			else
			{
				WaterMeters = waterMeters;
			}
			FilterConditionTypes = Enum.GetValues(typeof(FilterConditionType)).Cast<FilterConditionType>();
			WaterMeterTypes = Enum.GetValues(typeof(WaterMeterType)).Cast<WaterMeterType>();
			WaterMeterTypesAndImagesMapper = new Dictionary<WaterMeterType, string>();
			WaterMeterTypesAndImagesMapper.Add(WaterMeterType.Turbo975, Turbo975Image);
			WaterMeterTypesAndImagesMapper.Add(WaterMeterType.HidroMer71, HidroMer71Image);
			DataGridMap = new Dictionary<string, ObservableCollection<WaterMeter>>();
			FilterCollection = new CollectionViewSource();
			FilterCollection.Source = WaterMeters;
		}

		public static void SaveWaterMetersToXml()
		{
			dataIO.SerializeObject<ObservableCollection<WaterMeter>>(WaterMeters, xmlFilename);
		}

		public static ObservableCollection<WaterMeter> LoadWaterMetersFromXml()
		{
			return dataIO.DeserializeObject<ObservableCollection<WaterMeter>>(xmlFilename);
		}
	}
}
