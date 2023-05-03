using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace EAI.General.ExtendableHttp
{
    public class LogHandler : IMessageHandler
    {
        private IMessageHandler _messageHandler;
        private Action<string> _log;
        private bool _logRequestUri;
        private bool _logRequestContent;
        private bool _logResponse;
        private bool _logResponseContent;

        public IMessageHandler Next { get => _messageHandler; set => _messageHandler = value; }

        public Action<string> Log { get => _log; set => _log = value; }

        public bool LogRequestUri { get => _logRequestUri; set => _logRequestUri = value; }
        public bool LogRequestContent { get => _logRequestContent; set => _logRequestContent = value; }
        public bool LogResponse { get => _logResponse; set => _logResponse = value; }
        public bool LogResponseContent { get => _logResponseContent; set => _logResponseContent = value; }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage requestMessage, HttpClient client)
        {
            if (_logRequestUri)
                Log($"Request: {requestMessage.RequestUri}");

            if (_logRequestContent)
                Log($"Request content: {await requestMessage.Content.ReadAsStringAsync()}");

            var responseMessage = await _messageHandler.SendRequestAsync(requestMessage, client);

            if (_logResponse)
                Log($"Response: {responseMessage.StatusCode}, {responseMessage.ReasonPhrase}");

            if (_logResponseContent)
                Log($"Response content: {await responseMessage.Content.ReadAsStringAsync()}");

            return responseMessage;
        }
    }
}
