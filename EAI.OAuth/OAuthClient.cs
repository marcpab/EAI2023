using EAI.General;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EAI.OAuth
{
    public class OAuthClient
    {

        public static Task<OAuthTokenResponse> GetTokenAsync(Uri uri, string oauthRequest)
        {
            var httpClient = HttpClientCache.GetDefaultClient();

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = GetContent(oauthRequest)
            };




        }

        private static HttpContent GetContent(string oauthRequest) 
        {
            var content = new ByteArrayContent(Encoding.UTF8.GetBytes(oauthRequest.ToString()));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            return content;
        }

    }
}
