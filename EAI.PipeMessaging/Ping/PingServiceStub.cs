using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.Ping
{
    public class PingServiceStub : PipeObject, IPingService
    {
        public PingServiceStub()
        {
            AddMethod<PingBackEventRequest, PingBackEventResponse>(async r =>
                    {
                        if (PingBackEvent != null)
                            await PingBackEvent(r._arg);

                        return new PingBackEventResponse();
                    });

        }

        public Func<string, Task> PingBackEvent { get; set; }

        public static async Task<PingServiceStub> CreateObjectAsync(string pipeName = null)
        {
            var stub = new PingServiceStub();

            await stub.CreateRemoteInstance<PingServiceProxy>(pipeName);

            return stub;
        }

        public async Task<string> PingAsync(string request)
        {
            var pingRequestMessage = new PingRequestMessage()
            {
                request = request
            };

            var pingResponseMessage = await SendRequest<PingResponseMessage>(pingRequestMessage);

            return pingResponseMessage._ret;
        }
    }
}
