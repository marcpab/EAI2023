namespace EAI.SAPNco.IDOC.ValueConverter
{
    internal interface IIdocFieldValueConverter
    {
        object FromIDOC(string value);
        string ToIDOC(object value, int fieldLength);
    }
}