using EAI.General.Extensions;
using System;
using System.Globalization;

namespace EAI.SAPNco.IDOC.ValueConverter
{
    internal class FieldValueConverterDateTime : IIdocFieldValueConverter
    {
        private string _format;

        public FieldValueConverterDateTime(string format)
        {
            _format = format;
        }

        public object FromIDOC(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            if (DateTimeOffset.TryParseExact(stringValue, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeOffsetValue))
                return dateTimeOffsetValue;

            return null;
        }

        public string ToIDOC(object value, int length)
        {
            if (value == null)
                return new string(' ', length);

            var dateTimeOffsetValue = value as DateTimeOffset?;
            if (dateTimeOffsetValue != null)
                return dateTimeOffsetValue.Value.ToString(_format).Size(length);

            var dateTimeValue = value as DateTime?;
            if (dateTimeValue != null)
                return dateTimeValue.Value.ToString(_format).Size(length);

            var stringValue = value?.ToString();

            if (stringValue == null)
                return new string(' ', length);

            return stringValue.Size(length);
        }
    }
}
