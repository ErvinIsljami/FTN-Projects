using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    class CustomPrincipal : IPrincipal
    {
        //promeniti da konstruktor prima onaj paramter 
        IIdentity identity = null;
        public CustomPrincipal(IIdentity windowsIdentity)
        {
            identity = windowsIdentity;
        }

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string permission)
        {
            foreach (IdentityReference group in ((WindowsIdentity)identity).Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));
                string groupName = Formatter.ParseName(name.ToString());
                string[] permissions;
                if (RoleConfig.GetPermissions(groupName, out permissions))
                {
                    bool isAllowed = permissions.Contains(permission);
                    return isAllowed;
                }
            }
            return false;
        }
    }
}
