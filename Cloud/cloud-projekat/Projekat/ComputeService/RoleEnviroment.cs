using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

namespace ComputeService
{

    public class RoleEnviroment : IRoleEnvironment
    {
        public static int[] ports = { 10010, 10020, 10030, 10040 };
        public static Dictionary<int, bool> inUse = new Dictionary<int, bool>();
          
        public string[] BrotherInstances(string myAssemblyName, string myAddress)
        {
            if (myAssemblyName != "Assembly.dll")
                return null;
            string[] Brothers = new string[3];
            int i = 0;
         

            foreach (int port in inUse.Keys)
            {
                if (inUse[port] && !string.Format("net.tcp://localhost:{0}/Container", port).Equals(myAddress)) 
                    Brothers[i++] = port.ToString();
            }

            return Brothers;
        }

        public string GetAddress(string myAssemblyName, string containerId)
        {
            if (myAssemblyName != "Assembly.dll")
                return "";
            string address = string.Format("net.tcp://localhost:{0}/Container", Int32.Parse(containerId));

            return address;
        }
    }
}
