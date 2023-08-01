using EAI.General.Extensions;

namespace EAI.SAPNco.IDOC.ValueConverter
{
    internal class FieldValueConverterString : IIdocFieldValueConverter
    {
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
