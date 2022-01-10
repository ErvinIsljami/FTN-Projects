using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace ItemRepositoryWorker
{
    class NotifyOthers : INotifyOthers
    {
        public void Notify(string type)
        {
            Trace.TraceInformation("Brat mi poslao: " + type);
        }
    }
}
