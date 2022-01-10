using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace JobWorker
{
    class BrotherConnection : IBrotherConnection
    {
        public bool AreYouAlive()
        {
            return true;
        }
    }
}
