using EAI.SAPNco.IDOC.Metadata;
using EAI.SAPNco.IDOC.Model.Structure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EAI.SAPNco.IDOC
{
    public class Idoc : IIdocSegmentContainer
    {
        private IdocMetadata _idocMetadata;
        private List<IdocSegment> _segments;
        private EDI_DC40 _dc40;

        public Idoc(IdocMetadata idocMetadata, EDI_DC40 dc40)
        {
            _idocMetadata = idocMetadata;
            _dc40 = dc40;
            _segments = new List<IdocSegment>();
        }

        public IdocMetadata Metadata { get => _idocMetadata; }

        public IEnumerable<IdocSegment> ChildSegments { get => _segments; }
        public IEnumerable<IdocSegmentMetadata> ChildSegmentsMetadata { get => _idocMetadata?.RootSegments; }

        public string Number { get => _dc40.GetIdocNumber(); }
        public string Type { get => _dc40.GetIdocType(); }
        public EDI_DC40 DC40 { get => _dc40; }

        public IdocSegment AddSegment(string segmentDef)
            => AddSegment(segmentDef, null);

        public IdocSegment AddSegment(EDI_DD40 dd40)
            => AddSegment(dd40.SEGNAM, dd40);

        public IdocSegment AddSegment(IdocSegment segment)
        {
            var isValidSegment = _idocMetadata.RootSegments.Where(s => s.Name == segment.Metadata.Name).Any();

            if (!isValidSegment)
                throw new InvalidOperationException("Invalid child segment '{segmentDef}'");

            _segments.Add(segment);

            return segment;
        }

        public override string ToString()
            => $"{nameof(Idoc)}/{Type}/{Number}";

        private IdocSegment AddSegment(string segmentDef, EDI_DD40 dd40)
        {
            var childSegmentMetadata = _idocMetadata.RootSegments.Where(s => s.Name == segmentDef).FirstOrDefault();

            if (childSegmentMetadata == null)
                throw new InvalidOperationException("Invalid child segment '{segmentDef}'");

            var segment = new IdocSegment(childSegmentMetadata, dd40);

            _segments.Add(segment);

            return segment;
        }
    }
}
