using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankServiceApp.ServiceHosts
{
    public interface IServiceHost
    {
        void OpenService();

        void CloseService();
    }
}
