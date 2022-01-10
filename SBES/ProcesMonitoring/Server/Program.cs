using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Server
{
    class Program
    {
        static ProcessServiceHost serviceHost = new ProcessServiceHost();
        static void Main(string[] args)
        {
            serviceHost.StartService();
            ProxyLog.Proxy.LogEvent("Process service started.", ECriticalLvl.INFORMATION, DateTime.Now);
            Task checkInvalidProcessesTask = Task.Factory.StartNew(() => CheckProcesses());  

            Console.ReadLine();
            serviceHost.StopService();
            ProxyLog.Proxy.LogEvent("Process service stoped.", ECriticalLvl.INFORMATION, DateTime.Now);
        }

        static async void CheckProcesses()
        {
            Console.WriteLine("Check process task started.");
            ProxyLog.Proxy.LogEvent("Check process task started.", ECriticalLvl.INFORMATION, DateTime.Now);
            while (true)
            {
                await Task.Delay(15000);

                ServerDatabase.Instance.CheckInvalidProcess();
            }
        }
    }
}
