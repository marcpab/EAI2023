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
                .Select(t => new JProperty(t.Value<string>("key"), GetValue(t.Value<JToken>("value"))));

            return new JObject(jAttributes);
        }

        private static JToken GetValue(JToken jValue)
        {
            var jObject = jValue.Value<object>() as JObject;
            if (jObject == null)
                return jValue;

            if(jObject.Value<string>("__type").StartsWith("EntityReference:"))
                return jObject.Value<JToken>("Id");

            if (jObject.Value<string>("__type").StartsWith("OptionSetValue:"))
                return jObject.Value<JToken>("Value");

            return jValue;
        }    }
}
