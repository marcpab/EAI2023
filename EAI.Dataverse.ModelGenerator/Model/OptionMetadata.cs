using System;
using System.Collections.Generic;

namespace EAI.Dataverse.ModelGenerator.Model
{
    public class OptionMetadata
    {
        public string Color { get; set; }
        public Label Description { get; set; }
        public string ExternalValue { get; set; }
        public bool? HasChanged { get; set; }
        public bool? IsManaged { get; set; }
        public Label Label { get; set; }
        public Guid? MetadataId { get; set; }
        public List<int> ParentValues { get; set; }
        public string Tag { get; set; }
        public int? Value { get; set; }
    }
}
