using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace Backend.AccessServices
{
    public class CacheManager<V, T> : ICacheManager<V, T> where T : class
    {
        public ObjectCache CachedData => MemoryCache.Default;

        public IDictionary<V, T> Get(string key)
        {
            IDictionary<V, T> data = new ConcurrentDictionary<V, T>();
            if (!String.IsNullOrEmpty(key))
            {
                data = (IDictionary<V, T>)CachedData[key];
            }
            return data;
        }

        public bool IsSet(string key)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(key))
            {
                result = CachedData.Contains(key);
            }
            return result;
        }

        public void Remove(string key)
        {
            CachedData.Remove(key);
        }

        public void Set(string key, object data, int cacheTime)
        {
            if (data != null && !String.IsNullOrEmpty(key))
            {
                CacheItemPolicy policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(cacheTime)
                };

                CachedData.Set(new CacheItem(key, data), policy);
            }
        }
    }
}