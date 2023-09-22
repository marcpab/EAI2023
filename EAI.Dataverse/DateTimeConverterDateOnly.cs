using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.OData
{
    public class DateTimeConverterDateOnly : IsoDateTimeConverter
    {
        public DateTimeConverterDateOnly()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
