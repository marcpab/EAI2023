using EAI.General.SettingJson;
using EAI.General.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EAI.AzureFunctions
{
    public class Function
    {
        protected ILogger _logger;

        protected T GetMessage<T>(string messageContent)
        {
            if (typeof(T) == typeof(string))
                return (T)(object)messageContent;

            return JsonConvert.DeserializeObject<T>(messageContent);
        }

        protected virtual Task InitializeTestAsync()
        {
            return InitializeAsync(NullLoggerFactory.Instance);
        }

        protected virtual async Task InitializeAsync(ILoggerFactory loggerFactory)
        {
            var currentType = GetType();

            _logger = loggerFactory.CreateLogger(currentType);

            var config = await ConfigurationHandler.Instance.GetConfigurationAsync(currentType);
            var configJson = config.ToString();

            new SettingJsonHandler(_logger).DeserializeInstance(this, configJson);
        }
    }
}