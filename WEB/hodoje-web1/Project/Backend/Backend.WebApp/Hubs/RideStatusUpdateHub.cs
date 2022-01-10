using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Backend.Hubs
{
    public class RideStatusUpdateHub : Hub<ICustomer>
    {
        public void sendMessage(string message)
        {
            Clients.All.messageReceived($"Message '{message}' received at: {DateTime.Now.ToString()}");
        }
    }
}