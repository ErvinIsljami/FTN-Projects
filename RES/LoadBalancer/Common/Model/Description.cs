using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    class Description
    {
        public string Id { get; set; }
        public int Dataset { get; set; }
        public List<Item> Items { get; private set; }

        public Description(string id, int dataset)
        {
            Id = id;
            Dataset = dataset;
            Items = new List<Item>();
        }

        public Description()
        {
            Items = new List<Item>();
        }
    }
}
