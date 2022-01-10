using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    
    public enum EDeviceType : int
    {
        [Description("DIGITAL")]
        Digital = 1,
        [Description("ANALOG")]
        Analog
    }
}
