using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Rest
{
    public class RestRequest : IDisposable
    {
        private Func<Task<HttpClient>> _getHttpClientAsync;
        private Uri _uri;
        private HttpMethod _method;
        private JsonSerializerSettings _serializerSettings;
        private IMessageHandler _messageHandler;
        private object _requestData;

        private List<IDisposable> _disposables;

        public Uri Uri { get => _uri; }

        public RestRequest(Func<Task<HttpClient>> getHttpClientAsync, Uri uri, HttpMethod method, JsonSerializerSettings serializerSettings, IMessageHandler messageHandler, object requestData)
        {
            _disposables = new List<IDisposable>();

            _getHttpClientAsync = getHttpClientAsync;
            _uri = uri;
            _method = method;
            _serializerSettings = serializerSettings;
            _messageHandler = messageHandler;
            _requestData = requestData;
        }

        public async Task<HttpResponseMessage> GetResponseAsync()
        {
            var requestMessage = new HttpRequestMessage();
            _disposables.Add(requestMessage);

            requestMessage.Method = _method;
            requestMessage.Content = GetContent(_requestData);

            var client = await _getHttpClientAsync();
            _disposables.Add(client);

            var response = await SendRequestAsync(requestMessage, client);
            _disposables.Add(response);

            return response;
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

        public void Dispose()
        {
            foreach(var disposables in _disposables)
                disposables.Dispose();
        }
    }
}
