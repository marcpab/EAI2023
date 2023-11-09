using EAI.Dataverse.ModelGenerator;
using EAI.ModelGenerator;
using EAI.OnPrem.SAPNcoService;
using EAI.OnPrem.Storage;
using EAI.SAPNco.Model;
using EAI.SAPNco.ModelGenerator.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EAI.SAPNco.ModelGenerator
{
    public class RfcTokens
    {
        private Dictionary<string, RfcStructureToken> _rfcStructureTokens = new Dictionary<string, RfcStructureToken>();

        public string Namespace { get; set; }

        public async Task<IEnumerable<IToken>> GetRfcFunctionTokensAsync(Assembly modelAssembly, OnPremClient onPremClient, string sapSystemName, IEnumerable<string> rfcFunctionNames)
        {
            var view = new ModelAssemblyView() { ModelAssembly = modelAssembly };

            Namespace = view.Types.FirstOrDefault()?.Namespace ?? "EAI.SAPNco.Model";

            _rfcStructureTokens.Clear();

            var rfcClient = new RfcGatewayServiceProxy();
            rfcClient.OnPremClient = onPremClient;

            var tokens = new List<IToken>();

            foreach(var rfcFunctionName in rfcFunctionNames)
            {
                var rfcFunctionMetadata = await rfcClient.GetRfcFunctionMetadataAsync(sapSystemName, rfcFunctionName);

                var rfcFunctionToken = GenerateFunctionToken(rfcFunctionMetadata, rfcFunctionMetadata._name, RfcParameterDirection.IMPORT);
                var rfcFunctionResponseToken = GenerateFunctionToken(rfcFunctionMetadata, $"{rfcFunctionMetadata._name}_Response", RfcParameterDirection.EXPORT);

                var rfcFunctionMessageToken = new RfcFunctionToken()
                {
                    RfcFunctionName = $"{rfcFunctionMetadata._name}_Message",
                    Description = rfcFunctionMetadata._description,
                    Namespace = Namespace,
                    ChildTokens = new[] {
                            new PropertyToken()
                            {
                                PropertyName = rfcFunctionToken.RfcFunctionName,
                                Description = rfcFunctionMetadata._description,
                                CSharpType = rfcFunctionToken.RfcFunctionName,
                            },
                            new PropertyToken()
                            {
                                PropertyName = rfcFunctionResponseToken.RfcFunctionName,
                                Description = rfcFunctionMetadata._description,
                                CSharpType = rfcFunctionResponseToken.RfcFunctionName,
                            }
                    }
                };

                tokens.Add(rfcFunctionToken);
                tokens.Add(rfcFunctionResponseToken);
                tokens.Add(rfcFunctionMessageToken);

            }

            tokens.AddRange(_rfcStructureTokens.Values);

            return tokens;
        }

        private RfcFunctionToken GenerateFunctionToken(RfcFunctionMetadata rfcFunctionMetadata, string name, RfcParameterDirection direction)
        {
            var directions = new[] { RfcParameterDirection.CHANGING, RfcParameterDirection.TABLES, direction };

            return new RfcFunctionToken()
            {
                RfcFunctionName = name,
                Description = rfcFunctionMetadata._description,
                Namespace = Namespace,
                ChildTokens = rfcFunctionMetadata
                ._parameters
                .Where(p => directions.Contains(p._direction))
                .Select(p => new RfcParameterToken()
                {
                    ParameterName = p._name,
                    Documentation = p._documentation,
                    Direction = p._direction.ToString(),
                    DataType = p._rfcDataType.ToString(),
                    AbapType = p._abapDataType,
                    NuLength = p._nucLength,
                    Decimals = p._decimals,
                    Optional = p._optional,
                    DefaultValue = p._defaultValue,
                    CSharpType = GetCSharpType(p._rfcDataType, p._structureMetadata, p._tableMetadata),
                })
                .ToArray()
            };
        }

        private string GetCSharpType(RfcDataType rfcDataType, RfcStructureMetadata structureMetadata, RfcTableMetadata tableMetadata)
        {
            switch(rfcDataType)
            {
                case RfcDataType.BCD:
                    return $"{nameof(Decimal)}?";
                case RfcDataType.CHAR:
                    return $"{nameof(String)}?";
                case RfcDataType.STRING:
                    return $"{nameof(String)}?";
                case RfcDataType.DATE:
                    return $"{nameof(DateTimeOffset)}?";
                case RfcDataType.TIME:
                    return $"{nameof(DateTimeOffset)}?";
                case RfcDataType.NUM:
                    return $"{nameof(Decimal)}?";
                case RfcDataType.INT8:
                    return $"{nameof(Int64)}?";
                case RfcDataType.INT4:
                    return $"{nameof(Int32)}?";
                case RfcDataType.INT2:
                    return $"{nameof(Int16)}?";
                case RfcDataType.INT1:
                    return $"{nameof(SByte)}?";
                case RfcDataType.DECF16:
                    return $"{nameof(Single)}?";
                case RfcDataType.DECF34:
                    return $"{nameof(Double)}?";
                case RfcDataType.FLOAT:
                    return $"{nameof(Double)}?";
                case RfcDataType.BYTE:
                    return $"{nameof(Byte)}[]?";
                case RfcDataType.STRUCTURE:
                    {
                        var structureToken = GenerateRfcStructure(structureMetadata);

                        return Utils.Codeify(structureToken.StructureName);
                    }
                case RfcDataType.TABLE:
                    {
                        var structureToken = GenerateRfcStructure(tableMetadata._lineType);

                        return $"List<{Utils.Codeify(structureToken.StructureName)}>";
                    }
                default:
                    throw new NotImplementedException($"RfcDataType {rfcDataType} not implemented yet.");
            }
        }

        private RfcStructureToken GenerateRfcStructure(RfcStructureMetadata structureMetadata)
        {
            var structureName = structureMetadata._name;

            if (!_rfcStructureTokens.TryGetValue(structureName, out var rfcStructureToken))
            {
                rfcStructureToken = new RfcStructureToken()
                {
                    Namespace = Namespace,
                    StructureName = structureName,
                };

                _rfcStructureTokens.Add(structureName, rfcStructureToken);

                rfcStructureToken.ChildTokens = structureMetadata
                        ._fields
                        .Select(f => new RfcFieldToken()
                        {
                            FieldName = f._name,
                            Documentation = f._documentation,
                            DataType = f._rfcDataType.ToString(),
                            AbapType = f._abapDataType,
                            NuLength = f._ucLength,
                            Decimals = f._decimals,
                            CSharpType = GetCSharpType(f._rfcDataType, f._structureMetadata, f._tableMetadata),
                        })
                        .ToArray();
            }

            return rfcStructureToken;
        }
    }
}
