using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
	public class SectorAdditionalConfig
	{
		[JsonProperty("address")]
		public string Address { get; set; }
		[JsonProperty("endpointName")]
		public string EndpointName { get; set; }
		[JsonProperty("sectorResponseAddress")]
		public string SectorResponseAddress { get; set; }
		[JsonProperty("sectorResponseEndpointName")]
		public string SectorResponseEndpointName { get; set; }
	}
}
