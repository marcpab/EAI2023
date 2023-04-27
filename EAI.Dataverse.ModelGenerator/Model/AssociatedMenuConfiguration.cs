using System;

namespace EAI.Dataverse.ModelGenerator.Model
{
    public class AssociatedMenuConfiguration
    {
        public bool? AvailableOffline { get; set; }
        public object Behavior { get; set; }
        public object Group { get; set; }
        public string Icon { get; set; }
        public bool? IsCustomizable { get; set; }
        public Label Label { get; set; }
        public string MenuId { get; set; }
        public int? Order { get; set; }
        public string QueryApi { get; set; }
        public Guid? ViewId { get; set; }
    }
}
