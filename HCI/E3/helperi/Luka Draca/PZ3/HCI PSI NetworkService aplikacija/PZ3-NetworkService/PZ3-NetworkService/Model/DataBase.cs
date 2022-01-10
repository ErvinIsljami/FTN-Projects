using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PZ3_NetworkService.Model
{
    public static class DataBase
    {
        public static Dictionary<int, ValveModel> Valve_MainStorage { get; set; } = new Dictionary<int, ValveModel>();
        public static Dictionary<string,ValveModel> ValveCanvas_Storage { get; set; } = new Dictionary<string, ValveModel>();
        public static int IACount = 0;
        public static int IBCount = 0;
        public static string logtext="";
        public static int ItemsCount = 0;
    }
}
