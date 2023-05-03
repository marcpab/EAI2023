using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Dataverse.ModelGenerator.Model
{
    public class AttributeRequiredLevelManagedProperty
    {
        public bool? CanBeChanged { get; set; }
        public string ManagedPropertyLogicalName { get; set; }
        public object Value { get; set; }
    }
}
