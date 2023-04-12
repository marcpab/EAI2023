using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EAI.Rest
{
    public class RestClient
    {
        private Func<Task<HttpClient>> getHttpClientAsync;
        private Uri _realtiveUri;
        private JsonSerializerSettings _serializerSettings;
        private IMessageHandler _messageHandler;

        public async Task<RestResponse<T>> SendAsync<T>(HttpMethod method, string query, object requestData)
        {
            var requestMessage = new HttpRequestMessage();
            requestMessage.Method = method;

            requestMessage.Content = GetContent(requestData);

            var client = await getHttpClientAsync();

            var response = await SendRequestAsync(requestMessage, client);

            return new RestResponse<T>(response);
        }

        private static async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage requestMessage, HttpClient client)
        {
            return await client.SendAsync(requestMessage);
        }

        private HttpContent GetContent(object request)
        {
            if (request == null)
                return null;

            if (request is string)
                return new StringContent((string)request);

            return new StringContent(JsonConvert.SerializeObject(request, _serializerSettings));
        }
    }
}
