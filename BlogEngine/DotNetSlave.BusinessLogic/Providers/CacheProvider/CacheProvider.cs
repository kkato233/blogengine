using System;
using System.Collections;
using System.Web.Caching;

namespace BlogEngine.Core.Providers.CacheProvider
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheProvider : CacheBase
    {
        private readonly Cache _cache;
        private readonly string _keyPreffix = Blog.CurrentInstance.Id + "_";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        public CacheProvider(Cache cache)
        {
            _cache = cache;
        }

        public override int Count
        {
            get { return _cache.Count; }
        }

        public override long EffectivePercentagePhysicalMemoryLimit
        {
            get { return _cache.EffectivePercentagePhysicalMemoryLimit; }
        }

        public override long EffectivePrivateBytesLimit
        {
            get { return _cache.EffectivePrivateBytesLimit; }
        }

        public override object this[string key]
        {
            get
            {
                return _cache[_keyPreffix + key];
            }
            set
            {
                _cache[_keyPreffix + key] = value;
            }
        }

        public override object Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            return _cache.Add(_keyPreffix + key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }

        public override object Get(string key)
        {
            return _cache.Get(_keyPreffix + key);
        }

        public override IDictionaryEnumerator GetEnumerator()
        {
            return _cache.GetEnumerator();
        }

        public override void Insert(string key, object value)
        {
            _cache.Insert(_keyPreffix + key, value);
        }

        public override void Insert(string key, object value, CacheDependency dependencies)
        {
            _cache.Insert(_keyPreffix + key, value, dependencies);
        }

        public override void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            _cache.Insert(_keyPreffix + key, value, dependencies, absoluteExpiration, slidingExpiration);
        }

        public override void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            _cache.Insert(_keyPreffix + key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }

        public override object Remove(string key)
        {
            return _cache.Remove(_keyPreffix + key);
        }
    }
}
