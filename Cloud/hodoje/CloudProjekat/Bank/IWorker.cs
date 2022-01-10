using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public interface IWorker
    {
        void Start(string containerId);
        void Stop();
    }
}
