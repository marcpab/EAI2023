using EAI.General.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace EAI.General.SettingProperties
{
    /// <summary>
    /// Handler to process $ref properties from the setting object
    /// </summary>
    public class RefSettingPropertyHandler : ISettingsPropertyFactory
    {
        private readonly JObject _setting;

        public RefSettingPropertyHandler(JObject setting)
        {
            _setting = setting;
        }

        public Task ExecuteAsync()
        {

            var properties = JObjectSettingProperty.GetProperties("$ref", _setting);

            foreach (var property in properties)
            {
                try
                {

                    if (string.IsNullOrEmpty(property.Value))
                    {
                        continue;
                    }

                    var referencePath = property.Value;

                    if (referencePath.StartsWith("/"))
                    {
                        referencePath = referencePath.Remove(0, 1);
                    }

                    referencePath = referencePath.Replace('/', '.');

                    var token = _setting.SelectToken(property.Value);

                    if (token == null)
                    {
                        throw new AzureException($"Reference property '{property.Value}'. Path '{property.Property.Path}' not found");
                    }

                    if (token.Type == JTokenType.Object)
                    {
                        property.Parent.Merge(token);
                    }
                    else
                    {
                        property.Parent.Replace(token);
                    }
                }
                catch (Exception ex)
                {
                    throw new AzureException($"Error handling $ref property {property.Parent}", ex);
                }
                finally
                {
                    property.Property.Remove();
                }
            }

            return Task.CompletedTask;
        }
    }
}