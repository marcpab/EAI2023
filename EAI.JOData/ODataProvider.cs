using EAI.JOData.Base;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Net;
using System.Security;
using System.Text;
using System.Web;
using System.Xml.Linq;
using EAI.OAuth;
using System.Net.Mime;

namespace EAI.JOData
{
    public class ODataProvider : IDisposable
    {
        private static readonly TimeSpan DefaultTimeout = new(0, 5, 0);
        private HttpClient? _client = null;

        private static readonly string _mediaType = "application/json";
        private readonly Uri _odataUri = new(ODataVersion.DEFAULT, UriKind.Relative);
        private DateTime? _tokenExpiresOn = null;        
        private bool IsDisposed = false;
        private readonly Uri? Endpoint = null;
        private readonly JToken _jToken = JToken.Parse("{}");

        public ODataAuth AuthenticationDetails { get; set; }
        private OAuthResponse Token { get; set; } = new();
        public DateTime GetExpiresOn { get => _tokenExpiresOn ?? DateTime.Now; }


        private HttpClient GetClient()
        {
            if (_client is not null)
                return _client;

            _client = new HttpClient() 
            { 
                Timeout = DefaultTimeout,
                BaseAddress = Endpoint
            };

            return _client;
        }

        public ODataProvider(ODataAuth credentials, Uri endpoint, string? odataVersion = null)
        {
            if (string.IsNullOrWhiteSpace(odataVersion))
                odataVersion = ODataVersion.DEFAULT;
            else
                _odataUri = new Uri(odataVersion, UriKind.RelativeOrAbsolute);

            if (_odataUri.IsAbsoluteUri)
                throw new ArgumentException($"uri for odata version has to be relative but is absolute '{odataVersion}'");

            AuthenticationDetails = credentials;

            Endpoint = endpoint;

            _tokenExpiresOn = null;
        }

        private async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            if (_client is null || IsDisposed)
            {
                _client = GetClient();
                IsDisposed = false;
                _tokenExpiresOn = null;
            }

            if (_tokenExpiresOn is not null && _tokenExpiresOn > DateTime.Now.AddSeconds(5f))
                return _client;

            Token = await AuthenticationDetails.GetTokenAsync();
            _tokenExpiresOn = DateTime.Now.AddSeconds(Convert.ToDouble(Token.expires_in));

            string accessToken = Token.access_token;

            if (string.IsNullOrWhiteSpace(accessToken))
                throw new InvalidOperationException($"cannot obtain access token from oauth2 service!");

            // not thread safe will crash maybe -> reimplementation required IHttpClientFactory
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return _client;
        }

        /// <param name="record">when it finds any CtrlTokens, will remove them from the object</param>
        private static PreProcessRequest ValidateRecord(JObject? record, bool requiresId = false)
        {
            var pp = new PreProcessRequest();

            if (record is null)
            {
                pp.IsValid = false;
                pp.Error = "record is null";
                return pp;
            }

            if (!record.ContainsKey("@odata.type"))
            {
                pp.IsValid = false;
                pp.Error = "@odata.type property missing!";
                return pp;
            }

            var odataType = record?["@odata.type"]?.ToString();
            var entity = odataType?.Split('.').Last();
            if (string.IsNullOrWhiteSpace(entity))
            {
                pp.IsValid = false;
                pp.Error = $"@odata.type value = '{odataType}' doesnt contain an entity definition!";
                return pp;
            }

            pp.Entity = entity;
            pp.RecordIdName = $"{pp.Entity}id";

            pp.Tokens.Add(CtrlTokens.EnumerableEntity, JToken.FromObject($"{entity}s"));

            if (record?.ContainsKey(CtrlTokens.EnumerableEntity) ?? false)
            {
                var value = record[CtrlTokens.EnumerableEntity];
                if (value?.Type != JTokenType.Null)
                {
                    pp.Tokens[CtrlTokens.EnumerableEntity] = record?[CtrlTokens.EnumerableEntity] ?? JToken.Parse("{}");
                }

                record?.Property(CtrlTokens.EnumerableEntity)?.Remove();
            }

            if (record?.ContainsKey(CtrlTokens.AADImpersionationId) ?? false)
            {
                var value = record[CtrlTokens.AADImpersionationId];
                if (value is not null && value.Type != JTokenType.Null)
                {
                    pp.Tokens.Add(CtrlTokens.AADImpersionationId, value);
                }
                record?.Property(CtrlTokens.AADImpersionationId)?.Remove();
            }

            var hasId = false;
            if ((record is not null) && (record?.ContainsKey(pp.RecordIdName) ?? false))
            {
                pp.Tokens.Add(pp.RecordIdName, record[pp.RecordIdName]!);
                hasId = true;
            }

            if (requiresId && !hasId)
            {
                pp.IsValid = false;
                pp.Error = $"record key {pp.RecordIdName} entry is missing!";
                return pp;
            }

            return pp;
        }

        /*
         * POST [Organization URI]/api/data/v9.0/accounts(00000000-0000-0000-0000-000000000002)/opportunity_customer_accounts/$ref HTTP/1.1  
Content-Type: application/json  
Accept: application/json  
OData-MaxVersion: 4.0  
OData-Version: 4.0

{
"@odata.id":"[Organization URI]/api/data/v9.0/opportunities(00000000-0000-0000-0000-000000000001)"
}
         */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceEntity">accounts(00000000-0000-0000-0000-000000000002)</param>
        /// <param name="nameAssociation">opportunity_customer_accounts</param>
        /// <param name="targetEntity">opportunities(00000000-0000-0000-0000-000000000001)</param>
        /// <returns></returns>
        public async Task<ODataResponse> Associate(string nameAssociation, string lookupSource, string lookupTarget)
        {
            var client = await GetAuthenticatedClientAsync();

            var uri = new Uri($"{_odataUri}/{lookupSource}/{nameAssociation}/$ref", UriKind.Relative);
            var content = $"{{\"@odata.id\":\"{client?.BaseAddress?.Scheme}://{client?.BaseAddress?.Host}/{_odataUri}/{lookupTarget}\"}}";

#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var request = new HttpRequestMessage(HttpMethod.Post, uri))
            {
                request.Headers.Add("Accept", _mediaType);
                request.Headers.Add("Prefer", "return=representation");
                request.Content = new StringContent(content, Encoding.UTF8, _mediaType);

                try
                {
                    var response = await client?.SendAsync(request)!;
                    return new ODataResponse(response, uri, null, _jToken);
                }
                catch (Exception ex)
                {
                    return new ODataResponse((HttpResponseMessage?)null, uri, ex, _jToken);
                }
            }
#pragma warning restore IDE0063 // Use simple 'using' statement
        }

        public async Task<ODataResponse> Create(JObject? record)
        {
            var preProcess = ValidateRecord(record);

            if (!preProcess.IsValid)
                return new ODataResponse((HttpResponseMessage?)null, _odataUri, new ArgumentException($"{preProcess.Error}"), _jToken);

            var uri = new Uri($"{_odataUri}/{preProcess.Tokens[CtrlTokens.EnumerableEntity].Value<string>()}", UriKind.Relative);

#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var request = new HttpRequestMessage(HttpMethod.Post, uri))
            {
                request.Headers.Add("Accept", _mediaType);
                request.Headers.Add("Prefer", "return=representation");
                request.Content = new StringContent(record?.ToString() ?? "", Encoding.UTF8, _mediaType);

                try
                {
                    var response = await (await GetAuthenticatedClientAsync()).SendAsync(request);
                    return new ODataResponse(response, uri, null, _jToken);
                }
                catch (Exception ex)
                {
                    return new ODataResponse((HttpResponseMessage?)null, uri, ex, _jToken);
                }
            }
#pragma warning restore IDE0063 // Use simple 'using' statement
        }
        public async Task<ODataResponse> Create(JToken record) => await Create(record?.ToObject<JObject>());
        public async Task<ODataResponse> Create(string record) => await Create(JToken.Parse(record));
        public async Task<ODataResponse> Create(object record) => await Create(JsonConvert.SerializeObject(record, new ODataJsonConverter()));
        public async Task<ODataResponse> Create(ExpandoObject record) => await Create(JsonConvert.SerializeObject(record, new ODataJsonConverter()));

        public async Task<ODataResponse> Execute(string entity, Guid id, string action, JObject record)
        {
            var uri = new Uri($"{_odataUri}/{entity}({id})/Microsoft.Dynamics.CRM.{action}", UriKind.Relative);

            if (string.IsNullOrWhiteSpace(entity))
                return new ODataResponse((HttpResponseMessage?)null, uri, new ArgumentNullException($"record"), _jToken);
            if (id == Guid.Empty)
                return new ODataResponse((HttpResponseMessage?)null, uri, new ArgumentNullException($"id"), _jToken);
            if (string.IsNullOrWhiteSpace(action))
                return new ODataResponse((HttpResponseMessage?)null, uri, new ArgumentNullException($"action"), _jToken);

#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var request = new HttpRequestMessage(HttpMethod.Post, uri))
            {
                request.Headers.Add("Accept", _mediaType);
                request.Headers.Add("Prefer", "return=minimal");
                if (record != null)
                    request.Content = new StringContent(record.ToString(), Encoding.UTF8, _mediaType);

                try
                {
                    var response = await (await GetAuthenticatedClientAsync()).SendAsync(request);
                    return new ODataResponse(response, uri, null, _jToken);
                }
                catch (Exception ex)
                {
                    return new ODataResponse((HttpResponseMessage?)null, uri, ex, _jToken);
                }
            }
#pragma warning restore IDE0063 // Use simple 'using' statement
        }

        public async Task<ODataResponse> Delete(string enumerableEntity, Guid id)
        {
            var uri = new Uri($"{_odataUri}/{enumerableEntity}({id})", UriKind.Relative);

            if (string.IsNullOrWhiteSpace(enumerableEntity))
                return new ODataResponse((HttpResponseMessage?)null, uri, new ArgumentNullException(nameof(enumerableEntity)), _jToken);
            if (id == Guid.Empty)
                return new ODataResponse((HttpResponseMessage?)null, uri, new ArgumentNullException(nameof(id)), _jToken);

#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var request = new HttpRequestMessage(HttpMethod.Delete, uri))
            {
                request.Headers.Add("Prefer", "return=minimal");

                try
                {
                    var response = await (await GetAuthenticatedClientAsync()).SendAsync(request);
                    return new ODataResponse(response, uri, null, _jToken);
                }
                catch (Exception ex)
                {
                    return new ODataResponse((HttpResponseMessage?)null, uri, ex, _jToken);
                }
            }
#pragma warning restore IDE0063 // Use simple 'using' statement
        }

        public static JObject BuildFetch(string entity, string filter, int top = 0, bool islink = false)
        {
            var request = JObject.FromObject(new
                {
                    filter,
                    top,
                    islink
                });

            var property = new JProperty("@odata.type", $"#Microsoft.Dynamics.CRM.{entity}");
            request.First!.AddBeforeSelf(property);

            return request;
        }

        public async Task<ODataResponse> FetchPagingCustom(string enumerableEntity, string query, int top = 5000)
        {
            JArray records = new();
            var isLink = false;
            var filter = string.Empty;

            while (true)
            {
                // create request
                var request = ODataProvider.BuildFetch(enumerableEntity, filter, top, isLink);
                request?.Last?.AddAfterSelf(new JProperty("custom", query));
                request?.Last?.AddAfterSelf(new JProperty(CtrlTokens.EnumerableEntity, enumerableEntity));

                if (request is null)
                    throw new Exception("FetchPagingCustom request cannot be null here");

                // fetch
                var response = await Fetch(request);

                // process response
                if (!response.IsSuccess || response.Content is null || !response.Content.Contains("value"))
                    return new ODataResponse((JToken)records, new Uri(response.Endpoint, UriKind.RelativeOrAbsolute), response?.Response, _jToken, false);

                records.Merge(response.Content["value"] as JArray);
                
                filter = response.Content["@odata.nextLink"]?.Value<string>();
                if (filter is not null)
                {
                    isLink = true;
                }
                else
                    return new ODataResponse((JToken)records, new Uri(response.Endpoint, UriKind.RelativeOrAbsolute), response.Response, _jToken, true);
            }
        }


        public async Task<ODataResponse> FetchPaging(string enumerableEntity, string filter, int top = 5000)
        {
            JArray records = new();
            var isLink = false;

            if (filter is null)
                throw new ArgumentException(null, nameof(filter));

            while (true)
            {
                // create request
                var request = ODataProvider.BuildFetch(enumerableEntity, filter, top, isLink)!;
                request.Last?.AddAfterSelf(new JProperty("custom", filter));
                request.Last?.AddAfterSelf(new JProperty(CtrlTokens.EnumerableEntity, enumerableEntity));
                // fetch
                var response = (ODataResponse)await Fetch(request);

                // process response
                if (!response.IsSuccess)
                    return new ODataResponse((JToken)records, new Uri(response.Endpoint, UriKind.RelativeOrAbsolute), response.Response, _jToken, false);

                records.Merge(response.Content?["value"] as JArray);

                var intfilter = response.Content?["@odata.nextLink"]?.Value<string>();
                if (intfilter is not null)
                {
                    filter = intfilter;
                    isLink = true;
                }
                else
                    return new ODataResponse((JToken)records, new Uri(response.Endpoint, UriKind.RelativeOrAbsolute), response.Response, _jToken, true);
            }
        }

        public async Task<ODataResponse> FetchPaging<T>(string filter, int top = 5000) where T : IDataverseEntity, new()
            => await FetchPaging((new T()).NameCollection, filter, top);

        public async Task<ODataResponse> Metadata()
        {
            var uri = new Uri($"{_odataUri}/$metadata", UriKind.Relative);

#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                request.Headers.Add("Accept", "application/xml");

                try
                {
                    var response = await (await GetAuthenticatedClientAsync()).SendAsync(request);
                    return new ODataResponse(response, uri, null, _jToken);
                }
                catch (Exception ex)
                {
                    return new ODataResponse((HttpResponseMessage?)null, uri, ex, _jToken);
                }
            }
#pragma warning restore IDE0063 // Use simple 'using' statement
        }

        public async Task<ODataResponse> Fetch(string enumerableEntity, Guid key)
        {
            var uri = new Uri($"{_odataUri}/{enumerableEntity}({key})", UriKind.Relative);

#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                request.Headers.Add("Accept", _mediaType);
                request.Headers.Add("Prefer", $"odata.include-annotations=\"*\", return=representation");

                try
                {
                    var response = await (await GetAuthenticatedClientAsync()).SendAsync(request);
                    return new ODataResponse(response, uri, null, _jToken);
                }
                catch (Exception ex)
                {
                    return new ODataResponse((HttpResponseMessage?)null, uri, ex, _jToken);
                }
            }
#pragma warning restore IDE0063 // Use simple 'using' statement
        }

        public async Task<ODataResponse> Fetch<T>(Guid key) where T : IDataverseEntity, new()
            => await Fetch((new T()).NameCollection, key);

        public async Task<ODataResponse> Fetch(JObject? record)
        {
            var preProcess = ValidateRecord(record);

            if (!preProcess.IsValid)
                return new ODataResponse((HttpResponseMessage?)null, _odataUri, new ArgumentException($"{preProcess.Error}"), _jToken);

            var entity = preProcess.Tokens[CtrlTokens.EnumerableEntity].Value<string>();
            var uri = new Uri($"{_odataUri}/{entity}", UriKind.Relative);

            // read fetch params
            var top = 0;
            if (record?.ContainsKey("top") ?? false)
                top = record["top"]?.Value<int>() ?? 0;

            var filter = (string?)null;
            if (record?.ContainsKey("filter") ?? false)
                filter = record["filter"]?.Value<string>() ?? "";

            var custom = (string?)null;
            if (record?.ContainsKey("custom") ?? false)
                custom = record["custom"]?.Value<string>() ?? "";

            var isLink = false;
            if (record?.ContainsKey("islink") ?? false)
                isLink = record["islink"]?.Value<bool>() ?? false;
            else if (filter?.StartsWith("https:") ?? false)
                isLink = true;

            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(custom))
                return new ODataResponse((HttpResponseMessage?)null, uri, new ArgumentException($"record missing field 'filter' or 'custom'!"), _jToken);

            // filter is a paging link?
            if (isLink)
                uri = new Uri($"{filter}", UriKind.Absolute);
            else if (!string.IsNullOrWhiteSpace(custom))
                uri = new Uri($"{_odataUri}/{custom}", UriKind.Relative);
            else
                uri = new Uri($"{_odataUri}/{entity}?$filter={filter}", UriKind.Relative);

#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                request.Headers.Add("Accept", _mediaType);

                if (top > 0)
                    request.Headers.Add("Prefer", $"odata.maxpagesize={top}, odata.include-annotations=\"*\", return=representation");
                else
                    request.Headers.Add("Prefer", $"odata.include-annotations=\"*\", return=representation");

                try
                {
                    var response = await (await GetAuthenticatedClientAsync()).SendAsync(request);
                    return new ODataResponse(response, uri, null, _jToken);
                }
                catch (Exception ex)
                {
                    return new ODataResponse((HttpResponseMessage?)null, uri, ex, _jToken);
                }
            }
#pragma warning restore IDE0063 // Use simple 'using' statement
        }

        public async Task<ODataResponse> Fetch(JToken record) => await Fetch(record?.ToObject<JObject>());

        public async Task<ODataResponse> FetchXml(string enumerableEntity, XElement fetchXml)
        {
            var uri = new Uri($"{_odataUri}/{enumerableEntity}?fetchXml={HttpUtility.UrlEncode(fetchXml.ToString(SaveOptions.DisableFormatting))}", UriKind.Relative);

#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                request.Headers.Add("Accept", _mediaType);
                request.Headers.Add("Prefer", $"return=representation");

                try
                {
                    var response = await (await GetAuthenticatedClientAsync()).SendAsync(request);
                    return new ODataResponse(response, uri, null, _jToken);
                }
                catch (Exception ex)
                {
                    return new ODataResponse((HttpResponseMessage?)null, uri, ex, _jToken);
                }
            }
#pragma warning restore IDE0063 // Use simple 'using' statement
        }

        public async Task<ODataResponse> FetchXml<T>(XElement fetchXml) where T : IDataverseEntity, new()
            => await FetchXml((new T()).NameCollection, fetchXml);

        public async Task<ODataResponse> GetOptionSetOptions(string entityName, string attributeName)
        {
            var uri = new Uri($"{_odataUri}/EntityDefinitions(LogicalName='{entityName}')/Attributes(LogicalName='{attributeName}')/Microsoft.Dynamics.CRM.PicklistAttributeMetadata?$select=LogicalName&$expand=OptionSet($select=Options)", UriKind.Relative);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Prefer", "return=representation");

            try
            {
                var response = await (await GetAuthenticatedClientAsync()).SendAsync(request);
                return new ODataResponse(response, uri, null, _jToken);
            }
            catch (Exception ex)
            {
                return new ODataResponse((HttpResponseMessage?)null, uri, ex, _jToken);
            }
        }

        public async Task<ODataResponse> GetOptionSetOptions<T>(string attributeName) where T : IDataverseEntity, new()
            => await GetOptionSetOptions((new T()).Name, attributeName);

        public async Task<ODataResponse> GetStatusOptions(string entityName, string attributeName)
        {
            var uri = new Uri($"{_odataUri}/EntityDefinitions(LogicalName='{entityName}')/Attributes(LogicalName='{attributeName}')/Microsoft.Dynamics.CRM.StatusAttributeMetadata?$select=LogicalName&$expand=GlobalOptionSet($select=Options)", UriKind.Relative);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Prefer", "return=representation");

            try
            {
                var response = await (await GetAuthenticatedClientAsync()).SendAsync(request);
                return new ODataResponse(response, uri, null, _jToken);
            }
            catch (Exception ex)
            {
                return new ODataResponse((HttpResponseMessage?)null, uri, ex, _jToken);
            }
        }

        public async Task<ODataResponse> GetStatusOptions<T>(string attributeName) where T : IDataverseEntity, new()
            => await GetStatusOptions((new T()).Name, attributeName);

        public async Task<ODataResponse> GetOptionSetMetadata(string entityName, string attributeName)
        {
            var uri = new Uri($"{_odataUri}/EntityDefinitions(LogicalName='{entityName}')/Attributes(LogicalName='{attributeName}')/Microsoft.Dynamics.CRM.PicklistAttributeMetadata?$select=LogicalName&$expand=OptionSet", UriKind.Relative);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Prefer", "return=representation");

            try
            {
                var response = await (await GetAuthenticatedClientAsync()).SendAsync(request);
                return new ODataResponse(response, uri, null, _jToken);
            }
            catch (Exception ex)
            {
                return new ODataResponse((HttpResponseMessage?)null, uri, ex, _jToken);
            }
        }

        public async Task<ODataResponse> GetOptionSetMetadata<T>(string attributeName) where T : IDataverseEntity, new()
            => await GetOptionSetMetadata((new T()).Name, attributeName);

        public async Task<ODataResponse> Update(JObject? record)
        {
            var preProcess = ValidateRecord(record, true);

            if (!preProcess.IsValid)
                return new ODataResponse((HttpResponseMessage?)null, _odataUri, new ArgumentException($"{preProcess.Error}"), _jToken);

            var entity = preProcess.Tokens[CtrlTokens.EnumerableEntity].Value<string>();
            string id;
            try
            {
                id = preProcess?.Tokens[preProcess?.RecordIdName ?? ""]?.Value<string>() ?? string.Empty;
            }
            catch (Exception)
            {
                try
                {
                    id = preProcess.Tokens[preProcess?.RecordIdName ?? ""].Value<Guid>().ToString();
                }
                catch (Exception)
                {
                    return new ODataResponse((HttpResponseMessage?)null, _odataUri, new ArgumentException($"cannot parse record id token as string nor as guid!"), _jToken);
                }
            }
            string? impersonation = null;
            if (preProcess is not null &&
                preProcess.Tokens.ContainsKey(CtrlTokens.AADImpersionationId))
                impersonation = preProcess?.Tokens[CtrlTokens.AADImpersionationId]?.Value<string>();

            var uri = new Uri($"{_odataUri}/{entity}({id})", UriKind.Relative);

#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), uri))
            {
                request.Headers.Add("Accept", _mediaType);
                request.Headers.Add("Prefer", "return=representation");
                if (!string.IsNullOrWhiteSpace(impersonation))
                    request.Headers.Add("CallerObjectId", impersonation);

                request.Content = new StringContent(record?.ToString() ?? "", Encoding.UTF8, _mediaType);

                try
                {
                    var response = await (await GetAuthenticatedClientAsync()).SendAsync(request);
                    return new ODataResponse(response, uri, null, _jToken);
                }
                catch (Exception ex)
                {
                    return new ODataResponse((HttpResponseMessage?)null, uri, ex, _jToken);
                }
            }
#pragma warning restore IDE0063 // Use simple 'using' statement
        }
        public async Task<ODataResponse> Update(JToken record) => await Update(record?.ToObject<JObject>());
        public async Task<ODataResponse> Update(string record) => await Update(JToken.Parse(record));
        public async Task<ODataResponse> Update(object record) => await Update(JsonConvert.SerializeObject(record, new ODataJsonConverter()));
        public async Task<ODataResponse> Update(ExpandoObject record) => await Update(JsonConvert.SerializeObject(record, new ODataJsonConverter()));


        public void Dispose()
        {
            if (_client is not null && !IsDisposed)
            {
                GC.SuppressFinalize(this);
                IsDisposed = true;
                _client.Dispose();
            }
        }
    }
}