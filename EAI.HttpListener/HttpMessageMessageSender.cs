using EAI.General;
using EAI.LoggingV2;
using EAI.LoggingV2.Levels;
using EAI.Messaging.Abstractions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.HttpListener
{
    public class HttpMessageMessageSender : IMessageSender
    {
        private LoggerV2 _log;
        private string _baseAddress;
        private IClientHttpAuth _auth;

        public LoggerV2 Log { get => _log; set => _log = value; }
        public string BaseAddress { get => _baseAddress; set => _baseAddress = value; }

        public IClientHttpAuth Auth { get => _auth; set => _auth = value; }

        public async Task SendMessageAsync(object message, string messageType, string transactionKey)
        {
            var httpMessage = GetMessage(message);

            var httpClient = new HttpClient();


            using (var stream = new MemoryStream())
            {
                stream.Write(httpMessage._content);
                await stream.FlushAsync();
                stream.Position = 0;

                using (var content = new StreamContent(stream))
                {
                    foreach (var header in httpMessage._headers)
                        content.Headers.TryAddWithoutValidation(header.Key, header.Value);


                    var httpRequestMessage = new HttpRequestMessage()
                    {
                        Method = new HttpMethod(httpMessage._method),
                        RequestUri = GetRequestUri(httpMessage),
                        Version = new Version(httpMessage._version),
                        Content = content,
                    };

                    foreach (var header in httpMessage._headers)
                        httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);

                    if (_auth != null)
                        httpRequestMessage.Headers.Authorization = await _auth.GetAuthenticationHeaderValueAsync();

                    _log?.String<Info>($"Send http {httpRequestMessage.Method.Method} request to {httpRequestMessage.RequestUri}");
                    _log?.Message<Debug>(nameof(httpRequestMessage), httpRequestMessage, "request message");

                    var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                    _log?.String<Info>($"Received http response {(int)httpResponseMessage.StatusCode} {httpResponseMessage.ReasonPhrase}");
                    _log?.Message<Debug>(nameof(httpResponseMessage), httpResponseMessage, "response message");

                    httpResponseMessage.EnsureSuccessStatusCode();
                }
            }
        }

        private Uri GetRequestUri(HttpMessage httpMessage)
        {
            if(string.IsNullOrEmpty(_baseAddress))
                return new Uri(httpMessage._uri, UriKind.RelativeOrAbsolute);

            return new Uri(new Uri(_baseAddress, UriKind.RelativeOrAbsolute), new Uri(httpMessage._uri, UriKind.RelativeOrAbsolute));
        }

        public Task SendMessageAsync(object message)
        {
            return SendMessageAsync(message, null, null);
        }

        private HttpMessage GetMessage(object message) 
        {
            switch(message)
            {
                case HttpMessage httpMessage:
                    return httpMessage;
                case string stringMessage:
                    var deserialized =  JsonConvert.DeserializeObject<HttpMessage>(stringMessage);
                    if(deserialized == null)
                        throw new EAIException("Only HttpMessage supported.");

                    return deserialized;
                default:
                    throw new EAIException("Only HttpMessage supported.");
            }
        }
    }
}
