using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.General.Cache
{
    public class ResourceCacheItem<T>
    {
        private T _item;
        private DateTimeOffset _expiresOn;

        public ResourceCacheItem(T item)
        {
            _item = item;
        }

        public T Item { get { return _item; } }
        public DateTimeOffset ExpiresOn { get; set; }
    }
}
