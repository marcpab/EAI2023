using System.Net.Http;
using System.Threading.Tasks;

namespace EAI.General.ExtendableHttp
{
    public interface IMessageHandler
    {
        IMessageHandler Next { get; set; }

        Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage requestMessage, HttpClient client);
    }
}