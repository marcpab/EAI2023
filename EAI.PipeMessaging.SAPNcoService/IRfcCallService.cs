using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    public interface IRfcCallService
    {
        Task ConnectAsync(string connectionString, string userName, string password);
        Task DisconnectAsync();
        Task<string> GetJRfcSchemaAsync(string functionName);
        Task RfcPingAsync();
        Task<string> RunJRfcRequestAsync(string jRfcRequestMessage, bool autoCommit);
    }
}