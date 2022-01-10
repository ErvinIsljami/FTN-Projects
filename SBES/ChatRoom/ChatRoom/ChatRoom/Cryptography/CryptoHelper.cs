using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Cryptography
{
    public static class CryptoHelper
    {
        private static Dictionary<string, ICryptographyInterface> groupsAlgorithms;

        static CryptoHelper()
        {
            groupsAlgorithms = new Dictionary<string, ICryptographyInterface>();
            List<string> groups = ConfigurationManager.AppSettings["groups"].Split(',').ToList();

            foreach (var g in groups)
            {
                if (g == "Profesori")
                {
                    groupsAlgorithms.Add(g, new AES_Cryptography());
                }
                else
                {
                    groupsAlgorithms.Add(g, new INVERT_Cryptography());
                }
            }
        }

        public static ICryptographyInterface GetAlgForGroup(string g)
        {
            if (groupsAlgorithms.ContainsKey(g))
            {
                return groupsAlgorithms[g];
            }
            else
            {
                throw new Exception("Group doesnt exsits.");
            }
        }
    }
}