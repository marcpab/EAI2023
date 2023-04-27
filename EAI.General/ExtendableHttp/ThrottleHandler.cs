using EAI.General.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.General.ExtendableHttp
{
    public class ThrottleHandler : IMessageHandler
    {
        private const string _ratelimitBurstHeaderName = "x-ms-ratelimit-burst-remaining-xrm-requests";
        private const string _ratelimitTimeHeaderName = "x-ms-ratelimit-time-remaining-xrm-requests";
        private const string _retryAfterTimeHeaderName = "Retry-After";
        private const HttpStatusCode _tooManyRequests = (HttpStatusCode)429;

        private static int _requestCount;

        private IMessageHandler _messageHandler;
        private Action<string> _log;

        public IMessageHandler Next { get => _messageHandler; set => _messageHandler = value; }
        public Action<string> Log { get => _log; set => _log = value; }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage requestMessage, HttpClient client)
        {
            while(true) 
            { 
                var responseMessage = await _messageHandler.SendRequestAsync(requestMessage.Clone(), client);

                if(responseMessage.StatusCode == _tooManyRequests)
                {
                    var retryAfter = responseMessage.Headers.GetValues(_retryAfterTimeHeaderName).FirstOrDefault().TryParseInt() ?? 0;
                    if (retryAfter > 0)
                        await Task.Delay(TimeSpan.FromSeconds(retryAfter));

                    continue;
                }

                if(_log != null && Interlocked.Increment(ref _requestCount) % 1000 == 0) 
                {
                    var ratelimitBurst = responseMessage.Headers.GetValues(_ratelimitBurstHeaderName).FirstOrDefault().TryParseInt();
                    var ratelimitTime = responseMessage.Headers.GetValues(_ratelimitTimeHeaderName).FirstOrDefault().TryParseInt();

                    Log($"rate limit burst: {ratelimitBurst}, rate limit time {ratelimitTime}");
                }

                return responseMessage;
            }
        }
    }
}
