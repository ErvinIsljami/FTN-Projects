using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    class Item
    {
        public int Code { get; set; }
        public double Value { get; set; }

        public Item()
        {
        }

        public Item(int code, double value)
        {
            Code = code;
            Value = value;
        }
    }
}
