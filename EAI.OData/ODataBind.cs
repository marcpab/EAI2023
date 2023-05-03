using Newtonsoft.Json.Linq;
using System;

namespace EAI.OData
{
    public class ODataBind
    {
        public const string PropertyPostfix = "Bind";

        public string EntityName { get; set; }

        public Guid EntityId { get; set; }

        public JToken ToJToken()
        {
            return JToken.FromObject(ToString());
        }

        public override string ToString()
        {
            return $"{EntityName}({EntityId})";
        }
    }
}
