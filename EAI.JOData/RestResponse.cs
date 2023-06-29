using EAI.JOData.Base;
using System.Net;

namespace EAI.JOData
{
    public class RestResponse
    {
        public string Endpoint { get; private set; }
        public string? Method { get; private set; }
        public string? RequestMessage { get; private set; }
        public HttpStatusCode? StatusCode { get; private set; }
        public string? StatusReason { get; private set; }
        public string? StatusText { get; private set; }
        public string? Content { get; private set; }
        public KeyValuePair<string, IEnumerable<string>>[]? Headers { get; set; } = null;
        public IEnumerable<ExceptionEntry> Exception { get; private set; }
        public string Error { get; private set; }
        public bool IsSuccess { get; private set; } = false;


        public static string GetError(IEnumerable<ExceptionEntry> exception)
        {
            var msg = exception?.Select(x => x.Message);

            if (msg != null && msg.Count() > 0)
                return msg.Aggregate((x, y) => x + " " + y).Trim();

            return string.Empty;
        }

        public RestResponse(HttpResponseMessage msg, string endpoint, Exception? ex = null)
        {
            Endpoint = endpoint;
            Exception = ExceptionEntry.GetExceptionEntries(ex);
            Error = GetError(Exception);

            if (msg != null)
            {
                RequestMessage = msg.RequestMessage?.Content?.ReadAsStringAsync().Result;
                Method = msg.RequestMessage?.Method.ToString();
                IsSuccess = msg.IsSuccessStatusCode;
                StatusReason = msg.ReasonPhrase;
                StatusCode = msg.StatusCode;
                StatusText = msg.ToString();

                Content = msg.Content?.ReadAsStringAsync().Result;

                Headers = msg.Headers.ToArray();
            }
        }

        public RestResponse(HttpResponseMessage? msg, Uri endpoint, Exception? ex = null) 
            : this(msg, endpoint.ToString(), ex) { }
    }
}
