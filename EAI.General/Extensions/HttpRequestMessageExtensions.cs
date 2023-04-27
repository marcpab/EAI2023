using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace EAI.General.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage Clone(this HttpRequestMessage httpRequestMessage)
        {
            var clonedMessage = new HttpRequestMessage(httpRequestMessage.Method, httpRequestMessage.RequestUri)
            {
                Content = httpRequestMessage.Content.Clone(),
                Version = httpRequestMessage.Version
            };

            foreach (KeyValuePair<string, IEnumerable<string>> header in httpRequestMessage.Headers)
                clonedMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);

            foreach (KeyValuePair<string, object> prop in httpRequestMessage.Properties)
                clonedMessage.Properties.Add(prop);

            return clonedMessage;
        }
    }
}
