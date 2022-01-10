using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDevice
{
    class Program
    {
        static void Main(string[] args)
        {
            DeviceOperations.Device = DeviceOperations.DeviceTypeMenu();
            DeviceOperations.ConnectToService();
            DeviceOperations.UpdateDeviceValue();
            DeviceOperations.UserMenu();
        }
    }
}
