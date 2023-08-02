using EAI.SAPNco.Model;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    public interface IRfcGatewayService
    {
        Task ConnectAsync(string connectionString, string userName, string password);
        Task DisconnectAsync();
        Task<string> GetJRfcSchemaAsync(string functionName);
        Task<RfcFunctionMetadata> GetRfcFunctionMetadataAsync(string functionName);
        Task RfcPingAsync();
        Task<string> RunJRfcRequestAsync(string jRfcRequestMessage, bool autoCommit);
        Task StartServerAsync(IRfcServerCallbackAsync rfcServerCallback);
        Task StopServerAsync();
    }
}