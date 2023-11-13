using Newtonsoft.Json;
using System;
using System.Globalization;

namespace EAI.OnPrem.SAPNcoService
{
    internal class DateConverter : JsonConverter
    {
        public DateConverter()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            if(objectType == typeof(DateTimeOffset?))
                return true;

            return false;
        }

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var readPropertyName = reader.Read();
            if (!readPropertyName)
                return null;

            var readPropertyValue = reader.Read();
            if (!readPropertyValue)
                return null;

            var value = reader.Value as string;

            if (string.IsNullOrWhiteSpace(value) || value == "0000-00-00")
                return null;

            if(!DateTimeOffset.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTimeOffset dateTimeValue))
                return null;

            return dateTimeValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
