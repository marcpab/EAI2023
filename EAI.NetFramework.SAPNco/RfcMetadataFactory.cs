using EAI.SAPNco.Model;
using NCo = SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.NetFramework.SAPNco
{
    internal class RfcMetadataFactory
    {
        private static readonly RfcMetadataFactory _instance = new RfcMetadataFactory();

        internal static RfcMetadataFactory Instance => _instance;


        public RfcFunctionMetadata CreateRfcFunctionMetadata(NCo.RfcFunctionMetadata sapFunction)
            => new RfcFunctionMetadata()
            {
                _name = sapFunction.Name,
                _parameters = EnumElements(sapFunction, sapFunction.ParameterCount)
                    .Select(p => GetParameter(p))
                    .ToArray()
            };

        private RfcParameterMetadata GetParameter(NCo.RfcParameterMetadata sapParameter)
        {
            var rfcParameterMetadata = new RfcParameterMetadata()
            {
                _abapDataType = GetAbapType(sapParameter.DataType),
                _decimals = sapParameter.Decimals,
                _name = sapParameter.Name,
                _defaultValue = sapParameter.DefaultValue,
                _direction = (RfcParameterDirection)sapParameter.Direction,
                _documentation = sapParameter.Documentation,
                _nucLength = sapParameter.NucLength,
                _ucLength = sapParameter.UcLength,
                _optional = sapParameter.Optional,
                _rfcDataType = (RfcDataType)sapParameter.DataType
            };


            switch (sapParameter.DataType)
            {
                case NCo.RfcDataType.TABLE:

                    rfcParameterMetadata._tableMetadata = GetTableMetadata(sapParameter.ValueMetadataAsTableMetadata);
                    break;

                case NCo.RfcDataType.STRUCTURE:

                    rfcParameterMetadata._structureMetadata = GetStructureMetadata(sapParameter.ValueMetadataAsStructureMetadata);
                    break;

                default:
                    break;
            }

            return rfcParameterMetadata;
        }


        public RfcTableMetadata GetTableMetadata(NCo.RfcTableMetadata sapTable)
            => new RfcTableMetadata()
            {
                _name = sapTable.Name,
                _lineType = GetStructureMetadata(sapTable.LineType),
            };

        private RfcStructureMetadata GetStructureMetadata(NCo.RfcStructureMetadata sapStructure)
            => new RfcStructureMetadata()
                {
                    _name = sapStructure.Name,
                    _fields = EnumElements(sapStructure, sapStructure.FieldCount)
                                .Select(f => GetFieldMetadata(f))
                                .ToArray()
                };

        private RfcFieldMetadata GetFieldMetadata(NCo.RfcFieldMetadata sapField)
        {
            var rfcFieldMetadata = new RfcFieldMetadata()
            {
                _abapDataType = GetAbapType(sapField.DataType),
                _decimals = sapField.Decimals,
                _name = sapField.Name,
                _documentation = sapField.Documentation,
                _nucLength = sapField.NucLength,
                _ucLength = sapField.UcLength,
                _rfcDataType = (RfcDataType)sapField.DataType
            };

            switch (sapField.DataType)
            {
                case NCo.RfcDataType.TABLE:

                    rfcFieldMetadata._tableMetadata = GetTableMetadata(sapField.ValueMetadataAsTableMetadata);
                    break;

                case NCo.RfcDataType.STRUCTURE:

                    rfcFieldMetadata._structureMetadata = GetStructureMetadata(sapField.ValueMetadataAsStructureMetadata);
                    break;

                default:
                    break;
            }

            return rfcFieldMetadata;
        }

        private IEnumerable<T> EnumElements<T>(NCo.RfcContainerMetadata<T> sapContainer, int count)
            where T : NCo.RfcElementMetadata
        {
            for (int i = 0; i < count; i++)
                yield return sapContainer[i];
        }


        public static string GetAbapType(NCo.RfcDataType sapDataType)
        {
            switch (sapDataType)
            {
                case NCo.RfcDataType.CHAR: return " C";
                case NCo.RfcDataType.BYTE: return " X";
                case NCo.RfcDataType.NUM: return " N";
                case NCo.RfcDataType.BCD: return " P";
                case NCo.RfcDataType.DATE: return " D";
                case NCo.RfcDataType.TIME: return " T";
                case NCo.RfcDataType.UTCLONG: return " p";
                case NCo.RfcDataType.UTCSECOND: return " n";
                case NCo.RfcDataType.UTCMINUTE: return " w";
                case NCo.RfcDataType.DTDAY: return " d";
                case NCo.RfcDataType.DTWEEK: return "7";
                case NCo.RfcDataType.DTMONTH: return " x";
                case NCo.RfcDataType.TSECOND: return " t";
                case NCo.RfcDataType.TMINUTE: return " i";
                case NCo.RfcDataType.CDAY: return " c";
                case NCo.RfcDataType.FLOAT: return " F";
                case NCo.RfcDataType.INT1: return " b";
                case NCo.RfcDataType.INT2: return " s";
                case NCo.RfcDataType.INT4: return " I";
                case NCo.RfcDataType.INT8: return "8";
                case NCo.RfcDataType.DECF16: return " a";
                case NCo.RfcDataType.DECF34: return " e";
                case NCo.RfcDataType.STRING: return " g";
                case NCo.RfcDataType.XSTRING: return " y";
                case NCo.RfcDataType.STRUCTURE: return " u or v";
                case NCo.RfcDataType.TABLE: return " h";
                case NCo.RfcDataType.CLASS: return " * or +";
                default:
                    return "unknown";
            }
        }

    }
}
