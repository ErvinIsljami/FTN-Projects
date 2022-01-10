﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Communication
{
    public class SHESToComponentsQueueMock : ISHESToComponentsQueues
    {
        public ConcurrentQueue<Request> BatteryRequest { get; set; }
        public ConcurrentQueue<Response> BatteryResponses { get; set; }
        public ConcurrentQueue<Request> SolarRequest { get; set; }
        public ConcurrentQueue<Response> SolarReponses { get; set; }
        public ConcurrentQueue<Request> ConsumersRequest { get; set; }

        public SHESToComponentsQueueMock()
        {
            BatteryRequest = new ConcurrentQueue<Request>();
            BatteryResponses = new ConcurrentQueue<Response>();
            SolarRequest = new ConcurrentQueue<Request>();
            SolarReponses = new ConcurrentQueue<Response>();
            ConsumersRequest = new ConcurrentQueue<Request>();

            BatteryRequest.Enqueue(new Request(100, "test"));
            BatteryResponses.Enqueue(new Response(100, "test"));
            SolarRequest.Enqueue(new Request(100, "test"));
            SolarReponses.Enqueue(new Response(100, "test"));
            //ConsumersRequest.Enqueue(new Request(100, "test"));
        }
        

    }
}
