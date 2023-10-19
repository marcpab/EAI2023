using EAI.General.Cache;
using EAI.General.SettingProperties;
using EAI.General.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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

        private Task<JObject> CreateConfigurationAsync(Type loadForType)
        {
            if (string.IsNullOrEmpty(_storageConfigJson))
            {
                var settings = SettingsHandler.GetConfigurationRoot();

                _storageConfigJson = JsonConvert.SerializeObject(
                    new BlobStorageDefaultConfig
                    {
                        Type = settings["ConfigStorageType"] ?? "EAI.AzureStorage.BlobStorage, EAI.AzureStorage",
                        ConnectionString = settings["ConfigurationStorage"] ?? settings["AzureWebJobsStorage"],
                        RootPath = settings["ConfigRootPath"] ?? "config"
                    });

                _storageConfigBlobName = settings["StorageConfigBlobName"] ?? "config.json";
            }

            var storage = JsonConvert.DeserializeObject<IBlobStorage>(_storageConfigJson, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });

            var loader = new ConfigurationLoader(storage, loadForType, _storageConfigBlobName);

            return loader.CreateConfigurationAsync();
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
