using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.OData
{
    internal class DateTimeConverterTimeZoneIndependent : IsoDateTimeConverter
    {
        public DateTimeConverterTimeZoneIndependent() 
        {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
        }
    }
}
