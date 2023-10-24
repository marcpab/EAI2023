using EAI.General;
using EAI.LoggingV2;
using EAI.LoggingV2.Levels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace EAI.HttpListener
{
    internal class RequestHandler
    {
        private LoggerV2 _log;
        private string _listenUri;
        private HttpListenerDestination[] _httpListenerDestination;
        private IHttpListenerAuth _auth;
        private HttpListenerContext _originalContext;
        private HttpListenerRequest _originalRequest;


        public RequestHandler(LoggerV2 log, string listenUri, HttpListenerDestination[] httpListenerDestination, IHttpListenerAuth auth, HttpListenerContext originalContext)
        {
            _log = log;
            _listenUri = listenUri;
            _httpListenerDestination = httpListenerDestination;
            _auth = auth;
            _originalContext = originalContext;
            _originalRequest = _originalContext.Request;
        }

        public async Task ProcessRequestAsync()
        {
            using (var _ = new ProcessScope())
                try
                {
                    var originalRequest = _originalContext.Request;

                    _log?.Start<Info>(null, null, $"Received a {originalRequest.HttpMethod} request for " + originalRequest.RawUrl);

                    CheckAuthorization();

                    var relayRequestMessage = await GetMessageAsync();

                    _log?.Message<Debug>(nameof(relayRequestMessage), relayRequestMessage, "http message");

                    await TryLogContentAsync(relayRequestMessage);

                    await SendMessageAsync(relayRequestMessage);

                    _log.Success<Info>();

                }
                catch (UnauthorizedAccessException ex)
                {
                    _log?.Failed<Error>(ex);

                    SendUnauthorizedResponse();
                }
                catch (Exception ex)
                {
                    _log?.Failed<Error>(ex);

                    SendExceptionResponse(ex);
                }
                finally
                {
                    _originalContext.Response.Close();
                }
        }

        private async Task TryLogContentAsync(HttpMessage relayRequestMessage)
        {
            try
            {
                using(var stream = new  MemoryStream())
                {
                    stream.Write(relayRequestMessage._content);
                    await stream.FlushAsync();
                    stream.Position = 0;
                    using (var content = new StreamContent(stream))
                    {
                        foreach(var header in relayRequestMessage._headers)
                            content.Headers.TryAddWithoutValidation(header.Key, header.Value);

                        var stringContent = await content.ReadAsStringAsync();

                        _log?.Message<Debug>(nameof(stringContent), stringContent, "http content");
                    }
                }
            }
            catch (Exception )
            {
                // ignore possible decoding errors here
            }
        }

        private Task SendMessageAsync(HttpMessage relayRequestMessage)
        {
            if (_httpListenerDestination != null)
                foreach (var destination in _httpListenerDestination)
                    if (destination.IsMatch(relayRequestMessage._uri))
                        return destination.SendMessage(relayRequestMessage);

            return Task.CompletedTask;
        }

        private void CheckAuthorization()
        {
            if (_auth != null)
            {
                if (!_auth.IsAuthorized(_originalRequest.Headers))
                    throw new UnauthorizedAccessException();
            }
        }

        private async Task<HttpMessage> GetMessageAsync()
        {
            var queueMessage = new HttpMessage()
            {
                _method = _originalRequest.HttpMethod,
                _uri = _originalRequest.RawUrl,
                _version = _originalRequest.ProtocolVersion.ToString(),

                _headers = GetHeaders(_originalRequest.Headers),
                _content = await GetContentAsync(_originalRequest.InputStream)

            };

            return queueMessage;
        }

        private async Task<byte[]> GetContentAsync(Stream sourceStream)
        {
            var bufferStream = new MemoryStream();
            await sourceStream.CopyToAsync(bufferStream);

            bufferStream.Position = 0;

            var data = new byte[bufferStream.Length];

            await bufferStream.ReadAsync(data, 0, (int)bufferStream.Length);

            return data;
        }

        private Dictionary<string, string> GetHeaders(NameValueCollection headers)
            => headers.AllKeys.ToDictionary(k => k, k => headers[k]);

        private void SendUnauthorizedResponse()
        {
            _log?.String<Warning>("HttpStatusCode: Unauthorized");

            var originalResponse = _originalContext.Response;
//            originalResponse.AddHeader("Content-Type", SimpleResponseContentType ?? string.Empty);

            originalResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
            originalResponse.StatusDescription = HttpStatusCode.Unauthorized.ToString();

            originalResponse.AddHeader("WWW-Authenticate", $"Basic realm=\"{_listenUri}\", charset=\"UTF-8\"");

            originalResponse.ContentLength64 = 0;

            originalResponse.OutputStream.Flush();
            originalResponse.OutputStream.Close();
        }

        private void SendExceptionResponse(Exception ex)
        {
            _log?.String<Warning>("HttpStatusCode: InternalServerError");

            var originalResponse = _originalContext.Response;

            originalResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
            originalResponse.StatusCode = 500;
            originalResponse.StatusDescription = ex.Message;

            originalResponse.OutputStream.Flush();
            originalResponse.OutputStream.Close();
        }
    }
}
