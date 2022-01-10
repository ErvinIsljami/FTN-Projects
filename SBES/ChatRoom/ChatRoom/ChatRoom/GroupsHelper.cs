using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom
{
    public class GroupsHelper
    {
        private static GroupsHelper _instance = null;
        private static object _padLock = new object();

        private Dictionary<string, HashSet<string>> _groupsWithUsers = new Dictionary<string, HashSet<string>>();

        private Dictionary<string, IChatServiceCallback> _userCallbacks = new Dictionary<string, IChatServiceCallback>();

        public static GroupsHelper Instance
        {
            get
            {
                lock (_padLock)
                {
                    if (_instance == null)
                        _instance = new GroupsHelper();

                    return _instance;
                }
            }
        }

        public GroupsHelper()
        {  
            List<string> groups = ConfigurationManager.AppSettings["groups"].Split(',').ToList();
            foreach (string group in groups)
            {
                _groupsWithUsers[group] = new HashSet<string>();
            }
        }

        public bool CheckIfGroupExsits(string groupName)
        {
            return _groupsWithUsers.ContainsKey(groupName);
        }

        public void AddUserToGroup(string groupName, string userName, IChatServiceCallback callBack)
        {
            if (_groupsWithUsers.ContainsKey(groupName))
            {
                _groupsWithUsers[groupName].Add(userName); 
                _userCallbacks[userName] = callBack;        
            }
        }

        public void BroadcastMessage(byte[] message, string userName)
        {
            string group = GetGroupForUser(userName);
            if (group != null)
            {
                foreach (var user in _groupsWithUsers[group])
                {
                    _userCallbacks[user].SendMessageToClients($"{userName}:  ", message);
                }
            }
        }

        private string GetGroupForUser(string userName)
        {
            foreach (var groupName in _groupsWithUsers.Keys)   
            {
                if (_groupsWithUsers[groupName].Contains(userName))
                    return groupName;
            }
            return null;
        }

        public void AddUserToGroup2(string groupName, string userName)
        {
            if (_groupsWithUsers.ContainsKey(groupName))
            {
                _groupsWithUsers[groupName].Add(userName);  
            }
        }
    }
}