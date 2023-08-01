using EAI.SAPNco.IDOC.Model;
using EAI.SAPNco.IDOC.Model.Structure;
using System.Collections.Generic;
using System.Linq;

namespace EAI.SAPNco.IDOC.Metadata
{
    public class IdocMetadata
    {
        private IDOCTYPE_READ_COMPLETE_Response _idoctype_read_complete;
        private IdocSegmentMetadata[] _segmentMetadata;
        private IdocFieldMetadata[] _fieldMetadata;
        private IdocFieldValueMetadata[] _fieldValueMetadata;
        private IdocMessageMetadata[] _messageMetadata;

        public static IdocMetadata FromIDOCTYPE_READ_COMPLETE(IDOCTYPE_READ_COMPLETE_Response idoctype_read_complete)
        {
            var idocMetadata = new IdocMetadata();

            idocMetadata._idoctype_read_complete = idoctype_read_complete;

            idocMetadata._segmentMetadata = idoctype_read_complete.PT_SEGMENTS.Select(s => new IdocSegmentMetadata(idocMetadata, s)).ToArray();
            idocMetadata._fieldMetadata = idoctype_read_complete.PT_FIELDS.Select(f => new IdocFieldMetadata(idocMetadata, f)).ToArray();
            idocMetadata._fieldValueMetadata = idoctype_read_complete.PT_FVALUES.Select(v => new IdocFieldValueMetadata(idocMetadata, v)).ToArray();
            idocMetadata._messageMetadata = idoctype_read_complete.PT_MESSAGES.Select(m => new IdocMessageMetadata(idocMetadata, m)).ToArray();

            return idocMetadata;
        }

        public EDI_IAPI10 Header { get => _idoctype_read_complete.PE_HEADER;  }

        public IEnumerable<IdocSegmentMetadata> Segments { get => _segmentMetadata; }
        public IEnumerable<IdocFieldMetadata> Fields { get => _fieldMetadata; }
        public IEnumerable<IdocFieldValueMetadata> FieldValuess { get => _fieldValueMetadata; }
        public IEnumerable<IdocMessageMetadata> Messages { get => _messageMetadata; }

        public IEnumerable<IdocSegmentMetadata> RootSegments { get => Segments.Where(s => string.IsNullOrEmpty(s.ParentSegment)); }

        public string Type { get => $"{_idoctype_read_complete.PE_HEADER.IDOCTYP}/{_idoctype_read_complete.PE_HEADER.CIMTYP}/{_idoctype_read_complete.PE_HEADER.RELEASED}";  }

        public override string ToString()
            => $"{nameof(IdocMetadata)}/{Type}";
    }
}
