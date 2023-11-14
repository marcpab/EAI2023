using Newtonsoft.Json.Linq;

namespace EAI.JOData.Base
{
    internal class PreProcessRequest
    {
        public bool IsValid { get; set; } = true;
        public string? Error { get; set; }
        public string? Entity { get; set; }
        public string? RecordIdName { get; set; }
        public Dictionary<string, JToken> Tokens { get; set; } = new();
    }
}
