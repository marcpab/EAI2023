using EAI.SAPNco.IDOC.Model.Structure;

namespace EAI.SAPNco.IDOC.Metadata
{
    public class IdocFieldValueMetadata
    {
        private IdocMetadata _idocMetadata;
        private EDI_IAPI14 _fieldValue;

        public IdocFieldValueMetadata(IdocMetadata idocMetadata, EDI_IAPI14 fieldValue)
        {
            _idocMetadata = idocMetadata;
            _fieldValue = fieldValue;
        }

        public string FieldName { get => _fieldValue.FIELDNAME; }
        public string SegmentType { get => _fieldValue.STRNAME; }
        public string Description { get => _fieldValue.DESCRP; }

        public string ValueL { get => _fieldValue.FLDVALUE_L; }
        public string ValueH { get => _fieldValue.FLDVALUE_H; }

        public override string ToString()
            => $"{nameof(IdocFieldMetadata)}/{ValueL}:{ValueH}";
    }
}
