using EAI.General.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EAI.General.SettingProperties
{
    /// <summary>
    /// Handler to process %filter properties from the setting object
    /// </summary>
    public class FilterSettingPropertyHandler : ISettingsPropertyFactory
    {
        private readonly JObject _setting;
        private readonly string _identifier;

        public FilterSettingPropertyHandler(JObject setting, string identifier)
        {
            _setting = setting;
            _identifier = identifier;
        }
        public Task ExecuteAsync()
        {

            var properties = JObjectSettingProperty.GetProperties($"$filter", _setting);

            foreach (var property in properties)
            {
                try
                {
                    if (JObjectSettingProperty
                        .GetValueEnumerable(property.Property.Value)
                        .Any(value => Regex.IsMatch(_identifier, value) == true))
                    {
                        continue;
                    }

                    property.Parent.Remove();
                }
                catch (Exception ex)
                {
                    throw new AzureException($"Error handling $filter property {property.Property}", ex);
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