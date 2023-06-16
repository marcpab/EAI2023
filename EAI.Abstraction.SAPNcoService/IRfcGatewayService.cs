using System.Threading.Tasks;

namespace EAI.Abstraction.SAPNcoService
{
    public interface IRfcGatewayService
    {
        Task<string> CallRfcAsync(string name, string jRfcRequestMessage, bool autoCommit = false);
        Task<string> GetJRfcSchemaAsync(string name, string functionName);
    }
}