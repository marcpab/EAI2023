using EAI.SAPNco.Model;
using System.Threading.Tasks;

namespace EAI.Abstraction.SAPNcoService
{
    public interface IRfcGatewayService
    {
        Task<string> CallRfcAsync(string name, string jRfcRequestMessage, bool autoCommit = false);
        Task<T> CallRfcAsync<T>(string name, T jRfcRequestMessage, bool autoCommit = false);
        Task<string> GetJRfcSchemaAsync(string name, string functionName);
        Task<RfcFunctionMetadata> GetRfcFunctionMetadataAsync(string name, string functionName);
    }
}