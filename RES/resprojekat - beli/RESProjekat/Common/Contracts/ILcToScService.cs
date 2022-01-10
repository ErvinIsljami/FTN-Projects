using Common.Model;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Common.Contracts
{
	[ServiceContract]
	public interface ILcToScService
	{
		[OperationContract]
		LoginRequestResponse Login(List<Generator> lcGenerators);
		[OperationContract]
		void SendMeasurements(string lkGuid, List<Generator> lkGenerators);
	}
}
