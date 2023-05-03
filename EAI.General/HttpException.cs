using System;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Serialization;

namespace EAI.General
{
    [Serializable]
    internal class HttpException : Exception
    {
        private readonly Uri _requestUri;
        private readonly HttpStatusCode _statusCode;
        private readonly string _statusReasonPhrase;
        private readonly string _content;

        public Uri RequestUri { get => _requestUri; }

        public HttpStatusCode StatusCode { get => _statusCode; }

        public string StatusReasonPhrase { get => _statusReasonPhrase; }

        public string Content { get => _content; }

        public HttpException(Uri requestUri, HttpStatusCode statusCode, string statusReasonPhrase, HttpResponseHeaders headers, string content, Exception innerException) : this($"Request to {requestUri} failed: {(int)statusCode}, {statusReasonPhrase}: {content}", innerException)
        {
            _requestUri = requestUri;
            _statusCode = statusCode;
            _statusReasonPhrase = statusReasonPhrase;
            _content = content;
        }


        public HttpException()
        {
        }

        public HttpException(string message) : base(message)
        {
        }

        public HttpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}