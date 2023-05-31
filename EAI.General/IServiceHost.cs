using System.Collections.Generic;

namespace EAI.General
{
    public interface IServiceHost
    {
        IEnumerable<T> GetServices<T>() where T : class;
    }
}