using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3.Models
{
    public class GridPoint
    {
        public Tuple<int, int> Key { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool Taken { get; set; }

        public GridPoint() { }

        public GridPoint(int x, int y)
        {
            X = x;
            Y = y;
            Key = new Tuple<int, int>(X, Y);
            Taken = false;
        }
    }
}
