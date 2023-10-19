using EAI.SAPNco.Utils.Model;

namespace EAI.SAPNco.Utils.ReadTable
{
    public class SapReadTableField
    {
        private RFC_DB_FLD _field;

        public SapReadTableField(RFC_DB_FLD field)
        {
            _field = field;
        }

        public string Name { get => _field.FIELDNAME; }
        public int Length { get => (int)_field.LENGTH; }
        public string Description { get => _field.FIELDTEXT; }
        public string Type { get => _field.TYPE; }
    }
}