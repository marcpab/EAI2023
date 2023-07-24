namespace EAI.SAPNco.Model
{
    public class RfcStructureMetadata
    {
        public string _name;
        public RfcFieldMetadata[] _fields;

        public override string ToString()
            => $"STRUCTURE: {_name}";
    }
}