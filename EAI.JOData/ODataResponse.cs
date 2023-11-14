using EAI.JOData.Base;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mime;
using System.Text.Json.Serialization;

namespace EAI.JOData
{
    public class ODataResponse
    {
        [JsonIgnore]
        private ResponseContentType _contentType;

        public RestResponse? Response { get; set; }
        public ResponseContentType ContentType { 
            get 
            {
                return _contentType;
            }
            private set
            {
                _contentType = value;
                ContentTypeName = _contentType.ToString();
            }
        }
        public string ContentTypeName { get; private set; } = "Undefined";
        public string Endpoint { get; set; }
        public HttpStatusCode? StatusCode { get; private set; }
        public string? StatusReason { get; private set; }
        public string? StatusText { get; private set; }
        public string? Message { get; set; }
        public JToken? Content { get; set; }
        public JToken Token { get; set; }
        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();
        public IEnumerable<ExceptionEntry>? Exception { get; private set; }
        public string? ClientError { get; set; }
        public string? ServerError { get; set; }
        public string? Error { get; set; }
        public bool IsSuccess { get; set; } = false;
        public bool IsJsonResponse { get; private set; } = false;

        public static (ResponseContentType ContentType, JToken? Content, string? Error, bool IsJson) ParseContent(JToken? jContent)
        {
            var contentType = ResponseContentType.Undefined;
            var isJson = false;
            var errorMsg = (string?)null;

            if (jContent is null)
            {
                return (contentType, jContent, errorMsg, isJson);
            }
            
            isJson = true;
            contentType = ResponseContentType.Json;

            if (jContent.SelectToken("error") is not null)
            {
                contentType = ResponseContentType.Error;
                var errcode = jContent.SelectToken("error")?.SelectToken("code");
                var errmsg = jContent.SelectToken("error")?.SelectToken("message");
                errorMsg = $"{errcode}: {errmsg}";

                return (contentType, jContent, errorMsg, isJson);
            }
            
            // is this a odata response?
            var odataCtxt = jContent.SelectToken("['@odata.context']")?.ToObject<Uri>();
            if (odataCtxt is null)
            {
                return (contentType, jContent, errorMsg, isJson);
            }

            contentType = ResponseContentType.OData;

            // test if we have a single record response
            if ((odataCtxt.Fragment?.EndsWith("$entity") ?? false))
            {
                contentType = ResponseContentType.Entity;

                if (odataCtxt.AbsolutePath.Contains("PicklistAttributeMetadata"))
                    contentType = ResponseContentType.LocalOptionSet;
                else if (odataCtxt.AbsolutePath.Contains("StatusAttributeMetadata"))
                    contentType = ResponseContentType.Status;

                return (contentType, jContent, errorMsg, isJson);
            }

            // test if we have a multiple record response
            if (jContent?.SelectToken("value") is not null)
            {
                contentType = ResponseContentType.EntityList;
            }

            return (contentType, jContent, errorMsg, isJson);
        }

        public static (ResponseContentType ContentType, JToken? Content, string? Error, bool IsJson) ParseContent(string? content)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(content))
                {
                    return ParseContent(JToken.Parse(content));
                }
            }
            catch (Exception) 
            {
            }

            return (ResponseContentType.Undefined, JToken.Parse("{}"), (string?)null, false);
        }

        public ODataResponse(RestResponse? response, string? message, Uri endpoint, JToken authToken)
        {
            Response = response;
            Endpoint = $"{endpoint}";
            StatusCode = Response?.StatusCode ?? 0;
            StatusReason = Response?.StatusReason ?? "";
            StatusText = Response?.StatusText ?? "";
            Message = string.IsNullOrWhiteSpace(message) ? Response?.Content : message;
            Token = authToken ?? JToken.Parse("{}");

            Exception = Response?.Exception;
            Error = ClientError = Response?.Error;
            IsSuccess = Response?.IsSuccess ?? false;

            (ContentType, Content, ServerError, IsJsonResponse) = ParseContent(Response?.Content);

            if (!string.IsNullOrWhiteSpace(ClientError) ||
                (int)ContentType < 10)
            {
                IsSuccess = false;
            }

            if (!string.IsNullOrWhiteSpace(ServerError))
            {
                IsSuccess = false;
                Error = ServerError;
            }

            if (Response?.Headers is not null)
            {
                foreach (var h in Response.Headers)
                    Headers.Add(h.Key, h.Value.Last());
            }
        }

        public ODataResponse(HttpResponseMessage? msg, Uri endpoint, Exception? ex, JToken authToken)
            : this(new RestResponse(msg, endpoint, ex), null, endpoint, authToken) { }

        public ODataResponse(string message, Uri endpoint, Exception? ex, JToken authToken)
            : this(new RestResponse(null, endpoint, ex), message, endpoint, authToken) { }
                
        public ODataResponse(JToken? content, Uri endpoint, Exception ex, JToken authToken)
            : this(content, endpoint, new RestResponse(null, endpoint, ex), authToken, false) { }
                
        public ODataResponse(JToken? content, Uri endpoint, RestResponse? response, JToken token, bool isOverrideContent)
            : this(response, null, endpoint, token)
        {
            var reparse = true;
            Message = content?.ToString();
            Content = content;

            if (content is null || !content.HasValues)
            {
                if (!isOverrideContent && Response?.Content is not null)
                {
                    reparse = false;
                }
            }

            if(reparse)
                (ContentType, Content, Error, IsJsonResponse) = ParseContent(content);

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
            if (ContentType == ResponseContentType.EntityList)
                return Content?["value"]?.Children().ToList();

            if (ContentType == ResponseContentType.Entity)
                return new List<JToken>() { Content ?? JToken.Parse("{}") };

            if (ContentType == ResponseContentType.LocalOptionSet)
                return Content?.SelectTokens("['OptionSet']['Options']")?.Children().ToList();

            if (ContentType == ResponseContentType.Status)
                return Content?.SelectTokens("['GlobalOptionSet']['Options']")?.Children().ToList();

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
