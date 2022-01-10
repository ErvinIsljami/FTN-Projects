using Common.CertificateManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Common.Communication.AuthorizationPolicy
{
	public class OUPrincipal : IPrincipal
	{
		private IIdentity identity;
		private string ouGroup;

		public OUPrincipal(IIdentity identity, string ouGroup)
		{
			this.identity = identity;
			this.ouGroup = ouGroup;
		}

		public IIdentity Identity { get { return identity; } }

		public bool IsInRole(string role)
		{
			return ouGroup.Equals(role);
		}
	}
}
