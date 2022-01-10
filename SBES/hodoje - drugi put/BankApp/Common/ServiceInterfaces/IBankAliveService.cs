using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.ServiceInterfaces
{
	[ServiceContract]
	public interface IBankAliveService
	{
		[OperationContract]
		void CheckIfBankAlive(string callingSector);
	}
}
