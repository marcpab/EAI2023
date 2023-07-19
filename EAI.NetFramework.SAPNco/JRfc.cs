using Newtonsoft.Json.Linq;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EAI.NetFramework.SAPNco
{
    public class JRfc
    {
        public static IRfcFunction JsonToRfcFunction(JProperty jFunction, RfcDestination rfcDestination)
        {
            var e = rfcDestination.Repository.CheckFunctionExists(jFunction.Name);

            var rfcFunction = rfcDestination.Repository.CreateFunction(jFunction.Name);

            var jFunctionData = jFunction.Value as JObject;
            if (jFunctionData != null)
                JsonToRfcData(jFunctionData, rfcFunction, rfcFunction.SetParameterActive);

            return rfcFunction;
        }

        public static void JsonToRfcData(JObject jData, IRfcDataContainer rfcData, Action<string, bool> setParameterActive)
        {
            foreach(var jParam in jData.Properties())
            {
                var elementMetadata = rfcData.GetElementMetadata(jParam.Name);
                setParameterActive?.Invoke(jParam.Name, true);

                switch (elementMetadata.DataType)
                {
                    case RfcDataType.TABLE:
                        var jTable = jParam.Value as JArray;
                        if (jTable == null)
                            break;

                        var rfcTable = rfcData.GetTable(jParam.Name);
                        if (rfcTable == null)
                            break;

                        foreach (var jArrayElement in jTable)
                        {
                            var jRecord = jArrayElement as JObject;
                            if (jRecord == null)
                                break;

                            rfcTable.Append();
                            rfcTable.CurrentIndex = rfcTable.Count - 1;

                            JsonToRfcData(jRecord, rfcTable.CurrentRow, null);
                        }

                        break;

                    case RfcDataType.STRUCTURE:
                        var jStruct = jParam.Value as JObject;
                        if (jStruct == null)
                            break;

                        var rfcStruct = rfcData.GetStructure(jParam.Name);
                        if (rfcStruct == null)
                            break;

                        JsonToRfcData(jStruct, rfcStruct, null);
                        break;

                    case RfcDataType.BCD:
                        {
                            var stringValue = (jParam.Value as JValue)?.Value?.ToString();

                            decimal decValue = 0;
                            if (!string.IsNullOrEmpty(stringValue))
                                decValue = decimal.Parse(stringValue, CultureInfo.InvariantCulture);

                            rfcData.SetValue(jParam.Name, decValue);

                            break;
                        }
                    default:

                        rfcData.SetValue(jParam.Name, (jParam.Value as JValue)?.Value?.ToString());
                        break;

                }
            }
        }

        public static void RfcDataToJson(IRfcDataContainer rfcData, JObject jData)
        {
            for(var elementIndex = 0; elementIndex < rfcData.ElementCount; elementIndex++)
            {
                var elementMetadata = rfcData.GetElementMetadata(elementIndex);

                switch(elementMetadata.DataType) 
                {
                    case RfcDataType.TABLE:

                        var rfcTable = rfcData.GetTable(elementIndex);
                        if (rfcTable == null)
                            break;

                        var jTable = new JArray();

                        for(var rowIndex = 0; rowIndex < rfcTable.Count; rowIndex++)
                        {
                            var jRecord = new JObject();

                            rfcTable.CurrentIndex = rowIndex;
                            RfcDataToJson(rfcTable.CurrentRow, jRecord);

                            jTable.Add(jRecord);
                        }

                        jData.Add(new JProperty(elementMetadata.Name, jTable));

                        break;

                    case RfcDataType.STRUCTURE:

                        var rfcStruct = rfcData.GetStructure(elementIndex);
                        if (rfcStruct == null)
                            break;

                        var jStruct = new JObject();

                        RfcDataToJson(rfcStruct, jStruct);

                        jData.Add(new JProperty(elementMetadata.Name, jStruct));

                        break;
                    
                    default:

                        var rfcValue = rfcData.GetValue(elementIndex);
                        if(rfcValue == null)
                            break;

                        var jParam = new JValue(rfcValue);

                        jData.Add(new JProperty(elementMetadata.Name, jParam));

                        break;
                }
            }
        }

        public static void RfcMetadataToJsonSchema(IRfcDataContainer rfcData, JObject jData, RfcDirection direction)
        {
            var directions = new[] { RfcDirection.TABLES, RfcDirection.CHANGING, direction };

            for (var elementIndex = 0; elementIndex < rfcData.ElementCount; elementIndex++)
            {
                var elementMetadata = rfcData.GetElementMetadata(elementIndex);

                var paramMetadata = elementMetadata as RfcParameterMetadata;

                if (paramMetadata != null&& !directions.Contains(paramMetadata.Direction))
                    continue;

                switch (elementMetadata.DataType)
                {
                    case RfcDataType.TABLE:

                        var rfcTable = rfcData.GetTable(elementIndex);
                        if (rfcTable == null)
                            break;

                        rfcTable.Append();
                        rfcTable.Append();
                        rfcTable.Append();

                        var jTable = new JArray();

                        for (var rowIndex = 0; rowIndex < rfcTable.Count; rowIndex++)
                        {
                            var jRecord = new JObject();

                            rfcTable.CurrentIndex = rowIndex;
                            RfcMetadataToJsonSchema(rfcTable.CurrentRow, jRecord, direction);

                            jTable.Add(jRecord);
                        }

                        jData.Add(new JProperty(elementMetadata.Name, jTable));

                        break;

                    case RfcDataType.STRUCTURE:

                        var rfcStruct = rfcData.GetStructure(elementIndex);
                        if (rfcStruct == null)
                            break;

                        var jStruct = new JObject();

                        RfcMetadataToJsonSchema(rfcStruct, jStruct, direction);

                        jData.Add(new JProperty(elementMetadata.Name, jStruct));

                        break;

                    default:

                        var rfcValue = $"rfc type: {elementMetadata.DataType}, abap type: {GetAbapType(elementMetadata.DataType)}, decimals: {elementMetadata.Decimals}, nuc len: {elementMetadata.NucLength}";

                        var jParam = new JValue(rfcValue);

                        jData.Add(new JProperty(elementMetadata.Name, jParam));

                        break;
                }
            }
        }

        public static string GetAbapType(RfcDataType dataType)
        {
            switch(dataType)
            {
                case RfcDataType.CHAR: return " C";
                case RfcDataType.BYTE: return " X";
                case RfcDataType.NUM: return " N";
                case RfcDataType.BCD: return " P";
                case RfcDataType.DATE: return " D";
                case RfcDataType.TIME: return " T";
                case RfcDataType.UTCLONG: return " p";
                case RfcDataType.UTCSECOND: return " n";
                case RfcDataType.UTCMINUTE: return " w";
                case RfcDataType.DTDAY: return " d";
                case RfcDataType.DTWEEK: return "7";
                case RfcDataType.DTMONTH: return " x";
                case RfcDataType.TSECOND: return " t";
                case RfcDataType.TMINUTE: return " i";
                case RfcDataType.CDAY: return " c";
                case RfcDataType.FLOAT: return " F";
                case RfcDataType.INT1: return " b";
                case RfcDataType.INT2: return " s";
                case RfcDataType.INT4: return " I";
                case RfcDataType.INT8: return "8";
                case RfcDataType.DECF16: return " a";
                case RfcDataType.DECF34: return " e";
                case RfcDataType.STRING: return " g";
                case RfcDataType.XSTRING: return " y";
                case RfcDataType.STRUCTURE: return " u or v";
                case RfcDataType.TABLE: return " h";
                case RfcDataType.CLASS: return " * or +";
                default:
                    return "unknown";
            }
        }

    }
}
