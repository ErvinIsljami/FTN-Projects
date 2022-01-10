using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Common
{
	public class DataAccessHelper
	{
		private static DataAccessHelper instance = null;
		private static readonly object padlock = new object();
		private ObservableCollection<Generator> generators;
		private ObservableCollection<Group> groups;
		private Dictionary<string, ObservableCollection<Generator>> grupisanaBaza;

		public XmlReaderWriter ReaderWriter { get; set; }
		public ObservableCollection<LC> LCs { get; set; }
		public ObservableCollection<Generator> Generators { get => generators; set => generators = value; }
		public ObservableCollection<Group> Groups { get => groups; set => groups = value; }
		public Dictionary<string, ObservableCollection<Generator>> GrupisanaBaza { get => grupisanaBaza; set => grupisanaBaza = value; }

		private string lcsFilename = "../../../Data/lcs.xml";

		DataAccessHelper()
		{
			ReaderWriter = new XmlReaderWriter();
			LCs = new ObservableCollection<LC>();
			ReaderWriter.SerializeObject<ObservableCollection<LC>>(LCs, lcsFilename);
			UpdateDatabase();
		}
		public static DataAccessHelper Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new DataAccessHelper();
					}

					return instance;
				}
			}
		}

		public bool AddLcGenerator(string lcId, Generator g)
		{
			LC lc = LCs.FirstOrDefault(l => l.Guid == lcId);
			if (lc != null)
			{
				if(lc.Generators.FirstOrDefault(gen => gen.Id == g.Id) == null)
				{
					lc.Generators.Add(g);
					UpdateDatabase();
					return true;
				}
			}
			return false;
		}

		public void UpdateLcGenerators(string lcId, List<Generator> generators)
		{
			LC lc = LCs.FirstOrDefault(l => l.Guid == lcId);
			if(lc != null)
			{
				lc.Generators = generators;
			}
			else
			{
				LCs.Add(new LC(lcId, new List<Generator>()));
			}
			UpdateDatabase();
		}

		public void UpdateDatabase()
		{
			ReaderWriter.SerializeObject<ObservableCollection<LC>>(LCs, lcsFilename);
		}
	}
}
