using Newtonsoft.Json;

namespace EAI.General.SettingJson
{
    /// <summary>
    /// Wrapper to handle setting JSON file
    /// </summary>
    public class SettingJsonHandler
    {
        private readonly SerializerContext _context;
        private readonly JsonSerializerSettings _settings;

        public SettingJsonHandler(params object[] instances)
        {
            _context = new SerializerContext(instances);
            _settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new ContractResolver(_context),
                Converters = new JsonConverter[]
                        {
                            new InstanceConverter(_context)
                        }
            };
        }

        public void DeserializeInstance(object instance, string json)
        {
            _context.AddInstance(instance);

            JsonConvert.DeserializeObject(json, instance.GetType(), _settings);
        }
    }
}