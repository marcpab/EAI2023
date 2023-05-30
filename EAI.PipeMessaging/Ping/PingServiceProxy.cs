using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.Ping
{
    public class PingServiceProxy : PipeObject
    {
        private PingService _pingService = new PingService();

        public PingServiceProxy()
        {
            _pingService.PingBackEvent = (s) =>
                SendRequest<PingBackEventResponse>(new PingBackEventRequest
                {
                    _arg = s
                });

            AddMethod<PingRequestMessage, PingResponseMessage>(async r => new PingResponseMessage { _ret = await _pingService.PingAsync(r.request) });
        }
    }
}
