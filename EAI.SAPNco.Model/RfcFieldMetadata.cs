namespace EAI.SAPNco.Model
{
    public class RfcFieldMetadata : RfcElementMetadata
    {
        public int _nucOffset;
        public int _ucOffset;

        public RfcStructureMetadata _structureMetadata;
        public RfcTableMetadata _tableMetadata;

        public override string ToString()
            => $"FIELD: {_name}, {_rfcDataType}";
    }
}