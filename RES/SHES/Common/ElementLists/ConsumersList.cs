using Common.Communication;
using Common.SHES_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Wrappers
{
    public class ConsumersList
    {
        public List<Consumer> List { get; set; }
        public ISHESToComponentsQueues Queues { get; set; }

        public ConsumersList(ref ISHESToComponentsQueues queues)
        {
            List = new List<Consumer>();
            Queues = queues;
            Task.Factory.StartNew(() => ConsumerThread());
        }

        public ConsumersList()
        {
            List = new List<Consumer>();
            Task.Factory.StartNew(() => ConsumerThread());
        }

        public double Consume()
        {
            double retVal = 0;
            foreach (var consumer in List)
            {
                retVal -= consumer.Consumption;
            }
            return retVal;
        }

        public async void ConsumerThread()
        {
            while (true)
            {
                double retVal = Consume();
               
                Request r = new Request(retVal, "");

                Queues.ConsumersRequest.Enqueue(r);
                await Task.Delay(GlobalClock.Instance.Second);
            }
        }
    }
}
