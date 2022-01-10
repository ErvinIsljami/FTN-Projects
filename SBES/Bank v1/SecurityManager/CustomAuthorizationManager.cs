using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class CustomAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            var principal = operationContext.ServiceSecurityContext.
                 AuthorizationContext.Properties["Identities"] as List<IIdentity>;

            return true;
            //return principal.IsInRole("Read");
        }
    }
}
