using EAI.SAPNco.IDOC.Metadata;
using EAI.SAPNco.IDOC.Model.Structure;
using System.Collections.Generic;

namespace EAI.SAPNco.IDOC
{
    public interface IIdocSegmentContainer
    {
        IEnumerable<IdocSegment> ChildSegments { get; }
        IEnumerable<IdocSegmentMetadata> ChildSegmentsMetadata { get; }

        IdocSegment AddSegment(EDI_DD40 dd40);
        IdocSegment AddSegment(IdocSegment segment);
        IdocSegment AddSegment(string segmentDef);
    }
}