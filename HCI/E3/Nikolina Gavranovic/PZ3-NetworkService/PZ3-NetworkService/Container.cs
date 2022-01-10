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
		public static double IAMax;
		public static double IBMax;
		private static string IAImage;
		private static string IBImage;
		public static CommandInvoker CommandInvoker;
		public static ObservableCollection<Road> Roads;
		public static IEnumerable<RoadType> RoadTypes;
		public static Dictionary<RoadType, string> RoadTypesAndImagesMapper;
		public static Dictionary<string, ObservableCollection<Road>> DataGridMap;
		public static CollectionViewSource SearchCollection { get; set; }

		static Container()
		{
			xmlFilename = "roads.xml";
			dataIO = new DataIO();
			IAMax = 15000;
			IBMax = 7000;
			IAImage = "pack://application:,,,/Images/roadA.png";
			IBImage = "pack://application:,,,/Images/roadB.png";
			CommandInvoker = new CommandInvoker();
			ObservableCollection<Road> roads = LoadRoadsFromXml();
			if(roads == null)
			{
				Roads = new ObservableCollection<Road>();
			}
			else
			{
				Roads = roads;
			}
			RoadTypes = Enum.GetValues(typeof(RoadType)).Cast<RoadType>();
			RoadTypesAndImagesMapper = new Dictionary<RoadType, string>();
			RoadTypesAndImagesMapper.Add(RoadType.IA, IAImage);
			RoadTypesAndImagesMapper.Add(RoadType.IB, IBImage);
			DataGridMap = new Dictionary<string, ObservableCollection<Road>>();
			SearchCollection = new CollectionViewSource();
			SearchCollection.Source = Roads;
		}

		public static void SaveRoadsToXml()
		{
			dataIO.SerializeObject<ObservableCollection<Road>>(Roads, xmlFilename);
		}

		public static ObservableCollection<Road> LoadRoadsFromXml()
		{
			return dataIO.DeserializeObject<ObservableCollection<Road>>(xmlFilename);
		}
	}
}
