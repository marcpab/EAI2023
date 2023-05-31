using EAI.General.Cache;
using EAI.General.SettingProperties;
using EAI.General.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EAI.General.Settings
{
    public class ConfigurationHandler
    {
        private static ConfigurationHandler _instance = new ConfigurationHandler();

        public static ConfigurationHandler Instance { get { return _instance; } }

        private string _storageConfigJson;
        private string _storageConfigBlobName;

        public string StorageConfig { get { return _storageConfigJson; } set { _storageConfigJson = value; } }
        public string StorageConfigBlobName { get { return _storageConfigBlobName; } set { _storageConfigBlobName = value; } }

        public async Task<JObject> GetConfigurationAsync(Type configType)
        {
            return await ResourceCache<JObject>.GetResourceAsync(
                configType.FullName, 
                async () => new ResourceCacheItem<JObject>(await CreateConfigurationAsync(configType)) {
                    ExpiresOn = DateTime.UtcNow.AddMinutes(5),
            });
        }

        private async Task<JObject> CreateConfigurationAsync(Type loadForType)
        {
            if (string.IsNullOrEmpty(_storageConfigJson))
            {
                var settings = SettingsHandler.GetConfigurationRoot();

                _storageConfigJson = JsonConvert.SerializeObject(
                    new BlobStorageDefaultConfig
                    {
                        Type = settings["ConfigStorageType"] ?? "EAI.AzureStorage.BlobStorage, EAI.AzureStorage",
                        ConnectionString = settings["AzureWebJobsStorage"],
                        RootPath = settings["ConfigRootPath"]
                    });

                _storageConfigBlobName = settings["StorageConfigBlobName"];
            }

            var storage = JsonConvert.DeserializeObject<IBlobStorage>(_storageConfigJson, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });

            var settingRoot = await GetRootConfigurationAsync(storage, loadForType);

            var jSettingRoot = JObject.Parse(settingRoot);

            await ProcessConfigurationAsync(jSettingRoot, storage, loadForType); 

            return jSettingRoot;
        }

        private async Task ProcessConfigurationAsync(JObject jSetting, IBlobStorage storage, Type loadForType)
        {
            foreach (var handler in new ISettingsPropertyFactory[]
                {
                    new LoadSettingPropertyHandler(jSetting, storage),
                    new RefSettingPropertyHandler(jSetting),
                    new EncryptedSettingPropertyHandler(jSetting),
                    new FilterSettingPropertyHandler(jSetting, loadForType.FullName)
                })
                await handler.ExecuteAsync();
        }

        private async Task<string> GetRootConfigurationAsync(IBlobStorage storage, Type loadForType)
        {
            foreach (var blobName in 
                            GetBlobNames(loadForType)
                                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    )
                if (await storage.ExistsAsync(blobName))
                    return await storage.GetBlobAsStringAsync(blobName);

            throw new Exception($"No configuration file found ({string.Join(", ", GetBlobNames(loadForType))})");
        }

        private IEnumerable<string> GetBlobNames(Type loadForType)
        {
            yield return $"{loadForType.FullName}.json";
            yield return $"{new AssemblyName(loadForType.Assembly.FullName).Name}.json";
            yield return StorageConfigBlobName;
        }


        class BlobStorageDefaultConfig
        {
            [JsonProperty("$type")]
            public string Type { get; set; }
            public string ConnectionString { get; set; }
            public string RootPath { get; set; }
        }

    }
}
