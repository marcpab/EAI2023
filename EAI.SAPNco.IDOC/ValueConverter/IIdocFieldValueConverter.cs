using System;

namespace EAI.SAPNco.IDOC.ValueConverter
{
    internal interface IIdocFieldValueConverter
    {
        Type ClrType { get; }

        object FromIDOC(string value);
        string ToIDOC(object value, int fieldLength);
    }
}