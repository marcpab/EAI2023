using EAI.OnPrem.Storage;

namespace EAI.OnPrem.SAPNcoService
{
    public class GetJRfcSchemaRequest : OnPremMessage
    {
        public string _name { get; set; }
        public string _functionName { get; set; }
    }
}