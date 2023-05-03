using EAI.General;
using EAI.General.Extensions;
using EAI.General.ExtendableHttp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EAI.OAuth
{
    public class OAuthClient : ExtendableHttpClient
    {
        public OAuthClient()
        {
            MessageHandler = new RetryPolicyHandler
            {
                RetryCount = 3,
                WaitSeconds = 1,
                RetryStatusCodes = new[] { HttpStatusCode.InternalServerError, HttpStatusCode.ServiceUnavailable, HttpStatusCode.RequestTimeout, HttpStatusCode.GatewayTimeout },

                Next = SendMessageHandler.Instance
            };
        }

        public async Task<OAuthResponse> GetTokenAsync(Uri uri, string oauthRequest, HttpMethod httpMethod = null)
        {
            using (var httpRequest = new HttpRequestMessage(httpMethod ?? HttpMethod.Post, uri)
                                        {
                                            Content = GetRequestContent(oauthRequest)
                                        })
            using (var response = await SendAsync(httpRequest))
            {
                response.EnsureSuccessStatusCode();

                using (var content = response.Content)
                    return JsonConvert.DeserializeObject<OAuthResponse>(await content.ReadAsStringAsync());
            }
        }

        private static HttpContent GetRequestContent(string oauthRequest) 
        {
            var content = new ByteArrayContent(Encoding.UTF8.GetBytes(oauthRequest.ToString()));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            return content;
        }

    }
}
