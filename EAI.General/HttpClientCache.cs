using EAI.General.Cache;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EAI.General
{
    public class HttpClientCache
    {
        static HttpClientCache()
        {
            //Change max connections from .NET to a remote service default: 2
            System.Net.ServicePointManager.DefaultConnectionLimit = 32;
            //Bump up the min threads reserved for this app to ramp connections faster - minWorkerThreads defaults to 4, minIOCP defaults to 4 
            System.Threading.ThreadPool.SetMinThreads(32, 32);
            //Turn off the Expect 100 to continue message - 'true' will cause the caller to wait until it round-trip confirms a connection to the server 
            System.Net.ServicePointManager.Expect100Continue = false;
            //Can decrease overall transmission overhead but can cause delay in data packet arrival
            System.Net.ServicePointManager.UseNagleAlgorithm = false;
        }


        public static readonly TimeSpan DefaultTimeout = new TimeSpan(0, 6, 0);
        private static HttpClient _defaultClient;

        public static HttpClient GetDefaultClient()
        {
            if(_defaultClient != null)
                return _defaultClient;

            _defaultClient = new HttpClient() { Timeout = DefaultTimeout };

            return _defaultClient;
        }

        public static Task<HttpClient> GetHttpClientAsync(string key, Func<HttpClient> createHttpClientAsync, Func<ResourceCacheItem<HttpClient>, Task> updateCacheItemAsync) 
        {
            return ResourceCache<HttpClient>.GetResourceAsync(key, () => Task.FromResult(new ResourceCacheItem<HttpClient>(createHttpClientAsync())), updateCacheItemAsync);
        }
    }
}
