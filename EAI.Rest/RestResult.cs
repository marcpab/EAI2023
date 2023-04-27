using EAI.General.Cache;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EAI.Rest
{
    public class RestResult
    {
        private readonly HttpResponseMessage _httpResponse;
        private readonly JsonSerializerSettings _serializerSettings;

        public RestResult(HttpResponseMessage httpResponse, JsonSerializerSettings serializerSettings)
        {
            _httpResponse = httpResponse;
            _serializerSettings = serializerSettings;
        }

        public HttpResponseMessage OriginalHttpResponseMessage { get => _httpResponse; }

        public async Task<T> GetAsSingleAsync<T>()
        {
            var stringContent = await GetAsStringAsync();
                
            return JsonConvert.DeserializeObject<T>(stringContent, _serializerSettings);
        }

        public async Task<string> GetAsStringAsync()
        {
            return await _httpResponse.Content.ReadAsStringAsync();
        }

        public Task<Stream> GetAsStreamAsync()
        {
            return _httpResponse.Content.ReadAsStreamAsync();
        }
    }
}