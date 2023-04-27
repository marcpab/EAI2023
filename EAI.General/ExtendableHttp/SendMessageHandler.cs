using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace EAI.General.ExtendableHttp
{
    public class SendMessageHandler : IMessageHandler
    {
        public static readonly SendMessageHandler Instance = new SendMessageHandler();

        public IMessageHandler Next { get => null; set => throw new NotImplementedException(); }

        public Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage requestMessage, HttpClient client)
        {
            return client.SendAsync(requestMessage);
        }
    }
}
