using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Communication
{
    public class Response
    {
        public double Value { get; set; }
        public string Id { get; set; }

        public Response(double value, string id)
        {
            Value = value;
            Id = id;
        }

        public Response()
        {
        }
    }
}
