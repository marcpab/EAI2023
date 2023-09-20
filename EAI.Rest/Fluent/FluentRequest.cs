using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EAI.Rest.Fluent
{

    // restClient.Get(query).ResultAsString().WithCache(5).ExecuteAsync()
    // restClient.Get(query).ResultAs<account>().WithCache(5).ExecuteAsync()
    // restClient.Put(query, content).ExecuteAsync()
    // restClient.Put(query, content).ResultAs<account>().ExecuteAsync()



    public class FluentRequest
    {
        private readonly RestClient _restClient;

        private readonly RestRequest _restRequest;

        public FluentRequest(RestClient restClient, object query, string method, object content, Dictionary<string, string> requestHeaders = null)
        {
            _restClient = restClient;

            _restRequest = new RestRequest()
            {
                Method = new HttpMethod(method),
                Path = query.ToString(),
                Content = content,
                RequestHeaders = requestHeaders
            };
        }

        public FluentResponse<string> ResultAsString()
        {
            return new FluentResponse<string>(GetAsString, this);
        }

        private async Task<string> GetAsString()
        {
            var result = await _restClient.SendAsync(_restRequest);

            return await result.GetAsStringAsync();
        }

        public FluentResponse<T> ResultAs<T>()
        {
            return new FluentResponse<T>(GetAs<T>, this);
        }

        public async Task<T> GetAs<T>()
        {
            var result = await _restClient.SendAsync(_restRequest);

            return await result.GetAsSingleAsync<T>();
        }

        public IFluentStreamResponse<Stream> ResultAsStream<T>()
        {
            return new FluentResponse<Stream>(GetAsStream, this);
        }

        private async Task<Stream> GetAsStream()
        {
            var result = await _restClient.SendAsync(_restRequest);

            return await result.GetAsStreamAsync();
        }

        internal RestRequest GetRestRequest()
        {
            return _restRequest;
        }

        internal Uri GetFullUri()
        {
            return _restClient.GetFullUri(_restRequest);
        }
    }

}