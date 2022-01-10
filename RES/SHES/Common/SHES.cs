using Common;
using Common.Communication;
using Common.Wrappers;
using Common.SHES_Components;
using Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SHES 
    {
        private double currentPower;
        public double CurrentPower
        { get
            {
                return currentPower;
            }
            set
            {
                currentPower = value;
            }
        }

        public BatteryList Batteries { get; set; }
        public ConsumersList Consumers { get; set; }
        public SolarPanelList SolarPanels { get; set; }
        public Utility Utility { get; set; }
        private bool isConsuming;
        private ISHESToComponentsQueues queues;
        public SHES(ISHESToComponentsQueues queues)
        {
            CurrentPower = 0;
            Batteries = new BatteryList(ref queues);
            Consumers = new ConsumersList(ref queues);
            SolarPanels = new SolarPanelList(ref queues);
            
            Utility = new Utility(15);
            isConsuming = false;
            this.queues = queues;
        }

        public double GetDiff()
        {
            DateTime CurrentTime = GlobalClock.Instance.GetCurrentTime();
            int second = GlobalClock.Instance.Second;


            if (CurrentTime.Hour >= 3 && CurrentTime.Hour <= 6 && isConsuming)
            {
                isConsuming = false;
                Response r = new Response(0, "");
            }
            else if (CurrentTime.Hour >= 14 && CurrentTime.Hour <= 17 && isConsuming == true)
            {
                isConsuming = true;
                Response r = new Response(1, "");
            }

            double diff = 0;

            while (queues.BatteryRequest.Count > 0)
            {
                Request request;
                if (queues.BatteryRequest.TryDequeue(out request))
                {
                    diff += request.PowerDiff;
                }
            }

            while (queues.ConsumersRequest.Count > 0)
            {
                Request request;
                if (queues.ConsumersRequest.TryDequeue(out request))
                {
                    diff += request.PowerDiff;
                }
            }

            while (queues.SolarRequest.Count > 0)
            {
                Request request;
                if (queues.SolarRequest.TryDequeue(out request))
                {
                    diff += request.PowerDiff;
                }
            }

            CurrentPower += diff;

            return diff;
        }

    }
}
