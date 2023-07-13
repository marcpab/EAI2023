using EAI.General;
using EAI.General.SettingJson;
using EAI.General.Settings;
using EAI.LoggingV2;
using EAI.LoggingV2.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.GenericServer
{
    public class ServiceHost : IServiceHost
    {
        // settings ConfigStorageType
        // settings AzureWebJobsStorage
        // settings ConfigRootPath
        // settings StorageConfigBlobName

        public LoggerV2 Log { get; set; }

        public string Stage { get; set; }

        public IEnumerable<IService> Services { get; set; }

        public async Task InitializeAsync(Type configType)
        {
            var config = await ConfigurationHandler.Instance.GetConfigurationAsync(configType);
            var configJson = config.ToString();

            new SettingJsonHandler().DeserializeInstance(this, configJson);
        }

        public Task RunAsync(CancellationToken cancellationToken)
        {
            var type = GetType();

            ProcessContext.Create(null, Stage, type.FullName);

            LogStartup(type);

            var tasks = Services
                .Where(s => s != null)
                .Select(s => Task.Run(() => s.RunAsync(cancellationToken)))
                .ToArray();

            return Task.WhenAll(tasks);
        }

        private void LogStartup(Type type)
        {
            Log?.Start<Info>(null, null, $"Start {type.FullName}, Version {type.Assembly.GetName().Version}");
            Log?.String<Info>($"Version {type.Assembly.GetName().Version}");
            Log?.String<Info>($"Runtime Version {System.Environment.Version}");

            foreach (var service in Services)
                Log?.String<Info>($"Service {service.GetType().FullName}");
        }

        public IEnumerable<T> GetServices<T>()
            where T : class
        {
            return Services.Where(s => s != null && typeof(T).IsAssignableFrom(s.GetType())).Select(s => s as T);
        }
    }

}
