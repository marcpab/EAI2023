using System.Net.Http.Headers;

namespace EAI.HttpListener
{
    public interface IClientHttpAuth
    {
        Task<AuthenticationHeaderValue> GetAuthenticationHeaderValueAsync();
    }
}