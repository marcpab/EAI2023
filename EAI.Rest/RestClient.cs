using EAI.General.Cache;
using EAI.General.ExtendableHttp;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EAI.Rest
{
    public class RestClient : ExtendableHttpClient
    {
        private JsonSerializerSettings _serializerSettings;

        public RestClient()
        {
        }

        public JsonSerializerSettings SerializerSettings { get => _serializerSettings; set => _serializerSettings = value; }

        public async Task<RestResult> SendAsync(RestRequest requestData)
        {
            var requestMessage = requestData.CreateHttpRequestMessage(_serializerSettings);

            var response = await SendAsync(requestMessage);

            return new RestResult(response, _serializerSettings);
        }

        //public RestResult CreateRestResponse(RestRequest requestData)
        //{
        //    return new RestResult(this, requestData, SendAsync);
        //}

        public Uri GetFullUri(RestRequest restRequest)
        {
            if (BaseUri != null && restRequest.Path != null)
                return new Uri(BaseUri, restRequest.Path.ToString());

            if (BaseUri != null)
                return BaseUri;

            if (restRequest.Path != null)
                return new Uri(restRequest.Path.ToString());

            return null;
        }
    }
}
