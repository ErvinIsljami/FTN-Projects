using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondaryServer
{
    public class SecondaryServer : ISecondaryServer
    {
        public void SendMessageToSecondaryServer(string message)
        {
            Console.WriteLine(message);
        }
    }
}