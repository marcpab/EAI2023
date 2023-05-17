using System;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.Ping
{
    public interface IPingService
    {
        Func<string, Task> PingBackEvent { get; set; }

        Task<string> PingAsync(string request);
    }
}