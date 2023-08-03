using EAI.SAPNco.IDOC.Model.Structure;
using EAI.SAPNco.IDOC.ValueConverter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EAI.SAPNco.IDOC.Metadata
{
    public class IdocFieldMetadata
    {
        private IdocMetadata _idocMetadata;
        private EDI_IAPI12 _field;

        public IdocFieldMetadata(IdocMetadata idocMetadata, EDI_IAPI12 field)
        {
            _idocMetadata = idocMetadata;
            _field = field;
        }

        public IEnumerable<IdocFieldValueMetadata> Values { get => _idocMetadata.FieldValuess.Where(v => v.FieldName == Name && v.SegmentType == SegmentType); }

        public string Name { get => _field.FIELDNAME; }
        public string SegmentType { get => _field.SEGMENTTYP; }
        public string Description { get => _field.DESCRP; }

        public decimal InternalLenth { get => _field.INTLEN; }
        public decimal ExternalLenth { get => _field.EXTLEN; }

        public decimal Position { get => _field.FIELD_POS; }
        public decimal StartAtByte { get => _field.BYTE_FIRST; }
        public decimal EndAtByte { get => _field.BYTE_LAST; }

        public string RollName { get => _field.ROLLNAME; }
        public string DomainName { get => _field.DOMNAME; }
        public string IdocDataType { get => _field.DATATYPE; }
        public bool IsIsoCode { get => _field.ISOCODE == "X"; }
        public string ValueTable { get => _field.VALUETAB; }

        public DataTypeEnum DataType
        {
            get
            {
                if (Enum.TryParse<DataTypeEnum>(IdocDataType, out var dataType))
                    return dataType;

                return DataTypeEnum.Unknown;
            }
        }

        public Type ClrType { get => IdocFieldValueConverter.Instance.ClrType(DataType);  }

        public override string ToString()
            => $"{nameof(IdocFieldMetadata)}/{Name}/{Position}/{IdocDataType}";
    }
}
