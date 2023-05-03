using EAI.General.Cache;
using EAI.General.Extensions;
using EAI.General.ExtendableHttp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EAI.OAuth
{
    public class OAuthMessageHandler : IMessageHandler
    {
        private OAuthClient _oauthClient = new OAuthClient();
        private OAuthRequest _oauthRequest;
        private Uri _uri;
        private HttpMethod _method;
        private int _defaultLivetime;
        private IMessageHandler _next;

        public OAuthClient OAuthClient { get => _oauthClient; set => _oauthClient = value; }
        public OAuthRequest OAuthRequest { get => _oauthRequest; set => _oauthRequest = value; }

        public IMessageHandler Next { get => _next; set => _next = value; }
        public Uri Uri { get => _uri; set => _uri = value; }
        public HttpMethod Method { get => _method; set => _method = value; }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage requestMessage, HttpClient client)
        {
            var oauthRequest = _oauthRequest.ToString();

            var oauthResponse = await ResourceCache<OAuthResponse>.GetResourceAsync(oauthRequest, GetOAuthTokenAsync);

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", oauthResponse.access_token);

            return await _next.SendRequestAsync(requestMessage, client);
        }

        private async Task<ResourceCacheItem<OAuthResponse>> GetOAuthTokenAsync()
        {
            var oauthResponse = await _oauthClient.GetTokenAsync(_uri, _oauthRequest.ToString(), _method);

            var expiresIn = oauthResponse.expires_in.TryParseInt() ?? 0;
            if (expiresIn == 0)
                expiresIn = _defaultLivetime;

            return new ResourceCacheItem<OAuthResponse>(oauthResponse) { ExpiresOn = DateTime.UtcNow.AddSeconds(expiresIn - 60) };
        }
    }
}
