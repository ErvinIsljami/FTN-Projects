using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ProcessModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ProcessModel(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public ProcessModel()
        {
        }
    }
}
