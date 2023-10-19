using EAI.General.Cache;
using EAI.General.SettingFile;
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
    public class ConfigurationLoader
    {
        private IBlobStorage _storage;
        private Type _loadForType;
        private string _storageConfigBlobName;

        public ConfigurationLoader(IBlobStorage storage, Type loadForType, string storageConfigBlobName)
        {
            _storage = storage;
            _loadForType = loadForType;
            _storageConfigBlobName = storageConfigBlobName;
        }

        public async Task<JObject> CreateConfigurationAsync()
        {
            var settingRoot = await GetRootConfigurationAsync();

            var jSettingRoot = JObject.Parse(settingRoot);

            await ProcessConfigurationAsync(jSettingRoot); 

            return jSettingRoot;
        }

        private async Task ProcessConfigurationAsync(JObject jSetting)
        {
            foreach (var handler in new ISettingsPropertyFactory[]
                {
                    new LoadSettingPropertyHandler(jSetting, GetBlobAsync),
                    new RefSettingPropertyHandler(jSetting),
                    new EncryptedSettingPropertyHandler(jSetting),
                    new FilterSettingPropertyHandler(jSetting, _loadForType.FullName)
                })
                await handler.ExecuteAsync();
        }

        private async Task<string> GetRootConfigurationAsync()
        {
            foreach (var blobName in 
                            GetBlobNames(_loadForType)
                                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    )
                if (await _storage.ExistsAsync(blobName))
                    return await GetBlobAsync(blobName);

            throw new Exception($"No configuration file found ({string.Join(", ", GetBlobNames(_loadForType))})");
        }

        private IEnumerable<string> GetBlobNames(Type loadForType)
        {
            yield return $"{loadForType.FullName}.json";
            yield return $"{new AssemblyName(loadForType.Assembly.FullName).Name}.json";
            yield return _storageConfigBlobName;
        }

        private async Task<string> GetBlobAsync(string blobName)
        {
            var changed = false;
            var content = await _storage.GetBlobAsStringAsync(blobName);

            var jContent = JObject.Parse(content);


            foreach (var settingFile in new ISettingFile[] {
                    new EncryptSettingFile(jContent),
                    new SHA256SettingFile(jContent)
                })
            {
                await settingFile.ExecuteAsync();

                changed = changed || settingFile.Changed;
            }

            if (changed)
                await _storage.SaveBlobAsync(blobName, jContent.ToString());

            return content;
        }

    }
}
