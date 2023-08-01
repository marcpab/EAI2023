using EAI.SAPNco.IDOC.Metadata;
using EAI.SAPNco.IDOC.ValueConverter;

namespace EAI.SAPNco.IDOC
{
    public class IdocField
    {
        private const int _fieldOffset = 64;

        private IdocFieldMetadata _idocFieldMetadata;
        private IdocSegment _segment;

        public IdocField(IdocSegment segment, IdocFieldMetadata idocFieldMetadata)
        {
            _segment = segment;
            _idocFieldMetadata = idocFieldMetadata;
        }

        public IdocFieldMetadata Metadata { get => _idocFieldMetadata; }

        public object Value {  
            get
            {
                var value = GetRawValue();

                return IdocFieldValueConverter.Instance.FromIDOC(_idocFieldMetadata.DataType, value);
            }
            set
            {
                SetRawValue(IdocFieldValueConverter.Instance.ToIDOC(_idocFieldMetadata.DataType, value, (int)_idocFieldMetadata.ExternalLenth));
            }
        }

        public override string ToString()
            => $"{nameof(IdocField)}/{_idocFieldMetadata.Name}/{Value}";

        private string GetRawValue()
        {
            var startAtByte = (int)_idocFieldMetadata.StartAtByte - _fieldOffset;
            var endAtByte = (int)_idocFieldMetadata.EndAtByte - _fieldOffset;

            var length = endAtByte - startAtByte + 1;

            if (startAtByte >= _segment.DD40.SDATA.Length)
                return string.Empty;

            if (endAtByte >= _segment.DD40.SDATA.Length)
                length = _segment.DD40.SDATA.Length - startAtByte;

            return _segment.DD40.SDATA.Substring(startAtByte, length);
        }

        private void SetRawValue(string fieldValue)
        {
            if(fieldValue == null)
                fieldValue = string.Empty;

            var startAtByte = (int)_idocFieldMetadata.StartAtByte - _fieldOffset;
            var endAtByte = (int)_idocFieldMetadata.EndAtByte - _fieldOffset;

            var length = endAtByte - startAtByte + 1;

            if (fieldValue.Length > length)
                fieldValue = fieldValue.Substring(0, length);
            else
                fieldValue = fieldValue.PadRight(length);

            _segment.DD40.SDATA = _segment.DD40.SDATA.PadRight(endAtByte).Remove(startAtByte, length).Insert(startAtByte, fieldValue);
        }

    }

}
