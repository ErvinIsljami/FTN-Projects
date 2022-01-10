using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace DataProxy
{
    public interface ICacheManager<T> where T : class
    {
        ObjectCache CachedData { get; }

        //T Get(string key);
        IEnumerable<T> Get(string key);

        void Set(string key, object data, int cacheTime);
        bool IsSet(string key);

        //void Remove(string key);
        //void Clear();
    }
}
