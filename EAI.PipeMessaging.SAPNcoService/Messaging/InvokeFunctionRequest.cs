using Newtonsoft.Json.Linq;

namespace EAI.PipeMessaging.SAPNcoService.Messaging
{
    internal class InvokeFunctionRequest
    {
        public string _functionName { get; set; }
        public JObject _functionData { get; set; }
    }
}