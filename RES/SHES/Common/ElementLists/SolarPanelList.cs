using Common.Communication;
using Common.SHES_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Wrappers
{
    public class SolarPanelList
    {
        public List<SolarPanel> List { get; set; }
        public double SunPower { get; set; }
        public ISHESToComponentsQueues Queues { get; set; }

        public SolarPanelList(ref ISHESToComponentsQueues queues)
        {
            List = new List<SolarPanel>();
            Queues = queues;
            Task.Factory.StartNew(() => SolarThread());
        }

        public SolarPanelList()
        {
            List = new List<SolarPanel>();
            Task.Factory.StartNew(() => SolarThread());
        }

        public double Generate()
        {
            double retVal = 0;
            foreach (var solarPanel in List)
            {
                retVal += solarPanel.MaxPower * SunPower;
            }
            return retVal;
        }

        public async void SolarThread()
        {
            while (true)
            {
                while (Queues.SolarReponses.Count > 0)
                {
                    Response response;
                    if(Queues.SolarReponses.TryDequeue(out response))
                    {
                        this.SunPower = response.Value;
                    }
                }

                double retVal = Generate();

                Request r = new Request(retVal, "");

                Queues.BatteryRequest.Enqueue(r);
                await Task.Delay(GlobalClock.Instance.Second);
            }
        }
    }
}
