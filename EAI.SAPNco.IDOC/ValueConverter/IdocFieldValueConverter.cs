using EAI.General.Extensions;
using EAI.SAPNco.IDOC.Metadata;
using System;
using System.Collections.Generic;

namespace EAI.SAPNco.IDOC.ValueConverter
{
    internal class IdocFieldValueConverter
    {
        private static IdocFieldValueConverter _instance = new IdocFieldValueConverter();

        public static IdocFieldValueConverter Instance { get { return _instance; } }


        private Dictionary<DataTypeEnum, IIdocFieldValueConverter> _converter;

        private IdocFieldValueConverter()
        {
            _converter = new Dictionary<DataTypeEnum, IIdocFieldValueConverter>() {
                { DataTypeEnum.ACCP, new FieldValueConverterDateTime("yyyyMM") },
                { DataTypeEnum.CHAR, new FieldValueConverterString() },
                { DataTypeEnum.CLNT, new FieldValueConverterString() },
                { DataTypeEnum.CUKY, new FieldValueConverterString() },
                { DataTypeEnum.CURR, new FieldValueConverterNumber<decimal>("", true) },
                { DataTypeEnum.D16D, new FieldValueConverterNumber<decimal>("", true) },
                { DataTypeEnum.D16R, new FieldValueConverterNumber<decimal>("", true) },
                { DataTypeEnum.D16S, new FieldValueConverterNumber<decimal>("", true) },
                { DataTypeEnum.D34D, new FieldValueConverterNumber<decimal>("", true) },
                { DataTypeEnum.D34R, new FieldValueConverterNumber<decimal>("", true) },
                { DataTypeEnum.D34S, new FieldValueConverterNumber<decimal>("", true) },
                { DataTypeEnum.DATS, new FieldValueConverterDateTime("yyyyMMdd") },
                { DataTypeEnum.DEC , new FieldValueConverterNumber<decimal>("", true) },
                { DataTypeEnum.FLTP, new FieldValueConverterNumber<float>("", true) },
                { DataTypeEnum.INT1, new FieldValueConverterNumber<byte>("", false) },
                { DataTypeEnum.INT2, new FieldValueConverterNumber<ushort>("", false) },
                { DataTypeEnum.INT4, new FieldValueConverterNumber<int>("", false) },
                { DataTypeEnum.LANG, new FieldValueConverterString() },
                { DataTypeEnum.LCHR, new FieldValueConverterString() },
                { DataTypeEnum.LRAW, new FieldValueConverterString() },
                { DataTypeEnum.NUMC, new FieldValueConverterNumber<decimal>("", false) },
                { DataTypeEnum.PREC, new FieldValueConverterString() },
                { DataTypeEnum.QUAN, new FieldValueConverterNumber<decimal>("", true) },
                { DataTypeEnum.RAW , new FieldValueConverterString() },
                { DataTypeEnum.RSTR, new FieldValueConverterString() },
                { DataTypeEnum.SSTR, new FieldValueConverterString() },
                { DataTypeEnum.STRG, new FieldValueConverterString() },
                { DataTypeEnum.TIMS, new FieldValueConverterDateTime("HHmmss") },
                { DataTypeEnum.UNIT, new FieldValueConverterString() },
                { DataTypeEnum.VARC, new FieldValueConverterString() },
                { DataTypeEnum.Unknown, new FieldValueConverterString() },
            };
        }

        public Type ClrType(DataTypeEnum dataType)
        {
            return _converter[dataType].ClrType;
        }

        public string ToIDOC(DataTypeEnum dataType, object value, int fieldLength)
        {
            return _converter[dataType].ToIDOC(value, fieldLength);
        }

        public object FromIDOC(DataTypeEnum dataType, string value)
        {
            return _converter[dataType].FromIDOC(value);
        }
    }
}
