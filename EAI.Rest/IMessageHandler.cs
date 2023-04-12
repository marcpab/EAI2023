using System.Net.Http;
using System.Threading.Tasks;

namespace EAI.Rest
{
    public interface IMessageHandler
    {
        Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage requestMessage, HttpClient client);
    }
}