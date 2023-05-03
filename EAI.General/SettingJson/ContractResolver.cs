using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EAI.General.SettingJson
{
    internal class ContractResolver : IContractResolver
    {
        private readonly SerializerContext _context;
        private readonly IContractResolver _defaultResolver;
        private readonly SerializationCallback _onDeserializedCallback;


        public ContractResolver(SerializerContext context)
        {
            _context = context;
            _defaultResolver = new DefaultContractResolver();
            _onDeserializedCallback = new SerializationCallback(OnDeserialized);
        }

        public JsonContract ResolveContract(Type type)
        {
            var contract = _defaultResolver.ResolveContract(type);
            if (!contract.OnDeserializingCallbacks.Contains(_onDeserializedCallback))
                contract.OnDeserializingCallbacks.Add(_onDeserializedCallback);


            var existingInstance = _context.GetInstanceOfType(type);
            if (existingInstance != null)
            {
                contract.DefaultCreator = () => existingInstance;
                return contract;
            }

            var isSingleton = type
                .GetCustomAttributes(typeof(SingletonAttribute), true)
                .Any();

            if (!isSingleton)
            {
                return contract;
            }

            var creator = contract.DefaultCreator;

            contract.DefaultCreator = () =>
            {
                var instance = creator();

                _context.AddInstance(instance);

                return instance;
            };

            return contract;
        }


        private void OnDeserialized(object obj, StreamingContext context)
        {
            PromoteInstances(obj);
        }

        private void PromoteInstances(object obj)
        {
            if (obj == null)
                return;

            foreach (var property in obj.GetType().GetProperties())
            {
                if (!property.CanWrite)
                    continue;

                var objectType = property.PropertyType;

                var instance = _context.GetInstanceOfType(objectType);

                if (instance != null && (objectType.IsInterface || instance.GetType() == objectType))
                    property.SetValue(obj, _context.GetInstanceOfType(objectType));

            }
        }
    }
}