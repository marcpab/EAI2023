using EAI.General.SettingJson;
using EAI.General.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.GenericServer
{
    public class ServiceStartup
    {
        // settings ConfigStorageType
        // settings AzureWebJobsStorage
        // settings ConfigRootPath
        // settings StorageConfigBlobName

        public IEnumerable<IService> Services { get; set; }

        public async Task InitializeAsync(Type configType)
        {
            var config = await ConfigurationHandler.Instance.GetConfigurationAsync(configType);
            var configJson = config.ToString();

            new SettingJsonHandler().DeserializeInstance(this, configJson);
        }

        public Task RunAsync(CancellationToken cancellationToken)
        {
            var tasks = Services
                .Where(s => s != null)
                .Select(s => Task.Run(() => s.RunAsync(cancellationToken)))
                .ToArray();

            return Task.WhenAll(tasks);
        }

    }

}
