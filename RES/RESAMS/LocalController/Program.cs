using LocalController.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalController
{
    class Program
    {
        static void Main(string[] args)
        {
            string lcName = ControllerOperations.ChooseName();
            ControllerDeviceServiceHost service = new ControllerDeviceServiceHost(lcName);
            service.OpenService();


            ControllerOperations.Menu();

        }
    }
}
