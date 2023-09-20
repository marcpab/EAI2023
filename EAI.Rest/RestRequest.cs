using EAI.General.ExtendableHttp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Rest
{
    public class RestRequest // : IDisposable
    {
        private HttpMethod _method;
        private object _path;
        private object _requestData;
        private Dictionary<string, string> _requestHeaders;

        public HttpMethod Method { get => _method; set => _method = value; }
        public object Path { get => _path; set => _path = value; }
        public object Content { get => _requestData; set => _requestData = value; }
        public Dictionary<string, string> RequestHeaders { get => _requestHeaders; set => _requestHeaders = value; }

        public RestRequest()
        {
        }

        public virtual HttpRequestMessage CreateHttpRequestMessage(JsonSerializerSettings serializerSettings)
        {
            var requestMessage = new HttpRequestMessage();
            requestMessage.Method = _method;

            requestMessage.Content = GetContent(serializerSettings);
            requestMessage.RequestUri = Path == null ? null : new Uri(Path?.ToString(), UriKind.RelativeOrAbsolute);

            if(_requestHeaders != null)
                foreach(var header  in _requestHeaders)
                    requestMessage.Headers.Add(header.Key, header.Value);   

            return requestMessage;
        }

        private HttpContent GetContent(JsonSerializerSettings serializerSettings)
        {
            if (_requestData == null)
                return null;

            if (_requestData is string)
                return new StringContent((string)_requestData);

            return new StringContent(JsonConvert.SerializeObject(_requestData, serializerSettings), Encoding.UTF8, "application/json");
        }

        //public void Dispose()
        //{
        //    foreach(var disposables in _disposables)
        //        disposables.Dispose();
        //}
    }
}
