using EAI.General.Extensions;
using System;
using System.Globalization;

namespace EAI.SAPNco.IDOC.ValueConverter
{
    internal class FieldValueConverterNumber<T> : IIdocFieldValueConverter
        where T : struct
    {
        private string _format;
        private bool _decimalPlaces;

        public FieldValueConverterNumber(string format, bool decimalPlaces)
        {
            _format = format;
            _decimalPlaces = decimalPlaces;
        }

        public Type ClrType { get => typeof(T); }

        public object FromIDOC(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            if (decimal.TryParse(stringValue, _decimalPlaces ? NumberStyles.Any : NumberStyles.Integer, CultureInfo.InvariantCulture, out var tryDecimalValue))
                return (T)Convert.ChangeType(tryDecimalValue, typeof(T));

            return null;
        }

        public string ToIDOC(object value, int length)
        {
            if (value == null)
                return new string(' ', length);

            var decimalValue = value as decimal?;
            if (decimalValue != null)
                return decimalValue.Value.ToString(_format, CultureInfo.InvariantCulture).Size(length);

            var stringValue = value.ToString();
            if (decimal.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var tryDecimalValue))
                return tryDecimalValue.ToString(_format, CultureInfo.InvariantCulture).Size(length);

            return stringValue.ToString();
        }
    }
}
