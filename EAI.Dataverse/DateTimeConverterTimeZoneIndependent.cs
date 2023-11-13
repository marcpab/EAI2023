using Newtonsoft.Json.Converters;

namespace EAI.OData
{
    public class DateTimeConverterTimeZoneIndependent : IsoDateTimeConverter
    {
        public DateTimeConverterTimeZoneIndependent()
        {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
        }
    }
}
