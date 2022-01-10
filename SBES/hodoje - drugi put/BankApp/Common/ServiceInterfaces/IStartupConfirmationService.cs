using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.ServiceInterfaces
{
	[ServiceContract]
	public interface IStartupConfirmationService
	{
		[OperationContract]
		void ConfirmStartup(string sectorType);
	}
}
