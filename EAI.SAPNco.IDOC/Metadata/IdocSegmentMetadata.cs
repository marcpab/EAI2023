using EAI.SAPNco.IDOC.Model.Structure;
using System.Collections.Generic;
using System.Linq;

namespace EAI.SAPNco.IDOC.Metadata
{
    public class IdocSegmentMetadata
    {
        private IdocMetadata _idocMetadata;
        private EDI_IAPI11 _segment;

        public IdocSegmentMetadata(IdocMetadata idocMetadata, EDI_IAPI11 segment)
        {
            _idocMetadata = idocMetadata;
            _segment = segment;
        }

        public IEnumerable<IdocSegmentMetadata> ChildSegments { get => _idocMetadata.Segments.Where(s => s.ParentSegment == Type); }
        public IEnumerable<IdocFieldMetadata> Fields { get => _idocMetadata.Fields.Where(f => f.SegmentType == Type); }

        public string Name { get => _segment.SEGMENTDEF; }
        public string ParentSegment { get => _segment.PARSEG; }
        public decimal HierarchyLevel { get => _segment.HLEVEL; }
        public string Description { get => _segment.DESCRP; }
        public decimal Number {  get => _segment.NR; }
        public string Type { get => _segment.SEGMENTTYP; }
        public string RefType { get => _segment.REFSEGTYP; }
        public bool IsQualifier { get => _segment.QUALIFIER == "X"; }
        public decimal Length { get => _segment.SEGLEN; }
        public decimal ParentNumber { get => _segment.PARPNO; }
        public bool IsRequired { get => _segment.MUSTFL == "X"; }
        public decimal MinOccurrence { get => _segment.OCCMIN; }
        public decimal MaxOccurrence { get => _segment.OCCMAX; }
        public bool IsGroup { get => _segment.PARFLG == "X"; }
        public bool IsGroupRequired { get => _segment.MUSTFL == "X"; }
        public decimal MinGroupOccurrence { get => _segment.OCCMIN; }
        public decimal MaxGroupOccurrence { get => _segment.OCCMAX; }

        public override string ToString()
            => $"{nameof(IdocSegment)}/{Type}/{Name}";
    }
}
