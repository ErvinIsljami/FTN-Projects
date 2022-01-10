using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
	[DataContract]
	public class LoginRequestResponse
	{
		string lcId;
		double updateServicePointPort;

		public LoginRequestResponse()
		{

		}

		public LoginRequestResponse(string lcId, double updateServicePointPort)
		{
			this.lcId = lcId;
			this.updateServicePointPort = updateServicePointPort;
		}

		[DataMember]
		public string LcId { get => lcId; set => lcId = value; }
		[DataMember]
		public double UpdateServicePointPort { get => updateServicePointPort; set => updateServicePointPort = value; }
	}
}
