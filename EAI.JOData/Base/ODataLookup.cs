using Newtonsoft.Json.Linq;

namespace EAI.JOData.Base
{
    public class ODataLookup
    {
        public string EntityName { get; }
        public Guid EntityId { get; }
        public ODataLookup(string entityName, Guid entityId) => (EntityName, EntityId) = (entityName, entityId);

        public override string ToString()
        {
            return $"{EntityName}({EntityId})";
        }

        public JToken ToJToken()
        {
            return JToken.FromObject(ToString());
        }
    }
}
