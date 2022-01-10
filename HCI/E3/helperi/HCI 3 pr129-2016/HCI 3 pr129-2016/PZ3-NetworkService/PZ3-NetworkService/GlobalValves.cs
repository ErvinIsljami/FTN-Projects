using PZ3_NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PZ3_NetworkService
{
    public static class GlobalValves
    {
        private static ObservableCollection<Valve> allObjects = new ObservableCollection<Valve>();
        public static ObservableCollection<Valve> AllObjects { get => allObjects; set => allObjects = value; }

        public static Dictionary<string, Valve> DDbase { get; set; } = new Dictionary<string, Valve>();
        public static Dictionary<string, TextBlock> NVState { get; set; } = new Dictionary<string, TextBlock>();
        public static Dictionary<string, Border> NVBorder { get; set; } = new Dictionary<string, Border>();

		public static Dictionary<string, ValveType> ValveTypes { get; set; } = new Dictionary<string, ValveType>();

        static GlobalValves()
        {
            string imgGas = Environment.CurrentDirectory + @"\" + "gas.jpg";
            string imgWater = Environment.CurrentDirectory + @"\" + "vodeni.jpg";

			ValveTypes.Add("Gas", new ValveType("Gas", imgGas));
			ValveTypes.Add("Water", new ValveType("Water", imgWater));

            AllObjects.Add(new Valve { ID = 6, Name = "Valve 1", ValveType = ValveTypes["Gas"], Val = 0 });
            AllObjects.Add(new Valve { ID = 21, Name = "Valve 2", ValveType = ValveTypes["Gas"], Val = 0 });
            AllObjects.Add(new Valve { ID = 1337, Name = "Valve 3", ValveType = ValveTypes["Water"], Val = 0 });
            AllObjects.Add(new Valve { ID = 9000, Name = "Valve 4", ValveType = ValveTypes["Water"], Val = 0 });
            
        }

    }
}
