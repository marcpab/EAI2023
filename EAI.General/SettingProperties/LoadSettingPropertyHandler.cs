using EAI.General.Exceptions;
using EAI.General.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EAI.General.SettingProperties
{
    /// <summary>
    /// Handler to process $encrypted properties from the setting object
    /// </summary>
    public class LoadSettingPropertyHandler : ISettingsPropertyFactory
    {
        private readonly JObject _setting;
        private readonly IBlobStorage _storage;


        public LoadSettingPropertyHandler(JObject setting, IBlobStorage storage)
        {
            _setting = setting;
            _storage = storage;
        }


        public Task ExecuteAsync()
        {
            return ExecuteAsync(_setting);
        }

        private async Task ExecuteAsync(JObject setting)
        {
            var properties = JObjectSettingProperty.GetProperties($"load", setting);

            foreach (var property in properties)
                try
                {
                    var loadedSetting = await _storage.GetBlobAsStringAsync(property.Value);

                    var jLoadedSetting = JObject.Parse(loadedSetting);

                    await ExecuteAsync(jLoadedSetting);

                    foreach(var p in jLoadedSetting.Properties())
                        property.Property.AddBeforeSelf(p);

                }
                catch (Exception ex)
                {
                    throw new EAIException($"Error loading {property.Value}", ex);
                }
                finally
                {
                    property.Property.Remove();
                }

        }


    }
}