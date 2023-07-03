using EAI.JOData.Base;
using Newtonsoft.Json.Linq;
using System.Net;

namespace EAI.JOData
{
    public class ODataResponse
    {
        public RestResponse? Response { get; set; }
        public ResponseContentType ContentType { get; private set; }
        public string Endpoint { get; set; }
        public HttpStatusCode? StatusCode { get; private set; }
        public string? StatusReason { get; private set; }
        public string? StatusText { get; private set; }
        public string? Message { get; set; }
        public JToken? Content { get; set; }
        public JToken Token { get; set; }
        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();
        public IEnumerable<ExceptionEntry>? Exception { get; private set; }
        public string? Error { get; set; }
        public string SoftError { get; set; }
        public bool IsSuccess { get; set; } = false;
        public bool IsJsonResponse { get; private set; } = false;


        private static ResponseContentType SetContentType(Uri endpoint, ResponseContentType type = ResponseContentType.Undefined)
        {
            if (type != ResponseContentType.Undefined)
                return type;
            if (endpoint.ToString().Contains("Microsoft.Dynamics.CRM.PicklistAttributeMetadata?"))
                return ResponseContentType.LocalOptionSet;
            else if (endpoint.ToString().Contains("Microsoft.Dynamics.CRM.StatusAttributeMetadata?"))
                return ResponseContentType.Status;

            return ResponseContentType.EntityList;
        }

        public ODataResponse(HttpResponseMessage? msg, Uri endpoint, Exception? ex, JToken token, ResponseContentType type = ResponseContentType.Undefined)
        {
            Response = new RestResponse(msg, endpoint, ex);
            Endpoint = $"{endpoint}";
            StatusCode = msg?.StatusCode;
            StatusReason = msg?.ReasonPhrase;
            StatusText = msg?.ToString();
            Message = Response.Content;
            Token = token ?? JToken.Parse("{}");
            SoftError = string.Empty;

            ContentType = SetContentType(endpoint, type);

            try
            {
                if (Response?.Content is not null)
                {
                    Content = JToken.Parse(Response.Content);
                    IsJsonResponse = true;

                    if (Content.SelectToken("error") is not null)
                    {
                        var errcode = Content.SelectToken("error")?.SelectToken("code");
                        var errmsg = Content.SelectToken("error")?.SelectToken("message");
                        SoftError = $"{errcode}: {errmsg}";
                    }
                }
            }
            catch (Exception) { IsJsonResponse = false; }

            if (Response?.Headers is not null)
            {
                foreach (var h in Response.Headers)
                    Headers.Add(h.Key, h.Value.Last());
            }

            Exception = Response?.Exception;
            Error = Response?.Error;
            IsSuccess = Response?.IsSuccess ?? false;
        }

        public ODataResponse(string content, Uri endpoint, Exception ex, JToken token, ResponseContentType type = ResponseContentType.Undefined)
            : this(content, endpoint, new RestResponse(null, endpoint, ex), token, type) { }

        public ODataResponse(string content, Uri endpoint, RestResponse response, JToken token, ResponseContentType type = ResponseContentType.Undefined)
        {
            Response = response;
            Endpoint = $"{endpoint}";
            StatusCode = Response.StatusCode;
            StatusReason = Response.StatusReason;
            StatusText = Response.StatusText;
            Token = token ?? JToken.Parse("{}");
            SoftError = string.Empty;

            ContentType = SetContentType(endpoint, type);

            Message = content;
            if (string.IsNullOrWhiteSpace(content))
                Message = Response.Content;

            try
            {
                if (Response?.Content is not null)
                {
                    Content = JToken.Parse(Response.Content);
                    IsJsonResponse = true;

                    if (Content.SelectToken("error") is not null)
                    {
                        var errcode = Content.SelectToken("error")?.SelectToken("code");
                        var errmsg = Content.SelectToken("error")?.SelectToken("message");
                        SoftError = $"{errcode}: {errmsg}";
                    }
                }
            }
            catch (Exception)
            {
                IsJsonResponse = false;
            }

            Exception = Response?.Exception;
            Error = Response?.Error;
            IsSuccess = Response?.IsSuccess ?? false;
        }

        public ODataResponse(JToken content, Uri endpoint, Exception ex, JToken token, ResponseContentType type = ResponseContentType.Undefined)
            : this(content, endpoint, new RestResponse(null, endpoint, ex), token, type) { }

        public ODataResponse(JToken content, Uri endpoint, RestResponse? response, JToken token, ResponseContentType type = ResponseContentType.Undefined, bool overrideContent = false)
        {
            Response = response;
            Endpoint = $"{endpoint}";
            StatusCode = Response?.StatusCode ?? 0;
            StatusReason = Response?.StatusReason ?? "";
            StatusText = Response?.StatusText ?? "";
            Token = token ?? JToken.Parse("{}");
            SoftError = string.Empty;

            ContentType = SetContentType(endpoint, type);

            Message = content?.ToString();
            Content = content;

            if (content is null || !content.HasValues)
            {
                if (!overrideContent && Response?.Content is not null)
                {
                    try
                    {
                        Content = JToken.Parse(Response.Content);
                        IsJsonResponse = true;
                        Message = response?.Content;

                        if (Content.SelectToken("error") is not null)
                        {
                            var errcode = Content.SelectToken("error")?.SelectToken("code");
                            var errmsg = Content.SelectToken("error")?.SelectToken("message");
                            SoftError = $"{errcode}: {errmsg}";
                        }
                    }
                    catch (Exception)
                    {
                        IsJsonResponse = false;
                    }
                }
            }

            IsJsonResponse = true;

            Exception = Response?.Exception;
            Error = Response?.Error;
            IsSuccess = Response?.IsSuccess ?? false;
        }

        /// <summary>
        /// returns null when not an os result set
        /// </summary>
        /// <returns>null or dictionary</returns>
        public Dictionary<int, Dictionary<int, string>>? GetContentAsOptionSet()
        {
            if (Content is null)
                return null;

            if (ContentType != ResponseContentType.LocalOptionSet && ContentType != ResponseContentType.Status)
                return null;

            var result = new Dictionary<int, Dictionary<int, string>>();

            var key = "OptionSet";
            if (ContentType == ResponseContentType.Status)
                key = "GlobalOptionSet";

            var options = Content[key]?["Options"]?.Children();
            if (options is null)
                return null;

            foreach (var o in options)
            {
                var value = o["Value"]?.ToObject<int>();
                var localLabels = o["Label"]?["LocalizedLabels"]?.Children();

                if (localLabels is null)
                    continue;

                if (value is null)
                    continue;

                foreach (var l in localLabels)
                {
                    var label = l["Label"]?.ToString();
                    var code = l["LanguageCode"]?.ToObject<int>();

                    if (label is null)
                        continue;
                    if (code is null)
                        continue;


                    if (result.ContainsKey((int)code))
                    {
                        var labelDic = result[(int)code];
                        labelDic.Add((int)value, label);
                        result[(int)code] = labelDic;
                    }
                    else
                    {
                        var labelDic = new Dictionary<int, string>
                        {
                            { (int)value, label }
                        };
                        result.Add((int)code, labelDic);
                    }
                }
            }

            return result;
        }

        public Dictionary<int, string?>? GetContentAsOptionExternal()
        {
            if (Content is null)
                return null;

            if (ContentType != ResponseContentType.LocalOptionSet && ContentType != ResponseContentType.Status)
                return null;

            var result = new Dictionary<int, string?>();

            var key = "OptionSet";
            if (ContentType == ResponseContentType.Status)
                key = "GlobalOptionSet";

            var options = Content[key]?["Options"]?.Children();
            if (options is null)
                return null;

            foreach (var o in options)
            {
                var value = o["Value"]?.ToObject<int>();
                var external = o["ExternalValue"]?.ToString();

                if(value is not null)
                    result.Add((int)value, external);
            }

            return result;
        }

        /// <summary>
        /// returns null when not an os result set
        /// </summary>
        /// <param name="languageCode">ms lcid</param>
        /// <returns>null or dictionary</returns>
        public Dictionary<int, string>? GetContentAsOptionSet(int languageCode) 
            => GetContentAsOptionSet()?[languageCode];

        public List<JToken>? ToList()
        {
            if (Content is null)
                return null;

            if (ContentType == ResponseContentType.Entity)
                return Content?.Children().ToList();

            if (ContentType == ResponseContentType.EntityList)
                return Content["value"]?.Children().ToList();

            return null;
        }

        public List<JToken>? ToValueList()
        {
            if (Content?.SelectToken("value") is not null)
                return Content?["value"]?.Children().ToList();

            return null;
        }

        public (string? Code, string? Message) GetError()
        {
            var jError = Content?.SelectToken("error");

            // in case no error than both are null
            var errcode = (string?)jError?.SelectToken("code");
            var errmsg = (string?)jError?.SelectToken("message");

            return (errcode, errmsg);
        }

        public JToken? GetToken(int row, string key)
        {
            var jtokens = ToList();

            if (ContentType == ResponseContentType.Entity)
                return jtokens?
                .Select(x => (JProperty)x)
                .Where(y => y.Name == key)
                .SingleOrDefault()?
                .Value;

            if (jtokens is null || jtokens.Count is 0)
                return null;

            return jtokens?[row]?
                .Select(x => (JProperty)x)
                .Where(y => y.Name == key)
                .SingleOrDefault()?
                .Value;
        }

        public string? this[int row, string key]
        {
            get
            {
                var jtokens = ToList();

                if (ContentType == ResponseContentType.Entity)
                    return jtokens?
                    .Select(x => (JProperty)x)
                    .SingleOrDefault(y => y.Name == key)?
                    .Value.ToString();

                if (jtokens is null || jtokens.Count is 0)
                    return null;

                return jtokens?[row]?
                    .Select(x => (JProperty)x)
                    .Where(y => y.Name == key)
                    .SingleOrDefault()?
                    .Value.ToString();
            }
        }

        public string? this[string key]
        {
            get
            {
                return this[0, key];
            }
        }

    }
}
