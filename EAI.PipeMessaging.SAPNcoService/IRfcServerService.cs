using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    public interface IRfcServerService
    {
        Task StartAsync(string connectionString, string userName, string password);
        Task StopAsync();

        void SetCallback(IRfcServerCallbackAsync rfcServerCallback);
    }
}