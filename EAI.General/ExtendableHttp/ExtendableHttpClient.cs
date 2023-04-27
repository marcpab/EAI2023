using EAI.General.Cache;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EAI.General.ExtendableHttp
{
    public class ExtendableHttpClient
    {
        private IMessageHandler _messageHandler = SendMessageHandler.Instance;
        private Uri _baseUri;
        private TimeSpan? _timeout;

        public IMessageHandler MessageHandler { get => _messageHandler; set => _messageHandler = value; }
        public Uri BaseUri { get => _baseUri; set => _baseUri = value; }
        public TimeSpan? Timeout { get => _timeout; set => _timeout = value; }

        protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            var client = await HttpClientCache.GetHttpClientAsync(_baseUri, _timeout);

            return await _messageHandler.SendRequestAsync(requestMessage, client);
        }
    }
}
