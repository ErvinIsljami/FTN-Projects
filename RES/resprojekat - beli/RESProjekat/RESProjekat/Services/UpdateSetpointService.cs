using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Contracts;
using Common.Model;

namespace LocalController.Services
{
    public class UpdateSetpointService : IUpdateSetpoint
    {
        public void SetpointUpdate(List<SetpointArray> setpoints)
        {
            ThreadPool.QueueUserWorkItem(delegate { PowerUpdater.UpdateSetpoints(setpoints); });
        }
    }
}
