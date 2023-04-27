using EAI.General.Cache;
using System;
using System.Threading.Tasks;

namespace EAI.Rest.Fluent
{
    public class FluentResponse<T> : IFluentStreamResponse<T>
    {
        private readonly Func<Task<T>> _getResponse;
        private readonly FluentRequest _fluentRequest;
        private TimeSpan _cacheTimeSpan;

        public FluentResponse(Func<Task<T>> getResponse, FluentRequest fluentRequest)
        {
            _getResponse = getResponse;
            _fluentRequest = fluentRequest;
        }

        public FluentResponse<T> CacheFor(TimeSpan cacheTimeSpan)
        {
            _cacheTimeSpan = cacheTimeSpan;

            return this;
        }

        public async Task<T> ExecuteAsync()
        {
            if (_cacheTimeSpan == default)
                return await _getResponse();

            return await ResourceCache<T>.GetResourceAsync(
                GetCacheKey(),
                async () =>
                {
                    var content = await _getResponse();

                    return new ResourceCacheItem<T>(content) { ExpiresOn = DateTimeOffset.UtcNow + _cacheTimeSpan };
                });
        }

        private string GetCacheKey()
        {
            return $"{_fluentRequest.GetFullUri()}-{typeof(T).FullName}";
        }

    }

}