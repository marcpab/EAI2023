using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.General.Cache
{
    public class ResourceCacheItem<T>
    {
        private readonly T _item;

        public ResourceCacheItem(T item)
        {
            _item = item;
        }

        public T Item { get { return _item; } }
        public DateTimeOffset ExpiresOn { get; set; }
        public Func<Task> OnRemovedAsync { get; set; }
    }
}
