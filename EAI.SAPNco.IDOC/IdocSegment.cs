using EAI.SAPNco.IDOC.Metadata;
using EAI.SAPNco.IDOC.Model.Structure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EAI.SAPNco.IDOC
{
    public class IdocSegment : IIdocSegmentContainer
    {
        private IdocSegmentMetadata _idocSegmentMetadata;
        private List<IdocSegment> _segments;

        private Dictionary<string, IdocField> _fields;
        private EDI_DD40 _dd40;

        public IdocSegment(IdocSegmentMetadata idocSegmentMetadata, EDI_DD40 dd40)
        {
            _idocSegmentMetadata = idocSegmentMetadata;
            _segments = new List<IdocSegment>();
            _fields = new Dictionary<string, IdocField>();
            _dd40 = dd40;

            if (_dd40 == null)
                _dd40 = new EDI_DD40();

            foreach (var idocFieldMetadata in _idocSegmentMetadata.Fields)
                _fields.Add(idocFieldMetadata.Name, new IdocField(this, idocFieldMetadata));
        }

        public IdocSegmentMetadata Metadata { get => _idocSegmentMetadata; }

        public IEnumerable<IdocSegment> ChildSegments { get => _segments; }
        public IEnumerable<IdocSegmentMetadata> ChildSegmentsMetadata { get => _idocSegmentMetadata?.ChildSegments; }
        public IEnumerable<IdocField> Fields { get => _fields.Values; }

        public EDI_DD40 DD40 { get => _dd40; }

        public string Name { get => _dd40.SEGNAM;  }

        public string Number { get => _dd40.SEGNUM; }

        public string ParentNumber { get => _dd40.PSGNUM; }

        public string HierarchyLevel { get => _dd40.HLEVEL; }


        public IdocSegment AddSegment(string segmentDef)
            => AddSegment(segmentDef, null);

        public IdocSegment AddSegment(EDI_DD40 dd40)
            => AddSegment(dd40.SEGNAM, dd40);

        public IdocSegment AddSegment(IdocSegment segment)
        {
            var isValidSegment = _idocSegmentMetadata.ChildSegments.Where(s => s.Name == segment.Metadata.Name).Any();

            if (!isValidSegment)
                throw new InvalidOperationException("Invalid child segment '{segmentDef}'");

            _segments.Add(segment);

            return segment;
        }

        private IdocSegment AddSegment(string segmentDef, EDI_DD40 dd40)
        {
            var childSegmentMetadata = _idocSegmentMetadata.ChildSegments.Where(s => s.Name == segmentDef).FirstOrDefault();

            if (childSegmentMetadata == null)
                throw new InvalidOperationException("Invalid child segment '{segmentDef}'");

            var segment = new IdocSegment(childSegmentMetadata, dd40);

            _segments.Add(segment);

            return segment;
        }

        public object this[string fieldName]
        {
            get
            {
                return _fields[fieldName].Value;
            }
            set
            {
                _fields[fieldName].Value = value;
            }
        }

        public IdocField GetField(string fieldName)
        {
            return _fields[fieldName];
        }

        public override string ToString()
            => $"{nameof(IdocSegment)}/{_dd40.SEGNUM}/{_dd40.PSGNUM}/{_idocSegmentMetadata.Name}";
    }
}
