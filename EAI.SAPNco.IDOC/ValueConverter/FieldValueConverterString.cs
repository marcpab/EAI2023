using EAI.General.Extensions;
using System;

namespace EAI.SAPNco.IDOC.ValueConverter
{
    internal class FieldValueConverterString : IIdocFieldValueConverter
    {

        public Type ClrType { get => typeof(string); }

        public object FromIDOC(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            return stringValue.TrimEnd();
        }

        public string ToIDOC(object value, int length)
        {
            var stringValue = value?.ToString();

            if (stringValue == null)
                return new string(' ', length);

            return stringValue.Size(length);
        }
    }
}
