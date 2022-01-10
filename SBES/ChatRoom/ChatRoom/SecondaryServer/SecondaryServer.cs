using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SecondaryServer
{
    public class SecondaryServer : ISecondaryServer
    {
        public void SendMessageToSecondaryServer(string message)
        {
            Console.WriteLine(message);
            LogHandler.Instance.WriteMessage(message);

            SecondaryReplicationChannel channel = new SecondaryReplicationChannel();
            channel.ProxyReplication.SendMessageToReplicationServer(message);
        }       
    }
}