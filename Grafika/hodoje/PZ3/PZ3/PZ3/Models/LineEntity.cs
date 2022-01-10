using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3.Models
{
    public class LineEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsUnderground { get; set; }
        public double R { get; set; }
        public string ConductingMaterial { get; set; }
        public string LineType { get; set; }
        public double ThermalConstantHeat { get; set; }
        public double FirstEnd { get; set; }
        public double SecondEnd { get; set; }
        public List<Point> Vertices { get; set; }
    }
}
