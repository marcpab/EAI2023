using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Model
{
    public class OneToManyRelationshipMetadata
    {
        public AssociatedMenuConfiguration AssociatedMenuConfiguration { get; set; }
        public CascadeConfiguration CascadeConfiguration { get; set; }
        public string DenormalizedAttributeName { get; set; }
        public string EntityKey { get; set; }
        public bool? HasChanged { get; set; }
        public string IntroducedVersion { get; set; }
        public BooleanManagedProperty IsCustomizable { get; set; }
        public bool? IsCustomRelationship { get; set; }
        public bool? IsDenormalizedLookup { get; set; }
        public bool? IsHierarchical { get; set; }
        public bool? IsManaged { get; set; }
        public bool? IsRelationshipAttributeDenormalized { get; set; }
        public bool? IsValidForAdvancedFind { get; set; }
        public Guid? MetadataId { get; set; }
        public string ReferencedAttribute { get; set; }
        public string ReferencedEntity { get; set; }
        public string ReferencedEntityNavigationPropertyName { get; set; }
        public string ReferencingAttribute { get; set; }
        public string ReferencingEntity { get; set; }
        public string ReferencingEntityNavigationPropertyName { get; set; }
        public List<object> RelationshipAttributes { get; set; }
        public int? RelationshipBehavior { get; set; }
        public object RelationshipType { get; set; }
        public string SchemaName { get; set; }
        public object SecurityTypes { get; set; }
    }
}
