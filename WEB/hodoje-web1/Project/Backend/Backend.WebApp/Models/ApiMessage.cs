using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class ApiMessage<V, T> : IApiMessage<V, T> where T : class
    {
        public V Key { get; set; }
        public T Data { get; set; }
    }
}