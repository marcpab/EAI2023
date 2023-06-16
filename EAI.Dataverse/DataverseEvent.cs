using EAI.OData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAI.Dataverse
{
    public class DataverseEvent
    {
        public static T ToEntity<T>(string dataverseEvent, string propertyName = "PostEntityImages", string imageKey = "PostImage")
        {
            var jEvent = JObject.Parse(dataverseEvent);

            JObject jEntity = TransformEventToEntity(jEvent, propertyName, imageKey);

            return JsonConvert.DeserializeObject<T>(jEntity.ToString(), ODataClient.JsonSerializerSettings);
        }

        public static JObject TransformEventToEntity(JObject jEvent, string propertyName = "PostEntityImages", string imageKey = "PostImage")
        {
            var jAttributes = jEvent
                .Value<JArray>(propertyName)
                .Values<JObject>()
                .Where(t => t.Value<string>("key") == imageKey)
                .Select(t => t.Value<JObject>("value"))
                .Select(t => t.Value<JArray>("Attributes"))
                .Select(t => t.Values<JObject>())
                .FirstOrDefault()
                .Select(t => CreateEntityProperty(t));

            return new JObject(jAttributes);
        }

        private static JProperty CreateEntityProperty(JObject jEventPropertyObject)
        {
            var jValue = jEventPropertyObject.Value<JToken>("value");
            var name = jEventPropertyObject.Value<string>("key");

            var value = jValue; // .Value<object>() as JObject;

            switch(value)
            {
                case JObject jObject:

                    var valueType = jObject.Value<string>("__type");

                    var isEntityReference = valueType.StartsWith("EntityReference:");
                    if (isEntityReference)
                        return new JProperty($"_{name}_value", jObject.Value<JToken>("Id"));

                    var isOptionsetValue = valueType.StartsWith("OptionSetValue:");
                    if (isOptionsetValue)
                        return new JProperty(name, jObject.Value<JToken>("Value"));

                    var isMoney = valueType.StartsWith("Money:");
                    if (isMoney)
                        return new JProperty(name, jObject.Value<JToken>("Value"));

                    throw new NotImplementedException($"__type: {valueType}");

                case JArray jArray: // multiple option set

                    var values = jArray.Select(e => e.Value<string>("Value"));

                    return new JProperty(name, string.Join(",", values));

                default:
                    return new JProperty(name, jValue);
            }
        }
  
    }
}
