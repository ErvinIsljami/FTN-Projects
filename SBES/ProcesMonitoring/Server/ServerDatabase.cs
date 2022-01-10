using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Server.Services;

namespace Server
{
    public class ServerDatabase
    {
        private static HashSet<long> _whiteList;
        public HashSet<ProcessModel> ActiveProcess;
        private static object _padLock = new object();
        private static ServerDatabase _instance = null;
        public static ServerDatabase Instance
        {
            get
            {
                lock (_padLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ServerDatabase();
                    }
                    return _instance;
                }
            }
        }
        private static Dictionary<long, int> _processCounters;
        private ServerDatabase()
        {
            _whiteList = new HashSet<long>();
            _processCounters = new Dictionary<long, int>();
            ActiveProcess = new HashSet<ProcessModel>();
            LoadWhiteList();
        }

        private void LoadWhiteList()
        {
            string[] whiteListIds = ConfigurationManager.AppSettings.Get("whiteList").Split(',');
            foreach(var id in whiteListIds)
            {
                int processId;
                if(!Int32.TryParse(id, out processId))
                {
                    continue;
                }
                _whiteList.Add(processId);
            }
        }

        public bool AddProcess(ProcessModel process)
        {
            if(ActiveProcess.Any(x => x.Id == process.Id))
            {
                return false;
            }
            
            return ActiveProcess.Add(process);
        }

        public bool IsProcessWhiteListed(int id)
        {
            return _whiteList.Contains(id);
        }

        public void CheckInvalidProcess()
        {
            foreach(var process in ActiveProcess)
            {
                if(_whiteList.Contains(process.Id))
                {
                    continue;
                }

                if(!_processCounters.ContainsKey(process.Id))
                {
                    _processCounters[process.Id] = 0;
                }
                _processCounters[process.Id]++;

                switch(_processCounters[process.Id])
                {
                    case 1:
                        ProxyLog.Proxy.LogEvent($"Unauthorized process - {process.Name}.", ECriticalLvl.INFORMATION, DateTime.Now);
                        break;
                    case 2:
                        ProxyLog.Proxy.LogEvent($"Unauthorized process - {process.Name}.", ECriticalLvl.WARNING, DateTime.Now);

                        break;
                    default:
                        ProxyLog.Proxy.LogEvent($"Unauthorized process - {process.Name}.", ECriticalLvl.CRITICAL, DateTime.Now);
                        break;
                }
            }
        }
    }
}
