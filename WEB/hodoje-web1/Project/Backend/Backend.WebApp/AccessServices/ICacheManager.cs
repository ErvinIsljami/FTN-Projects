using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace Backend.AccessServices
{
    public interface ICacheManager<V, T> where T : class
    {
        ObjectCache CachedData { get; }

        IDictionary<V, T> Get(string key);

        void Set(string key, object data, int cacheTime);
        bool IsSet(string key);

        void Remove(string key);
    }
}