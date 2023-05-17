using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.Ping
{
    internal class PingService : IPingService
    {

        public Func<string, Task> PingBackEvent { get; set; }

        public async Task<string> PingAsync(string request)
        {
            await PingBackEvent(request);

            return request;
        }

    }
}
