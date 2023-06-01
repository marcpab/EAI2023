using System.Threading.Tasks;

namespace EAI.Abstraction.SAPNcoService
{
    public interface IRfcGatewayService
    {
        Task<string> CallRfcAsync(string name, string jRfcRequestMessage);
        Task<string> GetJRfcSchemaAsync(string name, string functionName);
    }
}