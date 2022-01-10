using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Hubs
{
    public interface ICustomer
    {
        void messageReceived(string msg);
    }
}
