using System;
using System.Collections.Generic;
using System.Linq;

namespace EAI.General.SettingJson
{
    internal class SerializerContext
    {
        private readonly List<object> _instances;
        private readonly List<Type> _types;

        public SerializerContext(IEnumerable<object> existingInstances)
        {
            existingInstances = existingInstances.Where(i => i != null);

            _instances = new List<object>(existingInstances);
            _types = new List<Type>(existingInstances.Select(i => i.GetType()));
        }

        internal bool HasAssignableType(Type objectType)
        {
            return _types.Any(type => objectType.IsAssignableFrom(type));
        }


        internal bool HasInstanceOfType(Type objectType)
        {
            return GetInstanceOfType(objectType) != null;
        }

        internal object GetInstanceOfType(Type objectType)
        {
            return _instances
                .FirstOrDefault(instance => objectType.IsAssignableFrom(instance.GetType()));
        }

        internal void AddInstance(object instance)
        {
            _instances.Add(instance);
            _types.Add(instance.GetType());
        }

        internal void AddType(Type type)
        {
            _types.Add(type);
        }
    }
}