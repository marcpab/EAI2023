using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.OData
{
    public class DateTimeConverterUserLocal : IsoDateTimeConverter
    {
        public DateTimeConverterUserLocal()
        {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:sszzz";
        }
    }
}
