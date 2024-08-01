using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Infrastructure
{
    public static class MemoryCacheManager
    {
        public static bool EnableCache = true;
        public static void Add<T>(string key, T o)
        {
            try
            {
                if (o != null)//https://github.com/mikeedwards83/Glass.Mapper/issues/283
                {
                    if (CacheEnable())
                    {
                        if (!Exists(key))
                        {

                            ObjectCache defCache = MemoryCache.Default;
                            defCache.Add(key, o, new CacheItemPolicy { Priority = CacheItemPriority.Default });

                        }
                    }
                }
            }
            catch 
            {
            }
        }

        private static bool CacheEnable()
        {
            return EnableCache;
        }


        public static void Clear()
        {
            ObjectCache defCache = MemoryCache.Default;
            foreach (var item in defCache)
            {
                defCache.Remove(item.Key);
            }
        }
        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>

        public static bool Clear(string key)
        {
            var keyDeleted = false;
            if (CacheEnable())
            {
                try
                {
                    if (Exists(key))
                    {
                        ObjectCache defCache = MemoryCache.Default;
                        defCache.Remove(key);

                        keyDeleted = true;
                    }
                }
                catch 
                {
                }
            }
            return keyDeleted;
        }

        public static bool Exists(string key)
        {
            ObjectCache defCache = MemoryCache.Default;
            return defCache[key] != null;
        }



        public static T Get<T>(string key)
        {
            try
            {
                if (CacheEnable())
                {
                    if (Exists(key))
                    {
                        ObjectCache defCache = MemoryCache.Default;
                        return (T)defCache[key];
                    }
                }
                return default;
            }
            catch 
            {
                return default;
            }
        }



        public static bool ClearFromTablePrefix(string key)
        {
            var keyDeleted = false;
            if (CacheEnable())
            {
                var cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
                var cacheItem = new List<string>();
                foreach (var cacheKey in cacheKeys)
                {
                    if (cacheKey.StartsWith(key))
                        cacheItem.Add(cacheKey);
                }
                try
                {
                    foreach (var newkey in cacheItem)
                    {
                        ObjectCache defCache = MemoryCache.Default;
                        defCache.Remove(key);
                        //if (LogCacheRelatedActivities)
                        {
                        }
                    }
                    keyDeleted = true;
                }
                catch (Exception ex)
                {
                }

            }
            return keyDeleted;
        }



        public static bool RemoveAllCache()
        {
            try
            {
                if (CacheEnable())
                {
                    var cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
                    foreach (var cacheKey in cacheKeys)
                    {
                        MemoryCache.Default.Remove(cacheKey);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }


}
