using EAI.General.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EAI.General.ExtendableHttp
{
    public class RetryPolicyHandler : IMessageHandler
    {
        private static HttpStatusCode[] _defaultRetryStatusCodes = new[] { HttpStatusCode.InternalServerError, HttpStatusCode.ServiceUnavailable, HttpStatusCode.RequestTimeout, HttpStatusCode.GatewayTimeout };


        private int _retryCount = 3;
        private int _waitSeconds = 3;
        private IMessageHandler _messageHandler;
        private HttpStatusCode[] _retryStatusCodes = _defaultRetryStatusCodes;

        public int RetryCount { get => _retryCount; set => _retryCount = value; }
        public int WaitSeconds { get => _waitSeconds; set => _waitSeconds = value; }
        public IMessageHandler Next { get => _messageHandler; set => _messageHandler = value; }
        public HttpStatusCode[] RetryStatusCodes { get => _retryStatusCodes; set => _retryStatusCodes = value; }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage requestMessage, HttpClient client)
        {
            var pendingTries = _retryCount;
            var waitSeconds = _waitSeconds;

            while(true)
            {
                pendingTries--;

                var response = await _messageHandler.SendRequestAsync(requestMessage.Clone(), client);

                if(response.IsSuccessStatusCode)
                    return response;

                if (pendingTries > 0 && _retryStatusCodes.Contains(response.StatusCode))
                {
                    await Task.Delay(TimeSpan.FromSeconds(waitSeconds));

                    waitSeconds += waitSeconds;

                    continue;
                }

                return response;
            }
        }
    }
}
