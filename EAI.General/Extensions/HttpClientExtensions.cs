using EAI.General.ExtendableHttp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EAI.General.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> SendAsync(this HttpClient client, HttpRequestMessage requestMessage, IMessageHandler messageHandler)
        {
            return (messageHandler ?? SendMessageHandler.Instance).SendRequestAsync(requestMessage, client);
        }
    }
}
