using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EAI.Dataverse.ModelGenerator.Model
{
    public class AttributeMetadata
    {
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }
        public string AttributeOf { get; set; }
        public AttributeTypeCode AttributeType { get; set; }
        public AttributeTypeDisplayName AttributeTypeName { get; set; }
        public string AutoNumberFormat { get; set; }
        public bool? CanBeSecuredForCreate { get; set; }
        public bool? CanBeSecuredForRead { get; set; }
        public bool? CanBeSecuredForUpdate { get; set; }
        public BooleanManagedProperty CanModifyAdditionalSettings { get; set; }
        public int? ColumnNumber { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string DeprecatedVersion { get; set; }
        public Label Description { get; set; }
        public Label DisplayName { get; set; }
        public string EntityLogicalName { get; set; }
        public string ExternalName { get; set; }
        public bool? HasChanged { get; set; }
        public string InheritsFrom { get; set; }
        public string IntroducedVersion { get; set; }
        public BooleanManagedProperty IsAuditEnabled { get; set; }
        public bool? IsCustomAttribute { get; set; }
        public BooleanManagedProperty IsCustomizable { get; set; }
        public bool? IsDataSourceSecret { get; set; }
        public bool? IsFilterable { get; set; }
        public BooleanManagedProperty IsGlobalFilterEnabled { get; set; }
        public bool? IsLogical { get; set; }
        public bool? IsManaged { get; set; }
        public bool? IsPrimaryId { get; set; }
        public bool? IsPrimaryName { get; set; }
        public BooleanManagedProperty IsRenameable { get; set; }
        public bool? IsRequiredForForm { get; set; }
        public bool? IsRetrievable { get; set; }
        public bool? IsSearchable { get; set; }
        public bool? IsSecured { get; set; }
        public BooleanManagedProperty IsSortableEnabled { get; set; }
        public BooleanManagedProperty IsValidForAdvancedFind { get; set; }
        public bool? IsValidForCreate { get; set; }
        public bool? IsValidForForm { get; set; }
        public bool? IsValidForGrid { get; set; }
        public bool? IsValidForRead { get; set; }
        public bool? IsValidForUpdate { get; set; }
        public bool? IsValidODataAttribute { get; set; }
        public Guid? LinkedAttributeId { get; set; }
        public string LogicalName { get; set; }
        public Guid? MetadataId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public AttributeRequiredLevelManagedProperty RequiredLevel { get; set; }
        public string SchemaName { get; set; }
        public int? SourceType { get; set; }

// string
        public object Format { get; set; }
        public StringFormatName FormatName { get; set; }
        public object ImeMode { get; set; }
        public int? MaxLength { get; set; }
        public string YomiOf { get; set; }
        public bool? IsLocalizable { get; set; }
        public int? DatabaseLength { get; set; }
        public string FormulaDefinition { get; set; }
        public int? SourceTypeMask { get; set; }

// lookup
        public List<string> Targets { get; set; }

// Datetime
        public DateTime? MinSupportedValue { get; set; }
        public DateTime? MaxSupportedValue { get; set; }
        public DateTimeBehavior DateTimeBehavior { get; set; }
        public BooleanManagedProperty CanChangeDateTimeBehavior { get; set; }

// double/decimal
        public decimal? MaxValue { get; set; }
        public decimal? MinValue { get; set; }

// enum
        public int? DefaultFormValue { get; set; }
    }
}
