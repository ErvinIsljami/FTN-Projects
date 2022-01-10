using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationServer
{
    class ReplicationServer : IReplication
    {
        public void SendMessageToReplicationServer(string message)
        {
            Console.WriteLine(message);
            LogHandler.Instance.WriteMessage(message);
        }
    }
}
