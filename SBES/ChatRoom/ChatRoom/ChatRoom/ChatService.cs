using ChatRoom.Cryptography;
using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatRoom
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ChatService : IChatService
    {
        public void SendMessageToServer(byte[] message)
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity winIdentity = identity as WindowsIdentity;

            GroupsHelper.Instance.BroadcastMessage(message, winIdentity.Name);

            string groupRealName = "";
            string realName = winIdentity.Name.Split('\\')[1]; 
            string completeMessage = "";
            foreach (IdentityReference group in winIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var groupName = sid.Translate(typeof(NTAccount));
                if (groupName.ToString().Contains('\\'))
                {
                    groupRealName = groupName.ToString().Split('\\')[1];
                    if (GroupsHelper.Instance.CheckIfGroupExsits(groupRealName))
                    { 
                        completeMessage ="\nGroup: " + groupRealName + ",   User: " + realName + ",   message: " +ASCIIEncoding.ASCII.GetString( message)+ " .";
                        break;
                    }
                }
            }
            ServerSecondaryServerChannel channel = new ServerSecondaryServerChannel();
            channel.ProxyForward.SendMessageToSecondaryServer(completeMessage);
        }

        public void ConnectTo()
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity winIdentity = identity as WindowsIdentity;

            foreach (IdentityReference group in winIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var groupName = sid.Translate(typeof(NTAccount));
                if (groupName.ToString().Contains('\\'))
                {
                    string groupRealName = groupName.ToString().Split('\\')[1];
                    if (GroupsHelper.Instance.CheckIfGroupExsits(groupRealName))
                    {
                     GroupsHelper.Instance.AddUserToGroup(groupRealName, winIdentity.Name, Callback);
                        break;
                    }
                }
            }
        }

        public ICryptographyInterface GetCryptoAlg()
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity winIdentity = identity as WindowsIdentity;
            string groupRealName = "";
            foreach (IdentityReference group in winIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var groupName = sid.Translate(typeof(NTAccount));
                if (groupName.ToString().Contains('\\'))
                {
                    groupRealName = groupName.ToString().Split('\\')[1];
                    if (GroupsHelper.Instance.CheckIfGroupExsits(groupRealName))
                    {
                        var r = CryptoHelper.GetAlgForGroup(groupRealName);
                        return r;
       
                    }
                }
            }
            return null;
        }

        private IChatServiceCallback Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
            }
        }
    }
}