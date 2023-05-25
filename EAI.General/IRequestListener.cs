using System;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.General
{
    public interface IRequestListener
    {
        Task RunAsync(Func<string, Task<string>> processRequest, CancellationToken cancellationToken);
    }
}