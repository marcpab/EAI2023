using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace EAI.General.SettingJson
{
    internal class InstanceConverter : JsonConverter
    {
        private readonly SerializerContext _context;
        private bool _canRead;
        private bool _canWrite;

        public InstanceConverter(SerializerContext context)
        {
            _context = context;
        }

        public override bool CanConvert(Type objectType)
        {
            _canRead = _context.HasInstanceOfType(objectType) && objectType.IsInterface;
            _canWrite = objectType.IsInterface && _context.HasAssignableType(objectType);

            if (objectType
                .GetCustomAttributes(true)
                .Contains(typeof(SingletonAttribute)))
                _context.AddType(objectType);

            return _canRead || _canWrite;
        }

        public override bool CanRead => _canRead;

        public override bool CanWrite => _canWrite;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var j = JObject.Load(reader);

            return _context.GetInstanceOfType(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();


        }
    }
}