using Common;
using Common.Contracts;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SystemController.Services
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class LcToScService : ILcToScService
	{
		public static object lockObj = new object();

		public LoginRequestResponse Login(List<Generator> lcGenerators)
		{
			LoginRequestResponse lrr = Cache.Instance.Login(lcGenerators);
			(Cache.SystemController as dynamic).UpdateLcGenerators(lrr.LcId, lcGenerators);
			return lrr;
		}

		public void SendMeasurements(string lcGuid, List<Generator> lcGenerators)
		{
			lock (lockObj)
			{
				if (Cache.LoggedLcs.ContainsKey(lcGuid))
				{
					Cache.LoggedLcs[lcGuid] = lcGenerators;
					(Cache.SystemController as dynamic).UpdateLcGenerators(lcGuid, lcGenerators);
				}
			}
		}
	}
}
