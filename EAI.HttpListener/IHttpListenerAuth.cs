using System.Collections.Specialized;

namespace EAI.HttpListener
{
    public interface IHttpListenerAuth
    {
        bool IsAuthorized(NameValueCollection headers);
    }
}