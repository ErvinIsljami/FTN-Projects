using Common.Communication;
using Common.SHES_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Wrappers
{
    public class BatteryList
    {
        public List<Battery> List { get; set; }
        public ISHESToComponentsQueues Queues { get; set; }
        public bool IsConsumig { get; set; }
        public BatteryList(ref ISHESToComponentsQueues queues)
        {
            List = new List<Battery>();
            this.Queues = queues;
            IsConsumig = false;
            Task.Factory.StartNew(() => BatteryThread());
        }

        public BatteryList()
        {
            List = new List<Battery>();
            IsConsumig = false;
            Task.Factory.StartNew(() => BatteryThread());
        }

        public double Consume()
        {
            double retVal = 0;
            foreach(var battery in List)
            {
                retVal -= battery.MaxPower / (battery.Capacity * 60);
            }
            return retVal;
        }

        public double Generate()
        {
            double retVal = 0;
            foreach (var battery in List)
            {
                retVal += battery.MaxPower / (battery.Capacity * 60);
            }
            return retVal;
        }

        public async void BatteryThread()
        {
            while(true)
            {
                while(Queues.BatteryResponses.Count > 0)
                {
                    Response response;
                    if(Queues.BatteryResponses.TryDequeue(out response))
                    {
                        IsConsumig = response.Value > 0;
                    }
                }

                double retVal = 0;
                if(IsConsumig)
                {
                    retVal = Consume();
                }
                else
                {
                    retVal = Generate();
                }

                Request r = new Request(retVal, "");

                Queues.BatteryRequest.Enqueue(r);

                await Task.Delay(GlobalClock.Instance.Second);
            }
        }
    }
}
