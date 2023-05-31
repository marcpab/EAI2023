using System;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.General
{
    public interface IRequestListener
    {
        void RegisterRequestHandler<requestT, responseT>(Func<requestT, Task<responseT>> message);

        Task RunAsync(CancellationToken cancellationToken);
    }
}