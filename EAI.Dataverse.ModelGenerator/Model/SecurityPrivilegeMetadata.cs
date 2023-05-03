
using System;

namespace EAI.Dataverse.ModelGenerator.Model
{
    public class SecurityPrivilegeMetadata
    {
        public bool? CanBeBasic { get; set; }
        public bool? CanBeDeep { get; set; }
        public bool? CanBeGlobal { get; set; }
        public bool? CanBeLocal { get; set; }
        public bool? CanBeEntityReference { get; set; }
        public bool? CanBeParentEntityReference { get; set; }
        public string Name { get; set; }
        public Guid? PrivilegeId { get; set; }
        public object PrivilegeType { get; set; }
    }
}
