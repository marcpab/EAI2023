using EAI.General.Cache;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;

namespace EAI.Rest
{
    public class RestResponse<T>
    {
        private RestRequest _request;

        public RestResponse(RestRequest request)
        {
            _request = request;
        }

        public async Task<string> GetAsStringAsync()
        {
            return await ResourceCache<string>.GetResourceAsync(
                _request.Uri.ToString(),
                async () => {
                        var response = await _request.GetResponseAsync();

                        var stringContent = await response.Content.ReadAsStringAsync();

                        return new ResourceCacheItem<string>(stringContent);
                    });
        }

        public async Task<T> GetAsSingleAsync<T>()
        {
            var response = await _request.GetResponseAsync();


        }

        public async Task<IEnumerable<T>> GetAsSingleAsync<T>()
        {
            var response = await _request.GetResponseAsync();


        }




    }
}