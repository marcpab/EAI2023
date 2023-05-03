using System.Collections.Generic;

namespace EAI.Dataverse.ModelGenerator.Model
{
    public class OptionSetMetadata
    {
        public Label Description { get; set; }
        public Label DisplayName { get; set; }
        public string ExternalTypeName { get; set; }
        public bool? HasChanged { get; set; }
        public string IntroducedVersion { get; set; }
        public BooleanManagedProperty IsCustomizable { get; set; }
        public bool? IsCustomOptionSet { get; set; }
        public bool? IsGlobal { get; set; }
        public bool? IsManaged { get; set; }
        public string MetadataId { get; set; }
        public string Name { get; set; }
        public List<OptionMetadata> Options { get; set; }
        public string OptionSetType { get; set; }
        public string ParentOptionSetName { get; set; }
    }
}
