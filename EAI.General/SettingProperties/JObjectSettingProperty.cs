using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace EAI.General.SettingProperties
{
    public class JObjectSettingProperty
    {
        public string Value { get; set; }
        public JObject Parent { get; set; }
        public JProperty Property { get; set; }

        public static List<JObjectSettingProperty> GetProperties(string identifier, JObject setting)
        {
            return setting
                .Descendants()
                .Where(token => token.Type == JTokenType.Property)
                .Select(token => (JProperty)token)
                .Where(property => property.Name == identifier)
                .Select(settingProperty => new JObjectSettingProperty
                {
                    Value = settingProperty.Value.Value<string>(),
                    Property = settingProperty,
                    Parent = (JObject)settingProperty.Parent,
                })
                .ToList();
        }

        public static IEnumerable<string> GetValueEnumerable(JToken token)
        {

            switch (token.Type)
            {
                case JTokenType.String:
                    {
                        yield return token.Value<string>();
                        break;
                    }

                case JTokenType.Array:
                    {
                        foreach (var entry in token.Values<string>())
                        {
                            yield return entry;
                        }

                        break;
                    }
            }
        }

    }


}