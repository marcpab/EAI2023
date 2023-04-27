using EAI.Dataverse.ModelGenerator.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EAI.Dataverse.ModelGenerator
{
    internal class MetadataTokens
    {
        private static XNamespace _nsEdmx = "http://docs.oasis-open.org/odata/ns/edmx";
        
        private XElement _xSchema;
        private XNamespace _nsSchema;
        private string _alias;

        private Dictionary<string, EntityToken> _complexTypesList;
        private ModelAssemblyView _view;
        private List<IToken> _tokenList;

        public async Task<IEnumerable<IToken>> GetMeatdataTokensAsync(string oDataV4MetadataPath, ModelAssemblyView view)
        {
            _view = view;

            _tokenList = new List<IToken>();
            _complexTypesList = new Dictionary<string, EntityToken>();

            var xMetadata = XElement.Load(oDataV4MetadataPath);

            _xSchema = xMetadata.Element(_nsEdmx + "DataServices").Elements().Where(e => e.Name.LocalName == "Schema").FirstOrDefault();
            _nsSchema = _xSchema.Name.Namespace;

            _alias = _xSchema.Attribute("Alias").Value;

            var xActions = _xSchema.Elements(_nsSchema + "Action");

            foreach (var cdsActionType in view.ActionTypes)
            {
                var xAction = xActions.Where(a => a.Attribute("Name")?.Value == cdsActionType.Name).FirstOrDefault();
                if (xAction == null)
                    continue;

                var requestEntity = new EntityToken()
                {
                    Namespace = cdsActionType.Namespace,
                    DisplayName = cdsActionType.Name,
                    Description = $"Action {cdsActionType.Name}",
                    Entity = cdsActionType.Name,
                };

                var requestEntityTokenList = new List<IToken>();
                requestEntity.ChildTokens = requestEntityTokenList;

                foreach (var xParameter in xAction.Elements(_nsSchema + "Parameter"))
                {
                    requestEntityTokenList.Add(new PropertyToken()
                    {
                        ODataName = xParameter.Attribute("Name").Value,
                        CSharpType = GetMetadataCSharpType(xParameter.Attribute("Type").Value, cdsActionType)
                    });
                }

                _tokenList.Add(requestEntity);

                var xReturnType = xAction.Elements(_nsSchema + "ReturnType").FirstOrDefault();
                if (xReturnType == null)
                    continue;

                var returnType = xReturnType.Attribute("Type").Value;

                requestEntity.Description += ", ReturnType: " + returnType;

                requestEntityTokenList.Add(new ExecuteToken()
                {
                    Entity = cdsActionType.Name,
                    CSharpType = GetMetadataCSharpType(returnType, cdsActionType),
                });
           }

            return _tokenList;
        }

        private string GetMetadataCSharpType(string type, Type cdsActionType)
        {
            if (type.StartsWith("Edm."))
                switch (type)
                {
                    case "Edm.String": // rdag_calculparam01,emailaddress3,emailaddress2,rdag_representativename,masteraccountidyominame
                        return "string";
                    case "Edm.DateTimeOffset": // modifiedon,opendeals_date,createdon,lastusedincampaign,rdag_priceshiftdate
                        return "DateTimeOffset";
                    //case "Money": // aging90,openrevenue,aging30,aging60_base,creditlimit
                    //    return "decimal?";
                    //case "Double": // address1_longitude,address2_latitude,address2_longitude,address1_latitude
                    //    return "double?";
                    case "Edm.Boolean": // donotpostalmail,creditonhold,donotbulkpostalmail,donotbulkemail,donotfax
                        return "bool?";
                    //case "Virtual": // address2_freighttermscodename,businesstypecodename,preferredappointmentdaycodename,accountcategorycodename,donotsendmarketingmaterialname
                    //    return "string";
                    //case "Picklist": // address1_addresstypecode,address1_shippingmethodcode,accountcategorycode,address1_freighttermscode,customersizecode
                    //    return "int?";
                    //case "BigInt": // entityimage_timestamp,versionnumber
                    //    return "long?";
                    case "Edm.Int32": // sharesoutstanding,onholdtime,address1_utcoffset,numberofemployees,opendeals_state
                        return "int?";
                    //case "State": // statecode
                    //    return "int?";
                    //case "Decimal": // exchangerate
                    //    return "decimal?";
                    //case "Memo": // description,address1_composite,address2_composite
                    //    return "string";
                    //case "Uniqueidentifier": // address1_addressid,entityimageid,stageid,address2_addressid,accountid
                    //    return "Guid?";
                    //case "EntityName": // owneridtype
                    //    return "string";
                    //case "Status": // statuscode
                    //    return "int?";
                    //case "Owner": // ownerid
                    //    return "Guid?";
                    default:
                        throw new Exception($"type {type} not implemented.");
                        return "string";
                }

            if (type.StartsWith(_alias))
            {
                var modelType = _view.EntityTypes.Where(t => _alias + "." + t.Name == type).FirstOrDefault();
                if (modelType != null)
                    return modelType.FullName;

                var complexTypeToken = CreateComplexType(type, cdsActionType);
                if (complexTypeToken != null)
                    return complexTypeToken.Namespace + "." + complexTypeToken.Entity;

                return "dynamic";

#warning complex type
            }

            throw new Exception($"type {type} not implemented.");

        }

        private EntityToken CreateComplexType(string type, Type cdsActionType)
        {
            var xComplexTypes = _xSchema.Elements(_nsSchema + "ComplexType");
            var xComplexType = xComplexTypes.Where(a => _alias + "." + a.Attribute("Name")?.Value == type).FirstOrDefault();

            if(xComplexType == null)
            {
                return null;
            }

            var complexTypeToken = new EntityToken()
            {
                DisplayName = type,
                Entity = xComplexType.Attribute("Name")?.Value,
                Namespace = cdsActionType.Namespace
            };

            EntityToken existingToken;
            if (_complexTypesList.TryGetValue(complexTypeToken.Namespace + "." + complexTypeToken.Entity, out existingToken))
                return existingToken;

            var complexTypeTokenList = new List<IToken>();

            foreach (var xParameter in xComplexType.Elements(_nsSchema + "Property"))
            {
                complexTypeTokenList.Add(new PropertyToken()
                {
                    ODataName = xParameter.Attribute("Name").Value,
                    CSharpType = GetMetadataCSharpType(xParameter.Attribute("Type").Value, cdsActionType)
                });
            }

            complexTypeToken.ChildTokens = complexTypeTokenList;

            _tokenList.Add(complexTypeToken);
            _complexTypesList.Add(complexTypeToken.Namespace + "." + complexTypeToken.Entity, complexTypeToken);

            return complexTypeToken;
        }
    }
}
