using Common.CertificateManagement;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Communication.AuthorizationPolicy
{
	/// <summary>
	/// Organizational unit authorization policy.
	/// </summary>
	public class OUAuthorizationPolicy : IAuthorizationPolicy
	{
		public OUAuthorizationPolicy()
		{
			Id = Guid.NewGuid().ToString();
		}

		public string Id { get; }

		public ClaimSet Issuer { get { return ClaimSet.System; } }

		public bool Evaluate(EvaluationContext evaluationContext, ref object state)
		{
			object list;
			if (!evaluationContext.Properties.TryGetValue("Identities", out list))
			{
				return false;
			}

			IList<IIdentity> identities = list as IList<IIdentity>;
			if (list == null || identities.Count <= 0)
			{
				return false;
			}

			string username = StringFormatter.GetAttributeFromSubjetName(identities[0].Name, "CN");
			string organizationalUnit = StringFormatter.GetAttributeFromSubjetName(identities[0].Name, "OU");

			if (String.IsNullOrEmpty(username))
			{
				return false;
			}

			evaluationContext.Properties["Principal"] = new OUPrincipal(new GenericIdentity(username), organizationalUnit);
			return true;
		}
	}
}
