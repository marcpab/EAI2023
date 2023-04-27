using System.Collections.Generic;

namespace EAI.Dataverse.ModelGenerator.Model
{
    public class Label
    {
        public List<LocalizedLabel> LocalizedLabels { get; set; }
        public LocalizedLabel UserLocalizedLabel { get; set; }
    }
}
