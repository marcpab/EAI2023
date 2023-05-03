
using System;

namespace EAI.Dataverse.ModelGenerator.Model
{
    public class LocalizedLabel
    {
        public bool? HasChanged { get; set; }
        public bool? IsManaged { get; set; }
        public string Label { get; set; }
        public int? LanguageCode { get; set; }
        public Guid? MetadataId { get; set; }
    }
}
