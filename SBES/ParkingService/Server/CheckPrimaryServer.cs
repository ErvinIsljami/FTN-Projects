using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class CheckPrimaryServer : ICheckPrimaryServer
    {
        public bool CheckIfAlive()
        {
            return true;
        }
    }
}
