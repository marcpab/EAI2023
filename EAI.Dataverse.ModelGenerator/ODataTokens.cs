
using EAI.Dataverse.ModelGenerator.Model;
using EAI.Dataverse.ModelGenerator.Tokens;
using EAI.ModelGenerator;
using EAI.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Dataverse.ModelGenerator
{
    internal class ODataTokens
    {
        private static AttributeTypeCode[] _lookupTypeNames = new[] { AttributeTypeCode.Lookup, AttributeTypeCode.Customer, AttributeTypeCode.Owner };

        private ODataClient _odataClient;

        public bool UseEnumTypeForPicklistProperties { get; set; }
        public bool GenerateLookups { get; set; } = false;
        public bool GenerateNavigationProperties { get; set; } = false;
        public bool GenerateDynamicNavigationProperties { get; set; } = false;

        public async Task<IEnumerable<IToken>> GetEntityTokensAsync(ODataClient odataClient, ModelAssemblyView view)
        {
            var tokens = new List<IToken>();

            _odataClient = odataClient;

            foreach (var entityType in view.EntityTypes)
            {
                var entityDefinition = await EntityMetadata.GetEntityDefinition(_odataClient, entityType.Name).ConfigureAwait(false);
                var attributes = entityDefinition.Attributes.Select(a => new
                {
                    a.LogicalName,
                    a.SchemaName,
                    a.AttributeType,
                    a.DisplayName,
                    a.Description,
                    a.AttributeOf,
                    ODataName = GetODataName(a.AttributeType, a.LogicalName),
                    CSharpType = GetODataCSharpType(a.AttributeType),
                    a.MaxLength,
                    a.Targets,
                    ReferencingRelationships = entityDefinition.ManyToOneRelationships.Where(r => r.ReferencingEntity == a.EntityLogicalName && r.ReferencingAttribute == a.LogicalName).ToArray(),
                    DateTimeBehavior = a.DateTimeBehavior?.Value
                })
                    .Where(a => !view.BaseEntityProperties.Contains(a.ODataName))
                    .OrderBy(a => a.LogicalName);

                var entity = new EntityToken()
                {
                    Namespace = entityType.Namespace,
                    DisplayName = entityDefinition.DisplayName?.UserLocalizedLabel?.Label,
                    Description = entityDefinition.Description?.UserLocalizedLabel?.Label,
                    Entity = entityType.Name,
                };

                var entityTokenList = new List<IToken>();

                entityTokenList.Add(new EntitySetToken() { EntitySetName = entityDefinition.EntitySetName });
                entityTokenList.Add(new ODataTypeToken() { EntityName = entityDefinition.LogicalName });

                if (GenerateLookups)
                    entityTokenList.Add(new ToLookupToken()
                    {
                        EntityName = entityType.Name
                    });

                foreach (var a in attributes)
                {
                    var cSharpType = a.CSharpType;

                    if (_lookupTypeNames.Contains(a.AttributeType))
                    {
                        if (GenerateLookups)
                            foreach (var referencingRelationship in a.ReferencingRelationships)
                                entityTokenList.Add(new ODataLookupPropertyToken()
                                {
                                    DisplayName = a.DisplayName?.UserLocalizedLabel?.Label,
                                    Description = a.Description?.UserLocalizedLabel?.Label,
                                    ReferencingRelationshipSchemaName = referencingRelationship.SchemaName,
                                    ReferencingRelationshipReferencingEntityNavigationPropertyName = referencingRelationship.ReferencingEntityNavigationPropertyName,

                                });

                        if (GenerateNavigationProperties)
                            foreach (var referencingRelationship in a.ReferencingRelationships)
                            {
                                var referencedEntityType = view.EntityTypes.Where(t => t.Name == referencingRelationship.ReferencedEntity).FirstOrDefault();

                                if (referencedEntityType != null || GenerateDynamicNavigationProperties)
                                    entityTokenList.Add(new NavigationPropertyToken()
                                    {
                                        DisplayName = a.DisplayName?.UserLocalizedLabel?.Label,
                                        Description = a.Description?.UserLocalizedLabel?.Label,
                                        ReferencingRelationshipSchemaName = referencingRelationship.SchemaName,
                                        ReferencingRelationshipReferencingAttribute = referencingRelationship?.ReferencingAttribute,
                                        ReferencingRelationshipReferencedEntity = referencingRelationship?.ReferencedEntity,
                                        ReferencingRelationshipReferencedAttribute = referencingRelationship?.ReferencedAttribute,
                                        ReferencedEntityTypeFullName = referencedEntityType?.FullName,
                                        ReferencingRelationshipReferencingEntityNavigationPropertyName = referencingRelationship.ReferencingEntityNavigationPropertyName,

                                    });
                            }

                        entityTokenList.Add(new LookupPropertyToken()
                        {
                            ODataName = a.ODataName,
                            DisplayName = a.DisplayName?.UserLocalizedLabel?.Label,
                            Description = a.Description?.UserLocalizedLabel?.Label,
                            AttributeType = a.AttributeType.ToString(),
                            Targets = a.Targets,
                            CSharpType = cSharpType,

                        });

                        continue;
                    }

                    if (a.AttributeType == AttributeTypeCode.Picklist)
                    {
                        var picklist = await AppendPicklistEnum(entityType.Name, a.ODataName);
                        entityTokenList.Add(picklist);
                        cSharpType = $"{picklist.EnumName}?";
                    }

                    if (a.AttributeType == AttributeTypeCode.State)
                    {
                        entityTokenList.Add(new StateCodeToken() { UseEnumTypeForPicklistProperties = UseEnumTypeForPicklistProperties });

                        var picklist = await AppendStateEnum(entityType.Name, a.ODataName);
                        entityTokenList.Add(picklist);
                        cSharpType = $"{picklist.EnumName}?";
                    }                    
                    
                    if (a.AttributeType == AttributeTypeCode.Status)
                    {
                        entityTokenList.Add(new StatusCodeToken() { UseEnumTypeForPicklistProperties = UseEnumTypeForPicklistProperties });

                        var picklist = await AppendStatusEnum(entityType.Name, a.ODataName);
                        entityTokenList.Add(picklist);
                        cSharpType = $"{picklist.EnumName}?";
                    }                    
                    
                    if (!UseEnumTypeForPicklistProperties)
                        cSharpType = a.CSharpType;

                    entityTokenList.Add(new PropertyToken()
                    {
                        ODataName = a.ODataName,
                        DisplayName = a.DisplayName?.UserLocalizedLabel?.Label,
                        Description = a.Description?.UserLocalizedLabel?.Label,
                        AttributeOf = a.AttributeOf,
                        AttributeType = a.AttributeType.ToString(),
                        MaxLength = a.MaxLength ?? -1,
                        CSharpAttribute = GetCSharpAttribute(a.AttributeType, a.DateTimeBehavior),
                        CSharpType = cSharpType,

                    });

                    entity.ChildTokens = entityTokenList;
                }

                tokens.Add(entity);
            }

            return tokens;
        }

        private string GetCSharpAttribute(AttributeTypeCode attributeType, string dateTimeBehavior)
        {
            switch (attributeType)
            {
                case AttributeTypeCode.DateTime:
                    return $"JsonConverter(typeof(DateTimeConverter{dateTimeBehavior}))";
                default:
                    return null;
            }

        }


        private async Task<PicklistEnumToken> AppendPicklistEnum(string cdsTypeName, string attributeName)
        {
            var picklistAttributes = await PicklistAttributeMetadata.GetPicklistAttributeMetadataAsync(_odataClient, cdsTypeName, attributeName);
            var options = picklistAttributes.OptionSet.Options.Select(o => new KeyValuePair<string, int?>(o.Label?.UserLocalizedLabel?.Label, o.Value));

            return new PicklistEnumToken()
            {
                EnumName = $"{attributeName}Enum",
                Options = options
            };
        }

        private async Task<PicklistEnumToken> AppendStateEnum(string cdsTypeName, string attributeName)
        {
            var picklistAttributes = await PicklistAttributeMetadata.GetStateAttributeMetadataAsync(_odataClient, cdsTypeName, attributeName);
            var options = picklistAttributes.OptionSet.Options.Select(o => new KeyValuePair<string, int?>(o.Label?.UserLocalizedLabel?.Label, o.Value));

            return new PicklistEnumToken()
            {
                EnumName = $"{attributeName}Enum",
                Options = options
            };
        }

        private async Task<PicklistEnumToken> AppendStatusEnum(string cdsTypeName, string attributeName)
        {
            var picklistAttributes = await PicklistAttributeMetadata.GetStatusAttributeMetadataAsync(_odataClient, cdsTypeName, attributeName);
            var options = picklistAttributes.OptionSet.Options.Select(o => new KeyValuePair<string, int?>(o.Label?.UserLocalizedLabel?.Label, o.Value));

            return new PicklistEnumToken()
            {
                EnumName = $"{attributeName}Enum",
                Options = options
            };
        }

        private string GetODataCSharpType(AttributeTypeCode attributeType)
        {
            if (_lookupTypeNames.Contains(attributeType))
                return "Guid?";

            switch (attributeType)
            {
                case AttributeTypeCode.Boolean: // donotpostalmail,creditonhold,donotbulkpostalmail,donotbulkemail,donotfax
                    return "bool?";
                case AttributeTypeCode.Customer: // 
                    return "Guid?";
                case AttributeTypeCode.DateTime: // modifiedon,opendeals_date,createdon,lastusedincampaign,rdag_priceshiftdate
                    return "DateTimeOffset?";
                case AttributeTypeCode.Decimal: // exchangerate
                    return "decimal?";
                case AttributeTypeCode.Double: // address1_longitude,address2_latitude,address2_longitude,address1_latitude
                    return "double?";
                case AttributeTypeCode.Integer: // sharesoutstanding,onholdtime,address1_utcoffset,numberofemployees,opendeals_state
                    return "int?";
                case AttributeTypeCode.Lookup: // address1_addressid,entityimageid,stageid,address2_addressid,accountid
                    return "Guid?";
                case AttributeTypeCode.Memo: // description,address1_composite,address2_composite
                    return "string";
                case AttributeTypeCode.Money: // aging90,openrevenue,aging30,aging60_base,creditlimit
                    return "decimal?";
                case AttributeTypeCode.Owner: // ownerid
                    return "Guid?";
                case AttributeTypeCode.PartyList: // 
                    throw new NotImplementedException(attributeType.ToString());
                    return "int?";
                case AttributeTypeCode.Picklist: // address1_addresstypecode,address1_shippingmethodcode,accountcategorycode,address1_freighttermscode,customersizecode
                    return "int?";
                case AttributeTypeCode.State: // statecode
                    return "int?";
                case AttributeTypeCode.Status: // statuscode
                    return "int?";
                case AttributeTypeCode.String: // rdag_calculparam01,emailaddress3,emailaddress2,rdag_representativename,masteraccountidyominame
                    return "string";
                case AttributeTypeCode.Uniqueidentifier: // 
                    return "Guid?";
                case AttributeTypeCode.CalendarRules: // 
                    throw new NotImplementedException(attributeType.ToString());
                    return "int?";
                case AttributeTypeCode.Virtual: // address2_freighttermscodename,businesstypecodename,preferredappointmentdaycodename,accountcategorycodename,donotsendmarketingmaterialname
                    return "string";
                case AttributeTypeCode.BigInt: // entityimage_timestamp,versionnumber
                    return "long?";
                case AttributeTypeCode.ManagedProperty: // 
                    throw new NotImplementedException(attributeType.ToString());
                    return "int?";
                case AttributeTypeCode.EntityName: // owneridtype
                    return "string";
                default:
                    throw new NotImplementedException(attributeType.ToString());
            }
        }

        private string GetODataName(AttributeTypeCode attributeType, string logicalName)
        {
            if (_lookupTypeNames.Contains(attributeType))
                return $"_{logicalName}_value";

            return logicalName;
        }
    }
}
