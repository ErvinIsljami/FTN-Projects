using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Model
{
    public static class Types
    {
        public static List<Type> TypesList = new List<Type>
        {
            new Type("Select type",""),
            new Type("IA","pack://application:,,,/Images/valve1.png"),
            new Type("IB","pack://application:,,,/Images/valve2.png")
        };

        public static List<string> TypesNames = new List<string>
        {
            "Select type",
            "IA",
            "IB"
        };
    }
}
