using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EAI.General.Cache
{
    public class ResourceCache<T>
    {
        private static Dictionary<string, ResourceCacheItem<T>> _cache = new Dictionary<string, ResourceCacheItem<T>>();
        private static DateTimeOffset _nextCleanup = DateTimeOffset.UtcNow;

        static ResourceCache()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            lock (_cache)
            {
                foreach (var removeItem in _cache
                    .ToArray())
                {
                    _cache.Remove(removeItem.Key);

                    CleanItem(removeItem.Value);
                }

                _cache = null;
            }
        }

        public static async Task<T> GetResourceAsync(string key, Func<Task<ResourceCacheItem<T>>> createResourceAsync, Func<ResourceCacheItem<T>, Task> updateResourceItemAsync = null)
        {
            if (_nextCleanup < DateTimeOffset.UtcNow)
            {
                _nextCleanup = DateTimeOffset.UtcNow.AddMinutes(1);

                _ = Task.Run(CleanUp);
            }

            ResourceCacheItem<T> item;
            lock (_cache)
                if (_cache.TryGetValue(key, out item))
                    if (item.ExpiresOn < DateTimeOffset.UtcNow)
                    {
                        _cache.Remove(key);

                        CleanItem(item);

                        item = null;
                    }

            if (item != null)
            {
                if(updateResourceItemAsync != null)
                    await updateResourceItemAsync(item);

                return item.Item;
            }

            item = await createResourceAsync();

            lock (_cache)
                if (!_cache.ContainsKey(key))
                    _cache.Add(key, item);

            if (updateResourceItemAsync != null)
                await updateResourceItemAsync(item);

            return item.Item;
        }

        private static void CleanItem(ResourceCacheItem<T> item)
        {
            (item.Item as IDisposable)?.Dispose();
            item.OnRemovedAsync?.Invoke();
        }

        private static void CleanUp()
        {
            lock (_cache)
            {
                foreach (var removeItem in _cache
                    .Where(c => c.Value.ExpiresOn < DateTimeOffset.UtcNow)
                    .ToArray())
                {
                    _cache.Remove(removeItem.Key);

                    CleanItem(removeItem.Value);
                }
            }
        }
    }
}