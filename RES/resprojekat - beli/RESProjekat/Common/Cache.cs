using Common.Contracts;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common
{
	public class Cache
	{
		private static object padlock = new object();
		private static Cache instance;
		public static Dictionary<string, List<Generator>> LoggedLcs = new Dictionary<string, List<Generator>>();
		public static Window SystemController { get; set; }

		public static Cache Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new Cache();
					}

					return instance;
				}
			}
		}

		public void InitializeCache(Window systemController)
		{
			SystemController = systemController;
		}

		public LoginRequestResponse Login(List<Generator> lcGenerators)
		{
			Random r = new Random();
            string newLcGuid = Guid.NewGuid().ToString();
			double updateServicePointPort = r.Next(4000, 65000);

			LoginRequestResponse lrr = new LoginRequestResponse(newLcGuid, updateServicePointPort);

			LoggedLcs.Add(newLcGuid, lcGenerators);
			(SystemController as dynamic).Login(lrr, LoggedLcs[newLcGuid]);

			return lrr;
		}

	}
}
