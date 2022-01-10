using System.Collections.Concurrent;

namespace Common.Communication
{
    public interface ISHESToComponentsQueues
    {
        ConcurrentQueue<Request> BatteryRequest { get; set; }
        ConcurrentQueue<Response> BatteryResponses { get; set; }
        ConcurrentQueue<Request> ConsumersRequest { get; set; }
        ConcurrentQueue<Response> SolarReponses { get; set; }
        ConcurrentQueue<Request> SolarRequest { get; set; }

    }
}