using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ProcessService : IProcessService
    {
        [PrincipalPermission(SecurityAction.Demand, Role = "Show")]
        public string ShowActiveProcess()
        {
            string retVal = "************* ACTIVE PROCESS ***************\n";
            foreach(var process in ServerDatabase.Instance.ActiveProcess)
            {
                retVal += $"{process.Id}\t->\t{process.Name}\n";
            }
            return retVal;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Basic")]
        public void StartProcess(ProcessModel process)
        {
            if(!ServerDatabase.Instance.AddProcess(process))
            {
                throw new Exception("Process alredy started");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void StopAllProcess()
        {
            ServerDatabase.Instance.ActiveProcess.Clear();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Basic")]
        public void StopProcess(long id)
        {
            if(ServerDatabase.Instance.ActiveProcess.RemoveWhere(x => x.Id == id) == 0)
            {
                throw new Exception("No process with given id.");
            }
        }
    }
}
