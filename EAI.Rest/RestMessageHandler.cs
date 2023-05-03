using EAI.General.ExtendableHttp;
using EAI.OAuth;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Rest
{
    public class RestMessageHandler : IMessageHandler
    {
        private RetryPolicyHandler _retryPolicyHandler;
        private ThrottleHandler _throttleHandler;
        private OAuthMessageHandler _oauthHandler;

        public RestMessageHandler()
        {
            _retryPolicyHandler = new RetryPolicyHandler();
            _throttleHandler = new ThrottleHandler();
            _oauthHandler = new OAuthMessageHandler();

            _retryPolicyHandler.Next = _throttleHandler;
            _throttleHandler.Next = _oauthHandler;
            _oauthHandler.Next = SendMessageHandler.Instance;
        }

        public IMessageHandler Next { get => _throttleHandler.Next; set => _throttleHandler.Next = value; }
        public RetryPolicyHandler RetryPolicyHandler { get => _retryPolicyHandler; set => _retryPolicyHandler = value; }
        public ThrottleHandler ThrottleHandler { get => _throttleHandler; set => _throttleHandler = value; }
        public OAuthMessageHandler OAuthHandler { get => _oauthHandler; set => _oauthHandler = value; }

        public Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage requestMessage, HttpClient client)
        {
            return _retryPolicyHandler.SendRequestAsync(requestMessage, client);
        }
    }
}
